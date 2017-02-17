using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl_Main : MonoBehaviour
{
    public DataControl dataScript;

    public string pathToItemImages = "/Images/Items/";

    public GameObject startMissionButton;

    public GameObject playerBoxPlaceholder;
    public GameObject playerBoxPrefab;
    public GameObject addPlayerButton;

    public GameObject savedCharacterPlaceholder;
    public GameObject savedCharacterPrefab;

    public GameObject addPlayerPanel;
    public GameObject startMissionPanel;
    public GameObject exitGamePanel;
    public GameObject soundOptionsPanel;
    public GameObject createNewCharacterPanel;
    public GameObject inviteFriendPanel;
    public GameObject selectedCharacterPanel;
    public GameObject levelUpPanel;
    public GameObject cannotLevelUpPanel;
    public GameObject restoreHPPanel;
    public GameObject removePlayerPanel;
    public GameObject deleteCharacterPanel;
    public GameObject itemOptionsPanel;
    public GameObject sellItemPanel;
    public GameObject identifyItemPanel;

    private bool isDeleteActive = false;
    private int activePlayer = -1;
    private int activeCharacter = -1;
    private int activeItem = -1;
    private GameObject tempObject;
    private float tempFloat = 0f;
    private int tempInt = 0;
    

	// Use this for initialization
	void Start ()
    {
        CheckStartMissionStatus();
        InitializePlayerBoxes();
        InitializeSavedCharacterBoxes();
    }


    private void CheckStartMissionStatus()
    { // TODO include a call to this function when a new character is created
        if (dataScript.playerList.Count > 0)
        {
            startMissionButton.SetActive(true);
        }
        else
        {
            startMissionButton.SetActive(false);
        }
    }


    private void InitializePlayerBoxes()
    {
        playerBoxPlaceholder.transform.SetAsLastSibling();
        for (int ii=playerBoxPlaceholder.transform.GetSiblingIndex(); ii > 0; ii--)
        {
            Destroy(playerBoxPlaceholder.transform.parent.GetChild(ii - 1).gameObject);
        }

        tempInt = 0;
        tempObject = null;
        tempFloat = playerBoxPlaceholder.transform.position.x;
        foreach (Character character in dataScript.playerList)
        {
            if (tempObject)
            {
                // TODO                                        vvvv   Make that not hardcoded.
                tempFloat = tempObject.transform.position.x + 209.5f;
            }
            tempObject = Instantiate(playerBoxPrefab, playerBoxPlaceholder.transform.parent) as GameObject;
            tempObject.transform.SetSiblingIndex(tempInt);
            tempObject.transform.position = new Vector3(tempFloat, playerBoxPlaceholder.transform.position.y, playerBoxPlaceholder.transform.position.z);
            tempObject.transform.FindChild("Character Name").GetComponent<Text>().text = character.name;
            tempObject.transform.FindChild("Level Text").GetComponent<Text>().text = "Lv " + character.level;
            tempObject.transform.FindChild("Attack/Attack Text").GetComponent<Text>().text = character.attackBonus.ToString();
            tempObject.transform.FindChild("Movement/Movement Text").GetComponent<Text>().text = character.movementBonus.ToString();
            tempObject.transform.FindChild("Defense/Defense Text").GetComponent<Text>().text = character.defenseBonus.ToString();
            tempObject.transform.FindChild("Credits/Credits Text").GetComponent<Text>().text = character.credits.ToString();
            tempObject.transform.FindChild("Health/HP Text").GetComponent<Text>().text = character.currentHP + "/" + character.tempMaxHP;
            tempObject.transform.FindChild("Health/Red Slider").GetComponent<Slider>().value = ((float)character.tempMaxHP) / ((float)character.maxHP);
            tempObject.transform.FindChild("Health/Red Slider/Green Slider").GetComponent<Slider>().value = ((float)character.currentHP) / ((float)character.maxHP);
            switch(tempInt)
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
        if (tempObject)
        {
            // TODO                                                                             vvvv   Make that not hardcoded.
            addPlayerButton.transform.position = new Vector3(tempObject.transform.position.x + 179.4f, addPlayerButton.transform.position.y, addPlayerButton.transform.position.z);
        }
        else
        {
            // TODO                                                 vvvv   Make that not hardcoded.
            addPlayerButton.transform.localPosition = new Vector3(-323.95f, addPlayerButton.transform.localPosition.y, addPlayerButton.transform.localPosition.z);
        }
        playerBoxPlaceholder.SetActive(false);
        tempObject = null;
    }


    private void InitializeSavedCharacterBoxes()
    {
        savedCharacterPlaceholder.transform.SetAsLastSibling();
        for (int ii = savedCharacterPlaceholder.transform.GetSiblingIndex(); ii > 0; ii--)
        {
            Destroy(savedCharacterPlaceholder.transform.parent.GetChild(ii - 1).gameObject);
        }

        tempInt = 0;
        tempObject = null;
        tempFloat = savedCharacterPlaceholder.transform.position.y;
        foreach (Character character in dataScript.savedCharacterListClass.list)
        {
            if (tempObject)
            {
                // TODO                                        vvvv   Make that not hardcoded.
                tempFloat = tempObject.transform.position.y - 151.3f;
            }
            tempObject = Instantiate(savedCharacterPlaceholder, savedCharacterPlaceholder.transform.parent) as GameObject;
            if(tempObject.transform.parent.gameObject.activeInHierarchy)
            {
                tempObject.SetActive(true);
            }
            tempObject.transform.SetSiblingIndex(tempInt);
            tempObject.transform.position = new Vector3(savedCharacterPlaceholder.transform.position.x, tempFloat, savedCharacterPlaceholder.transform.position.z);
            tempObject.transform.FindChild("Character Name").GetComponent<Text>().text = character.name;
            tempObject.transform.FindChild("Level Text").GetComponent<Text>().text = "Lv " + character.level;
            tempObject.transform.FindChild("Attack/Attack Text").GetComponent<Text>().text = character.attackBonus.ToString();
            tempObject.transform.FindChild("Movement/Movement Text").GetComponent<Text>().text = character.movementBonus.ToString();
            tempObject.transform.FindChild("Defense/Defense Text").GetComponent<Text>().text = character.defenseBonus.ToString();
            tempObject.transform.FindChild("Credits/Credits Text").GetComponent<Text>().text = character.credits.ToString();
            tempObject.transform.FindChild("Health/HP Text").GetComponent<Text>().text = character.currentHP + "/" + character.tempMaxHP;
            tempObject.transform.FindChild("Health/Red Slider").GetComponent<Slider>().value = ((float)character.tempMaxHP) / ((float)character.maxHP);
            tempObject.transform.FindChild("Health/Red Slider/Green Slider").GetComponent<Slider>().value = ((float)character.currentHP) / ((float)character.maxHP);
            switch (tempInt)
            {
                case 0:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(0); });
                    break;
                case 1:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(1); });
                    break;
                case 2:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(2); });
                    break;
                case 3:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(3); });
                    break;
                case 4:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(4); });
                    break;
                case 5:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(5); });
                    break;
                case 6:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(6); });
                    break;
                case 7:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(7); });
                    break;
                case 8:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(8); });
                    break;
                case 9:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(9); });
                    break;
                case 10:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(10); });
                    break;
                case 11:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(11); });
                    break;
                case 12:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(12); });
                    break;
                case 13:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(13); });
                    break;
                case 14:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(14); });
                    break;
                case 15:
                    tempObject.GetComponent<Button>().onClick.AddListener(delegate { SavedCharacterClicked(15); });
                    break;
            }

            tempInt++;
        }
        savedCharacterPlaceholder.SetActive(false);
        tempObject = null;
    }


    // Update is called once per frame
    void Update ()
    {

    }


    private void DeactivateAllPanels()
    {
        addPlayerPanel.SetActive(false);
        startMissionPanel.SetActive(false);
        exitGamePanel.SetActive(false);
        soundOptionsPanel.SetActive(false);
        createNewCharacterPanel.SetActive(false);
        inviteFriendPanel.SetActive(false);
        selectedCharacterPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        cannotLevelUpPanel.SetActive(false);
        restoreHPPanel.SetActive(false);
        removePlayerPanel.SetActive(false);
        deleteCharacterPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);
    }


    public void AddPlayerClicked()
    {
        Debug.Log("AddPlayerClicked()");
        DeactivateAllPanels();
        addPlayerPanel.SetActive(true);
    }


    public void StartMissionClicked()
    {
        Debug.Log("StartMissionClicked()");
        activePlayer = -1;
        activeCharacter = -1;
        activeItem = -1;
        isDeleteActive = false;
        DeactivateAllPanels();
        startMissionPanel.SetActive(true);
    }


    public void StartMissionConfirmed()
    {
        Debug.Log("StartMissionConfirmed()");
    }


    public void SoundOptionsClicked()
    {
        Debug.Log("SoundOptionsClicked()");
        activePlayer = -1;
        activeCharacter = -1;
        activeItem = -1;
        isDeleteActive = false;
        DeactivateAllPanels();
        soundOptionsPanel.SetActive(true);
    }


    public void ExitGameClicked()
    {
        Debug.Log("ExitGameClicked()");
        activePlayer = -1;
        activeCharacter = -1;
        activeItem = -1;
        isDeleteActive = false;
        DeactivateAllPanels();
        exitGamePanel.SetActive(true);
    }


    public void ExitGameConfirmed()
    {
        Application.Quit();
    }


    public void CreateNewCharacterClicked()
    {
        Debug.Log("CreateNewCharacterClicked()");
        createNewCharacterPanel.SetActive(true);
    }


    public void DeleteCharacterClicked()
    {
        isDeleteActive = !isDeleteActive;
        Debug.Log("DeleteCharacterClicked() -> isDeleteActive = " + isDeleteActive);
    }


    public void DeleteCharacterConfirmed()
    {
        Debug.Log("DeleteCharacterConfirmed(" + activeCharacter + ")");
        deleteCharacterPanel.SetActive(false);
        isDeleteActive = false;

        dataScript.savedCharacterListClass.list.RemoveAt(activeCharacter);
        dataScript.SaveCharacters();
        dataScript.LoadSavedCharacters();
        CheckStartMissionStatus();
        InitializePlayerBoxes();
        InitializeSavedCharacterBoxes();
    }


    public void InviteFriendClicked()
    {
        Debug.Log("InviteFriendClicked()");
        inviteFriendPanel.SetActive(true);
    }


    public void PlayerClicked(int playerIndex)
    {
        Debug.Log("PlayerClicked(" + playerIndex + ")");
        activePlayer = playerIndex;
        DeactivateAllPanels();
        selectedCharacterPanel.SetActive(true);
    }


    public void LevelUpClicked()
    {
        Debug.Log("LevelUpClicked(" + activePlayer + ")");
        activeItem = -1;
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);
        levelUpPanel.SetActive(true);
        // TODO set up credit check to determine which level up panel to show
    }


    public void LevelUpConfirmed()
    {
        Debug.Log("LevelUpConfirmed(" + activePlayer + ")");
        levelUpPanel.SetActive(false);
    }


    public void RestoreHPClicked()
    {
        Debug.Log("RestoreHPClicked(" + activePlayer + ")");
        activeItem = -1;
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);
        restoreHPPanel.SetActive(true);
    }


    public void RemovePlayerClicked()
    {
        Debug.Log("RemovePlayerClicked(" + activePlayer + ")");
        activeItem = -1;
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);
        removePlayerPanel.SetActive(true);
    }


    public void RemovePlayerConfirmed()
    {
        Debug.Log("RemovePlayerConfirmed(" + activePlayer + ")");
        activePlayer = -1;
        removePlayerPanel.SetActive(false);
        selectedCharacterPanel.SetActive(false);
    }


    public void SavedCharacterClicked(int characterIndex)
    {
        Debug.Log("SavedCharacterClicked(" + characterIndex + ")");
        activeCharacter = characterIndex;
        if(isDeleteActive)
        {
            deleteCharacterPanel.SetActive(true);
        }
        else
        {
            SavedCharacterSelectConfirmed();
        }
    }


    private void SavedCharacterSelectConfirmed()
    {
        Debug.Log("SavedCharacterSelectConfirmed(" + activeCharacter + ")");
        addPlayerPanel.SetActive(false);
        CheckStartMissionStatus();
    }


    public void ItemClicked(int itemIndex)
    {
        activeItem = itemIndex;
        Debug.Log("ItemClicked(" + itemIndex + ")");
        itemOptionsPanel.SetActive(true);
    }


    public void SellItemClicked()
    {
        Debug.Log("SellItemClicked(" + activeItem + ")");
        sellItemPanel.SetActive(true);
    }


    public void SellItemConfirmed()
    {
        Debug.Log("SellItemConfirmed(" + activeItem + ")");
        activeItem = -1;
        sellItemPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
    }


    public void IdentifyItemClicked()
    {
        Debug.Log("IdentifyItemClicked(" + activeItem + ")");
        identifyItemPanel.SetActive(true);
    }


    public void IdentifyItemConfirmed()
    {
        Debug.Log("IdentifyItemConfirmed(" + activeItem + ")");
        activeItem = -1;
        identifyItemPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
    }


}
