using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl_Main : MonoBehaviour
{
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

	// Use this for initialization
	void Start ()
    {
		
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
    }


    public void DeleteCharacterConfirmed()
    {
        Debug.Log("DeleteCharacterConfirmed(" + activeCharacter + ")");
        deleteCharacterPanel.SetActive(false);
        isDeleteActive = false;
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
