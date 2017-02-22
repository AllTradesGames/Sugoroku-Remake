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
    public GameObject playerBoxCardPrefab;
    public GameObject selectableCardPrefab;

    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject restButton;
    public GameObject restConfirmPanel;
    public GameObject attackConfirmPanel;
    public GameObject combatOptionsPanel;
    public GameObject defendingCardPanel;
    public GameObject attackingCardPanel;
    public GameObject surrenderItemsPanel;

    private DataControl dataScript;
    private int activePlayer = 0;
    private bool hasActivePlayerMoved = false;
    private bool hasActivePlayerAttacked = false;
    private int tempInt = 0;
    private float tempFloat = 0f;
    private bool tempBool = false;
    private GameObject tempObject = null;
    private GameObject currentCard = null;
    private int defendingCharacterIndex = -1;
    private ActionType currentCombatAction;
    private bool failedRun = false;

    private int activePlayerRoll = 0;
    private int defendingCharacterRoll = 0;

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
            playerModels.Add(Instantiate(Resources.Load(dataScript.playerList[ii].pathToModel), new Vector3(5 + (ii * 10), 0f, 5f), Quaternion.identity) as GameObject);
        }
        for (int ii = 0; ii < playerModels.Count; ii++)
        {
            playerModels[ii].GetComponent<CharacterControl>().characterIndex = ii;
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
                currentCard = Instantiate(playerBoxCardPrefab, tempObject.transform) as GameObject;
                currentCard.transform.SetAsFirstSibling();
                currentCard.transform.localScale = Vector3.one;
                // TODO                                                                                              vvv   Make that not hardcoded.
                currentCard.transform.position = new Vector3(tempObject.transform.FindChild("Card").position.x + (jj*35f), tempObject.transform.FindChild("Card").position.y, tempObject.transform.FindChild("Card").position.z);
                switch(playerHands[ii][jj].type)
                {
                    case CardType.Move:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.blue;
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
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.green;
                        break;
                    case CardType.Attack:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.red;
                        break;
                    case CardType.Defense:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[ii][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.yellow;
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
        attackConfirmPanel.SetActive(false);
        RemoveClickCheckers();
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


    public void EndTurn()
    {
        activePlayer++;
        if (activePlayer == dataScript.playerList.Count)
        {
            activePlayer = 0;
        }
        hasActivePlayerAttacked = false;
        hasActivePlayerMoved = false;
        StartTurn(activePlayer);
    }


    public void CardSelected(int cardIndex, ActionType currentAction)
    {
        // TODO Apply buffs based on current action
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
        if (!hasActivePlayerAttacked)
        {
            // Check nearby spaces for characters and add ClickCheckers
            foreach (GameObject model in playerModels)
            {
                if (model.GetComponent<CharacterControl>().characterIndex != activePlayer)
                {
                    if (Vector3.Magnitude(model.transform.position - playerModels[activePlayer].transform.position) < 12f)
                    {
                        model.AddComponent<ClickChecker>();
                        model.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }


    public void AttackCharacterClicked(CharacterControl characterScript)
    {
        if (!characterScript.isMonster)
        {
            defendingCharacterIndex = characterScript.characterIndex;
        }
        /*else
        {
            attackedCharacter = monsterList[characterScript.characterIndex];
        }*/

        attackConfirmPanel.transform.FindChild("Text").GetComponent<Text>().text = "Attack " + dataScript.playerList[defendingCharacterIndex].name + "?";
        attackConfirmPanel.SetActive(true);

    }


    public void AttackCharacterConfirmed()
    {
        RemoveClickCheckers();

        StartCombat();
    }


    public void RemoveClickCheckers()
    {
        // Remove click checkers
        foreach (ClickChecker cc in GameObject.FindObjectsOfType<ClickChecker>())
        {
            cc.gameObject.GetComponent<Renderer>().material.color = Color.grey;
            Destroy(cc);
        }
    }


    public void StartCombat()
    {
        Debug.Log("Start combat between " + dataScript.playerList[activePlayer].name + " and " + dataScript.playerList[defendingCharacterIndex].name);
        failedRun = false;
        attackConfirmPanel.SetActive(false);
        ShowCombatOptions();
    }


    public void ShowCombatOptions()
    {
        Debug.Log("Show Combat Options");
        combatOptionsPanel.SetActive(true);
    }


    public void CounterattackSelected()
    {
        currentCombatAction = ActionType.CounterAttack;
        combatOptionsPanel.SetActive(false);
        ShowCards(defendingCharacterIndex, currentCombatAction);
    }


    public void DefendSelected()
    {
        currentCombatAction = ActionType.Defend;
        combatOptionsPanel.SetActive(false);
        ShowCards(defendingCharacterIndex, currentCombatAction);
    }


    public void RunSelected()
    {
        currentCombatAction = ActionType.Run;
        combatOptionsPanel.SetActive(false);
        ShowCards(defendingCharacterIndex, currentCombatAction);
    }


    public void SurrenderSelected()
    {
        tempBool = false;
        foreach(int index in dataScript.playerList[defendingCharacterIndex].itemIndices)
        {
            if(index !=0)
            {
                tempBool = true;
            }
        }
        if (tempBool)
        {
            currentCombatAction = ActionType.Surrender;
            combatOptionsPanel.SetActive(false);
            ShowSurrenderItems();
        }
    }


    public void ShowSurrenderItems()
    {
        // Show surrendering player's items
        surrenderItemsPanel.SetActive(true);
        tempObject = surrenderItemsPanel.transform.FindChild("Item Buttons").gameObject;
        for (int ii = 0; ii < DataControl.NUM_ITEMS_PER_CHARACTER; ii++)
        {
            if ((dataScript.playerList[defendingCharacterIndex].itemIndices.Length > ii) && (dataScript.playerList[defendingCharacterIndex].itemIndices[ii] != 0))
            {
                tempObject.transform.GetChild(ii).FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(dataScript.pathToItemImages + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].imageName);
                if (!dataScript.playerList[defendingCharacterIndex].identifiedItems[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]])
                {
                    tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = "???";
                }
                else
                {
                    switch (dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].itemEffect)
                    {
                        case (int)ItemEffect.None:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\n";
                            break;
                        case (int)ItemEffect.Attack:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nAtk +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Defense:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nDef +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Movement:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nMv +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Evade:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nEv Trap +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount + "%";
                            break;
                        case (int)ItemEffect.Escape:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nEscape +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.CritAttack:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCrit Attack x2";
                            break;
                        case (int)ItemEffect.CritDefend:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCrit Defense x2";
                            break;
                        case (int)ItemEffect.CritEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCrit Empty Hand";
                            break;
                        case (int)ItemEffect.CritStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCrit Stun";
                            break;
                        case (int)ItemEffect.CritLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCrit Leg Dmg";
                            break;
                        case (int)ItemEffect.Voodoo:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nVoodoo";
                            break;
                        case (int)ItemEffect.MoveHeal:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nMove Heal +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.RestUp:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRest Heal +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Crutch:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nLeg Dmg Mv +" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.NoPanic:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nNo Panic";
                            break;
                        case (int)ItemEffect.NoStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nNo Stun";
                            break;
                        case (int)ItemEffect.NoLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nNo Leg Dmg";
                            break;
                        case (int)ItemEffect.NoEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nNo Empty Hand";
                            break;
                        case (int)ItemEffect.RollCap5:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRoll Max is 4" + dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.CausePanic:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRandom Panic";
                            break;
                        case (int)ItemEffect.CauseEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRandom Empty Hand";
                            break;
                        case (int)ItemEffect.CauseStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRandom Stun";
                            break;
                        case (int)ItemEffect.CauseLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRandom Leg Dmg";
                            break;
                        case (int)ItemEffect.NPC0NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nGON No Counter";
                            break;
                        case (int)ItemEffect.NPC1NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nCAL No Counter";
                            break;
                        case (int)ItemEffect.NPC2NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nBRO No Counter";
                            break;
                        case (int)ItemEffect.NPC3NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[defendingCharacterIndex].itemIndices[ii]].name + "\nRAD No Counter";
                            break;
                    }
                }
                tempObject.transform.GetChild(ii).gameObject.SetActive(true);
            }
            else
            {
                tempObject.transform.GetChild(ii).gameObject.SetActive(false);
            }
        }
        tempObject = null;
    }


    public void SurrenderItemConfirmed(int itemIndex)
    {
        surrenderItemsPanel.SetActive(false);
        tempInt = dataScript.playerList[defendingCharacterIndex].itemIndices[itemIndex];
        dataScript.playerList[defendingCharacterIndex].itemIndices[itemIndex] = 0;
        for (int ii=0; ii < dataScript.playerList[activePlayer].itemIndices.Length; ii++)
        {
            if (dataScript.playerList[activePlayer].itemIndices[ii] == 0)
            {
                dataScript.playerList[activePlayer].itemIndices[ii] = tempInt;
                TeleportPlayer(defendingCharacterIndex);
                FinishCombat();
                return;
            }
        }        
    }


    public void ShowCards(int playerIndex, ActionType inputAction)
    {
        if (playerIndex == activePlayer)
        {
            if (inputAction == ActionType.Attack)
            {
                // Show attacking player's cards
                attackingCardPanel.SetActive(true);
                tempObject = attackingCardPanel.transform.FindChild("Card Parent").gameObject;
                tempObject.transform.FindChild("Card").SetAsLastSibling();
                for (int kk = tempObject.transform.FindChild("Card").GetSiblingIndex(); kk > 0; kk--)
                {
                    Destroy(tempObject.transform.GetChild(kk - 1).gameObject);
                }
                for (int jj = 0; jj < playerHands[activePlayer].Count; jj++)
                {
                    currentCard = Instantiate(selectableCardPrefab, tempObject.transform) as GameObject;
                    currentCard.transform.SetAsFirstSibling();
                    currentCard.transform.localScale = Vector3.one;
                    // TODO                                                                                                vvv   Make that not hardcoded.
                    currentCard.transform.position = new Vector3(tempObject.transform.FindChild("Card").position.x + (jj * 83f), tempObject.transform.FindChild("Card").position.y, tempObject.transform.FindChild("Card").position.z);
                    switch (playerHands[activePlayer][jj].type)
                    {
                        case CardType.Move:
                            currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[activePlayer][jj].amount;
                            currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.blue;
                            if (currentCombatAction == ActionType.Run)
                            {
                                currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = true;
                            }
                            else
                            {
                                currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = false;
                            }
                            break;
                        case CardType.Trap:
                            switch ((TrapType)playerHands[activePlayer][jj].amount)
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
                            currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.green;
                            currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = false;
                            break;
                        case CardType.Attack:
                            currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[activePlayer][jj].amount;
                            currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.red;
                            break;
                        case CardType.Defense:
                            currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[activePlayer][jj].amount;
                            currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.yellow;
                            break;
                    }
                    switch (jj)
                    {
                        case 0:
                            currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { AttackerCardChosen(0); });
                            break;
                        case 1:
                            currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { AttackerCardChosen(1); });
                            break;
                        case 2:
                            currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { AttackerCardChosen(2); });
                            break;
                        case 3:
                            currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { AttackerCardChosen(3); });
                            break;
                        case 4:
                            currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { AttackerCardChosen(4); });
                            break;
                    }
                }
                tempObject.transform.FindChild("Card").gameObject.SetActive(false);
            }
            else if (inputAction == ActionType.Move)
            {
                // Show moving player's cards
                // delegate MovingCardChosen(int cardIndex)            
            }
        }
        else if (playerIndex == defendingCharacterIndex)
        {
            // Show defending player's cards
            defendingCardPanel.SetActive(true);
            tempObject = defendingCardPanel.transform.FindChild("Card Parent").gameObject;
            tempObject.transform.FindChild("Card").SetAsLastSibling();
            for (int kk = tempObject.transform.FindChild("Card").GetSiblingIndex(); kk > 0; kk--)
            {
                Destroy(tempObject.transform.GetChild(kk - 1).gameObject);
            }
            for (int jj = 0; jj < playerHands[defendingCharacterIndex].Count; jj++)
            {
                currentCard = Instantiate(selectableCardPrefab, tempObject.transform) as GameObject;
                currentCard.transform.SetAsFirstSibling();
                currentCard.transform.localScale = Vector3.one;
                // TODO                                                                                                vvv   Make that not hardcoded.
                currentCard.transform.position = new Vector3(tempObject.transform.FindChild("Card").position.x + (jj * 83f), tempObject.transform.FindChild("Card").position.y, tempObject.transform.FindChild("Card").position.z);
                switch (playerHands[defendingCharacterIndex][jj].type)
                {
                    case CardType.Move:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[defendingCharacterIndex][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.blue;
                        if (currentCombatAction == ActionType.Run)
                        {
                            currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = false;
                        }
                        break;
                    case CardType.Trap:
                        switch ((TrapType)playerHands[defendingCharacterIndex][jj].amount)
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
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.green;
                        currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = false;
                        break;
                    case CardType.Attack:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[defendingCharacterIndex][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.red;
                        if (currentCombatAction == ActionType.CounterAttack)
                        {
                            currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = false;
                        }
                        break;
                    case CardType.Defense:
                        currentCard.transform.FindChild("Text").GetComponent<Text>().text = "+" + playerHands[defendingCharacterIndex][jj].amount;
                        currentCard.transform.FindChild("Button").GetComponent<Image>().color = Color.yellow;
                        currentCard.transform.FindChild("Button").GetComponent<Button>().interactable = true;
                        break;
                }
                switch (jj)
                {
                    case 0:
                        currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { DefenderCardChosen(0); });
                        break;
                    case 1:
                        currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { DefenderCardChosen(1); });
                        break;
                    case 2:
                        currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { DefenderCardChosen(2); });
                        break;
                    case 3:
                        currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { DefenderCardChosen(3); });
                        break;
                    case 4:
                        currentCard.transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(delegate { DefenderCardChosen(4); });
                        break;
                }
            }
            tempObject.transform.FindChild("Card").gameObject.SetActive(false);
        }
    }


    public void DefenderCardChosen(int cardIndex)
    {
        defendingCardPanel.SetActive(false);
        if (cardIndex >= 0)
        {
            switch (playerHands[defendingCharacterIndex][cardIndex].type)
            {
                case CardType.Attack:
                    dataScript.playerList[defendingCharacterIndex].attackBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
                case CardType.Defense:
                    dataScript.playerList[defendingCharacterIndex].defenseBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
                case CardType.Move:
                    dataScript.playerList[defendingCharacterIndex].movementBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
            }
            playerHands[defendingCharacterIndex].RemoveAt(cardIndex);
        }
        ShowCards(activePlayer, ActionType.Attack);
    }


    public void AttackerCardChosen(int cardIndex)
    {
        attackingCardPanel.SetActive(false);
        if (cardIndex >= 0)
        {
            switch (playerHands[activePlayer][cardIndex].type)
            {
                case CardType.Attack:
                    dataScript.playerList[activePlayer].attackBonus += playerHands[activePlayer][cardIndex].amount;
                    break;
                case CardType.Defense:
                    dataScript.playerList[activePlayer].defenseBonus += playerHands[activePlayer][cardIndex].amount;
                    break;
                case CardType.Move:
                    dataScript.playerList[activePlayer].movementBonus += playerHands[activePlayer][cardIndex].amount;
                    break;
            }
            playerHands[activePlayer].RemoveAt(cardIndex);
        }
        if (currentCombatAction == ActionType.Run)
        {
            ShowDice(ActionType.Run);
        }
        else
        {
            ShowDice(ActionType.Attack);
        }
    }


    public void MovingCardChosen(int cardIndex)
    {
        if (cardIndex >= 0)
        {
            switch (playerHands[activePlayer][cardIndex].type)
            {
                case CardType.Attack:
                    dataScript.playerList[activePlayer].attackBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
                case CardType.Defense:
                    dataScript.playerList[activePlayer].defenseBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
                case CardType.Move:
                    dataScript.playerList[activePlayer].movementBonus += playerHands[defendingCharacterIndex][cardIndex].amount;
                    break;
                case CardType.Trap:
                    // TODO Place a Trap at current location
                    break;
            }
        }
    }


    public void ShowDice(ActionType currentAction)
    {
        // Show and roll dice based on current action
        switch(currentAction)
        {
            case ActionType.Attack:
                activePlayerRoll = (int)Random.Range(-0.49f, 6.49f) + (int)Random.Range(-0.49f, 6.49f);
                Debug.Log(dataScript.playerList[activePlayer].name + " rolled " + activePlayerRoll + " on attack");
                activePlayerRoll += dataScript.playerList[activePlayer].attackBonus;                

                defendingCharacterRoll = (int)Random.Range(0.51f, 6.49f) + (int)Random.Range(0.51f, 6.49f);
                Debug.Log(dataScript.playerList[defendingCharacterIndex].name + " rolled " + defendingCharacterRoll + " on defense");
                if (!failedRun)
                {
                    defendingCharacterRoll += dataScript.playerList[defendingCharacterIndex].defenseBonus;
                    if (currentCombatAction == ActionType.Defend)
                    {
                        defendingCharacterRoll += dataScript.playerList[defendingCharacterIndex].defenseBonus;
                    }
                }

                ApplyDamage(defendingCharacterIndex, activePlayerRoll - defendingCharacterRoll);
                break;
            case ActionType.CounterAttack:
                activePlayerRoll = (int)Random.Range(-0.49f, 6.49f) + (int)Random.Range(-0.49f, 6.49f);
                Debug.Log(dataScript.playerList[activePlayer].name + " rolled " + activePlayerRoll + " on defense");
                activePlayerRoll += dataScript.playerList[activePlayer].defenseBonus;

                defendingCharacterRoll = (int)Random.Range(-0.49f, 6.49f) + (int)Random.Range(-0.49f, 6.49f);
                Debug.Log(dataScript.playerList[defendingCharacterIndex].name + " rolled " + defendingCharacterRoll + " on attack");
                defendingCharacterRoll += dataScript.playerList[defendingCharacterIndex].attackBonus;

                ApplyDamage(activePlayer, defendingCharacterRoll - activePlayerRoll);
                break;
            case ActionType.Run:
                failedRun = false;
                activePlayerRoll = (int)Random.Range(-0.49f, 6.49f) + (int)Random.Range(-0.49f, 6.49f);
                Debug.Log(dataScript.playerList[activePlayer].name + " rolled " + activePlayerRoll + " on chase");
                activePlayerRoll += dataScript.playerList[activePlayer].movementBonus;

                defendingCharacterRoll = (int)Random.Range(-0.49f, 6.49f) + (int)Random.Range(-0.49f, 6.49f);
                Debug.Log(dataScript.playerList[defendingCharacterIndex].name + " rolled " + defendingCharacterRoll + " on escape");
                defendingCharacterRoll += dataScript.playerList[defendingCharacterIndex].movementBonus;

                if ((activePlayerRoll - defendingCharacterRoll) > 0)
                {
                    failedRun = true;
                    ShowDice(ActionType.Attack);
                }
                else
                {
                    FinishCombat();
                }
                break;
            case ActionType.Move:
                // TODO Move stuff yo
                break;
        }
    }


    public void ApplyDamage(int targetCharacterIndex, int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }
        dataScript.playerList[targetCharacterIndex].currentHP -= amount;
        Debug.Log(dataScript.playerList[targetCharacterIndex].name + " took " + amount + " damage");
        if (dataScript.playerList[targetCharacterIndex].currentHP <= 0)
        {
            dataScript.playerList[targetCharacterIndex].currentHP = 0;
            FinishCombat();
        }
        else
        {
            if ((targetCharacterIndex == defendingCharacterIndex) && (currentCombatAction == ActionType.CounterAttack))
            {
                ShowDice(currentCombatAction);
            }
            else
            {
                FinishCombat();
            }
        }
    }


    public void CharacterDied(int targetCharacterIndex)
    {
        Debug.Log(dataScript.playerList[targetCharacterIndex].name + " died :(");
        TeleportPlayer(targetCharacterIndex);
        dataScript.playerList[targetCharacterIndex].tempMaxHP = Mathf.CeilToInt(dataScript.playerList[targetCharacterIndex].tempMaxHP / 2);
        dataScript.playerList[targetCharacterIndex].currentHP = dataScript.playerList[targetCharacterIndex].tempMaxHP;
        // TODO Make player stunned for next turn
    }


    public void FinishCombat()
    {
        Debug.Log("Combat finished");
        // Remove combat buffs
        dataScript.UpdateStatBonus(activePlayer);
        dataScript.UpdateStatBonus(defendingCharacterIndex);

        // Check participants for critical damage
        if (dataScript.playerList[activePlayer].currentHP == 0)
        {
            CharacterDied(activePlayer);
        }
        if (dataScript.playerList[defendingCharacterIndex].currentHP == 0)
        {
            CharacterDied(defendingCharacterIndex);
        }

        // Reset combat variables
        hasActivePlayerAttacked = true;
        tempInt = 0;
        tempFloat = 0f;
        tempBool = false;
        tempObject = null;
        currentCard = null;
        defendingCharacterIndex = -1;
        currentCombatAction = ActionType.Move;
        failedRun = false;
        activePlayerRoll = 0;
        defendingCharacterRoll = 0;

        // Update UI
        InitializePlayerBoxes();

        if (hasActivePlayerMoved)
        {
            EndTurn();
        }
    }


    public void RestButtonClicked()
    {
        RemoveClickCheckers();
        attackConfirmPanel.SetActive(false);
        if (!hasActivePlayerMoved && !hasActivePlayerAttacked)
        {
            restConfirmPanel.transform.FindChild("Text").GetComponent<Text>().text = "Use your turn to Rest and heal " + Mathf.CeilToInt(dataScript.playerList[activePlayer].tempMaxHP * restRatio) + "HP?";
            restConfirmPanel.SetActive(true);
        }
        else
        {
            EndTurn();
        }
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
        EndTurn();
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


    public void TeleportPlayer(int playerIndex)
    {
        // TODO
    }


}
