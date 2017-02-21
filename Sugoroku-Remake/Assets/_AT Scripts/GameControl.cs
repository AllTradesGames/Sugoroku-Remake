using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TrapType
{
    Leg, 
    Stun, 
    Empty, 
    Damage
}

public enum ActionType
{
    Move, 
    Attack, 
    Defend, 
    Run,
    CounterAttack,
    Surrender
}

public class GameControl : MonoBehaviour
{
    public int startingHandSize;
    public int maxHandSize;
    public float restRatio = 0.25f;
    public Vector3 cameraOffset = new Vector3(65f, 85f, -65f);

    public Text cardsRemainingText;
    public Deck deck = new Deck();
    public List<List<Card>> playerHands = new List<List<Card>>();
    public List<GameObject> playerModels = new List<GameObject>();

    public GameObject playerBoxPlaceholder;
    public GameObject playerBoxPrefab;
    public GameObject cardPrefab;

    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject restButton;
    public GameObject restConfirmPanel;

    private DataControl dataScript;
    private int activePlayer = 0;
    private bool hasActivePlayerMoved = false;
    private bool hasActivePlayerAttacked = false;
    private int tempInt = 0;
    private float tempFloat = 0f;
    private GameObject tempObject = null;
    private GameObject currentCard = null;

    private void Awake()
    {
        dataScript = GameObject.FindGameObjectWithTag("DataController").GetComponent<DataControl>();
        Random.InitState((int)(Time.time * 100f));
    }

	// Use this for initialization
	void Start ()
    {
        moveButton.SetActive(false);
        attackButton.SetActive(false);
        restButton.SetActive(false);
        for (int ii=0; ii < dataScript.playerList.Count; ii++)
        {
            playerHands.Add(new List<Card>());
            DrawCards(ii, startingHandSize);
            playerModels.Add(Instantiate(Resources.Load(dataScript.playerList[ii].pathToModel), new Vector3(5 + (ii * 10), 0f, 5 + (ii * 10)), Quaternion.identity) as GameObject);
        }
        InitializePlayerBoxes();
        StartTurn(activePlayer);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void DrawCards(int playerIndex, int numCards)
    {
        for (int ii = 0; ii < numCards; ii++)
        {
            if ((deck.cards.Count > 0) && (playerHands[playerIndex].Count < maxHandSize))
            {
                tempInt = (int)Random.Range(-0.49f, (deck.cards.Count - 0.51f));
                playerHands[playerIndex].Add(deck.cards[tempInt]);
                deck.cards.RemoveAt(tempInt);
            }
        }
    }


    private void InitializePlayerBoxes()
    {
        playerBoxPlaceholder.transform.SetAsLastSibling();
        for (int ii = playerBoxPlaceholder.transform.GetSiblingIndex(); ii > 0; ii--)
        {
            Destroy(playerBoxPlaceholder.transform.parent.GetChild(ii - 1).gameObject);
        }

        tempInt = 0;
        tempObject = null;
        tempFloat = playerBoxPlaceholder.transform.position.x;
        for (int ii=0; ii < dataScript.playerList.Count; ii++)
        {
            if (tempObject)
            {
                // TODO                                        vvvv   Make that not hardcoded.
                tempFloat = tempObject.transform.position.x + 209.5f;
            }
            tempObject = Instantiate(playerBoxPrefab, playerBoxPlaceholder.transform.parent) as GameObject;
            tempObject.transform.SetSiblingIndex(tempInt);
            tempObject.transform.position = new Vector3(tempFloat, playerBoxPlaceholder.transform.position.y, playerBoxPlaceholder.transform.position.z);
            tempObject.transform.FindChild("Character Name").GetComponent<Text>().text = dataScript.playerList[ii].name;
            tempObject.transform.FindChild("Level Text").GetComponent<Text>().text = "Lv " + dataScript.playerList[ii].level;
            tempObject.transform.FindChild("Stat_Card Parent/Attack/Attack Text").GetComponent<Text>().text = dataScript.playerList[ii].attackBonus.ToString();
            tempObject.transform.FindChild("Stat_Card Parent/Movement/Movement Text").GetComponent<Text>().text = "+" + dataScript.playerList[ii].movementBonus.ToString();
            tempObject.transform.FindChild("Stat_Card Parent/Defense/Defense Text").GetComponent<Text>().text = dataScript.playerList[ii].defenseBonus.ToString();
            // tempObject.transform.FindChild("Credits/Credits Text").GetComponent<Text>().text = dataScript.playerList[ii].credits.ToString();
            tempObject.transform.FindChild("Health/HP Text").GetComponent<Text>().text = dataScript.playerList[ii].currentHP + "/" + dataScript.playerList[ii].tempMaxHP;
            tempObject.transform.FindChild("Health/Red Slider").GetComponent<Slider>().value = ((float)dataScript.playerList[ii].tempMaxHP) / ((float)dataScript.playerList[ii].maxHP);
            tempObject.transform.FindChild("Health/Red Slider/Green Slider").GetComponent<Slider>().value = ((float)dataScript.playerList[ii].currentHP) / ((float)dataScript.playerList[ii].maxHP);

            // Populate Player's Cards
            tempObject = tempObject.transform.FindChild("Stat_Card Parent/Card Parent").gameObject;
            tempObject.transform.FindChild("Card").SetAsLastSibling();
            for (int kk = tempObject.transform.FindChild("Card").GetSiblingIndex(); kk > 0; kk--)
            {
                Destroy(tempObject.transform.GetChild(kk - 1).gameObject);
            }
            for (int jj=0; jj < playerHands[ii].Count; jj++)
            {
                currentCard = Instantiate(cardPrefab, tempObject.transform) as GameObject;
                currentCard.transform.SetAsFirstSibling();
                currentCard.transform.localScale = Vector3.one;
                // TODO                                                                                                                           vvv   Make that not hardcoded.
                currentCard.transform.position = new Vector3(tempObject.transform.FindChild("Card").position.x + (jj*35f), tempObject.transform.FindChild("Card").position.y, tempObject.transform.FindChild("Card").position.z);
                switch(playerHands[ii][jj].type)
                {
                    case CardType.Move:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Image").GetComponent<Image>().color = Color.blue;
                        break;
                    case CardType.Trap:
                        switch((TrapType)playerHands[ii][jj].amount)
                        {
                            case TrapType.Damage:
                                currentCard.transform.FindChild("Text").GetComponent<Text>().text = "D";
                                break;
                            case TrapType.Empty:
                                currentCard.transform.FindChild("Text").GetComponent<Text>().text = "E";
                                break;
                            case TrapType.Stun:
                                currentCard.transform.FindChild("Text").GetComponent<Text>().text = "S";
                                break;
                            case TrapType.Leg:
                                currentCard.transform.FindChild("Text").GetComponent<Text>().text = "L";
                                break;
                        }
                        currentCard.transform.FindChild("Image").GetComponent<Image>().color = Color.green;
                        break;
                    case CardType.Attack:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Image").GetComponent<Image>().color = Color.red;
                        break;
                    case CardType.Defense:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Image").GetComponent<Image>().color = Color.yellow;
                        break;
                }

            }
            tempObject.transform.parent.gameObject.SetActive(true);
            tempObject.transform.FindChild("Card").gameObject.SetActive(false);

            // Populate Player's Items
            tempObject = tempObject.transform.parent.parent.FindChild("Item Parent/Images").gameObject;
            for (int jj = 0; jj < DataControl.NUM_ITEMS_PER_CHARACTER; jj++)
            {
                if ((dataScript.playerList[ii].itemIndices.Length > jj) && (dataScript.playerList[ii].itemIndices[jj] != 0))
                {
                    tempObject.transform.GetChild(jj).GetComponent<Image>().sprite = Resources.Load<Sprite>(dataScript.pathToItemImages + dataScript.masterItemListClass.list[dataScript.playerList[ii].itemIndices[jj]].imageName);
                }
                else
                {
                    tempObject.transform.GetChild(jj).GetComponent<Image>().sprite = null;
                }
            }
            tempObject.transform.parent.gameObject.SetActive(false);
            tempObject = tempObject.transform.parent.parent.gameObject;

            switch (tempInt)
            {
                case 0:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { PlayerClicked(0); });
                    break;
                case 1:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { PlayerClicked(1); });
                    break;
                case 2:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { PlayerClicked(2); });
                    break;
                case 3:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { PlayerClicked(3); });
                    break;
            }
            tempInt++;
        }

        playerBoxPlaceholder.SetActive(false);
        tempObject = null;
    }


    public void PlayerClicked(int playerIndex)
    {
        TogglePlayerItems(playerIndex);
    }


    public void MoveButtonClicked()
    {
        restConfirmPanel.SetActive(false);
    }


    public void StartTurn(int playerIndex)
    {
        Debug.Log("Starting " + dataScript.playerList[playerIndex].name + "'s turn");
        activePlayer = playerIndex;
        moveButton.SetActive(true);
        attackButton.SetActive(true);
        restButton.SetActive(true);
        
        Camera.main.transform.position = playerModels[activePlayer].transform.position + cameraOffset;
        Camera.main.transform.LookAt(playerModels[activePlayer].transform.position);

        DrawCards(activePlayer, 1);
        InitializePlayerBoxes();
    }


    public void EndTurn(int playerIndex)
    {
        activePlayer = playerIndex + 1;
        if (activePlayer == dataScript.playerList.Count)
        {
            activePlayer = 0;
        }
        
        StartTurn(activePlayer);
    }


    public void ShowCards(int playerIndex, ActionType currentAction)
    {
        // TODO Show player's hand so cards can be selected
    }


    public void CardSelected(int cardIndex, ActionType currentAction)
    {
        // TODO Apply buffs based on current action
    }


    public void ShowDice(ActionType currentAction)
    {
        // TODO Show and roll dice based on current action
    }


    public void ShowMoveTiles()
    {
        // TODO Show active character's move tiles (should be done after applying all movement buffs and rolling movement die)
    }


    public void MoveTileClicked()
    {

    }


    public void AttackButtonClicked()
    {
        restConfirmPanel.SetActive(false);
    }


    public void RestButtonClicked()
    {
        restConfirmPanel.transform.FindChild("Text").GetComponent<Text>().text = "Use your turn to Rest and heal " + Mathf.CeilToInt(dataScript.playerList[activePlayer].tempMaxHP * restRatio) + "HP?";
        restConfirmPanel.SetActive(true);
    }


    public void RestConfirmed()
    {
        restConfirmPanel.SetActive(false);
        dataScript.playerList[activePlayer].currentHP += Mathf.CeilToInt(dataScript.playerList[activePlayer].tempMaxHP * restRatio);
        if(dataScript.playerList[activePlayer].currentHP > dataScript.playerList[activePlayer].tempMaxHP)
        {
            dataScript.playerList[activePlayer].currentHP = dataScript.playerList[activePlayer].tempMaxHP;
        }
        DrawCards(activePlayer, 3);
        InitializePlayerBoxes();
        EndTurn(activePlayer);
    }

    public void TogglePlayerItems(int playerIndex)
    {
        tempObject = playerBoxPlaceholder.transform.parent.GetChild(playerIndex).gameObject;
        if(tempObject.transform.FindChild("Stat_Card Parent").gameObject.activeInHierarchy)
        {
            tempObject.transform.FindChild("Stat_Card Parent").gameObject.SetActive(false);
            tempObject.transform.FindChild("Item Parent").gameObject.SetActive(true);
        }
        else
        {
            tempObject.transform.FindChild("Stat_Card Parent").gameObject.SetActive(true);
            tempObject.transform.FindChild("Item Parent").gameObject.SetActive(false);
        }
    }


}
