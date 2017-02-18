using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl_Main : MonoBehaviour
{
    public int identifyFeePerLevel = 50;

    public DataControl dataScript;

    public string pathToItemImages = "Images/Items/";

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
    public GameObject pointPlacementPanel;
    public GameObject cannotLevelUpPanel;
    public GameObject restoreHPPanel;
    public GameObject removePlayerPanel;
    public GameObject deleteCharacterPanel;
    public GameObject itemOptionsPanel;
    public GameObject sellItemPanel;
    public GameObject identifyItemPanel;
    public GameObject cannotIdentifyPanel;

    private bool isDeleteActive = false;
    private int activePlayer = -1;
    private int activeCharacter = -1;
    private int activeItem = -1;
    private GameObject tempObject;
    private float tempFloat = 0f;
    private int tempInt = 0;
    private bool tempBool = false;

    private int tempHPPoints = 0;
    

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

            tempObject = tempObject.transform.FindChild("Item Panel/Images").gameObject;
            for (int ii=0; ii < DataControl.NUM_ITEMS_PER_CHARACTER; ii++)
            {
                if ((character.itemIndices.Length > ii) && (character.itemIndices[ii] != 0))
                {
                    tempObject.transform.GetChild(ii).GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToItemImages + dataScript.masterItemListClass.list[character.itemIndices[ii]].imageName);
                }
                else
                {
                    tempObject.transform.GetChild(ii).GetComponent<Image>().sprite = null;
                }
            }
            tempObject = tempObject.transform.parent.parent.gameObject;

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


    public void InitializePointsPlacementPanel()
    {
        /*tempInt = 0;
        tempInt += dataScript.playerList[activePlayer].movementPoints - dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].movementPoints;
        tempInt += dataScript.playerList[activePlayer].attackPoints - dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].attackPoints;
        tempInt += dataScript.playerList[activePlayer].defensePoints - dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].defensePoints;
        tempInt += dataScript.playerList[activePlayer].hpPoints - dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].hpPoints;
        dataScript.playerList[activePlayer].pointsAvailable += tempInt;*/

        pointPlacementPanel.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].pointsAvailable;
        pointPlacementPanel.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].pointsAvailable.ToString();

        tempObject = pointPlacementPanel.transform.FindChild("Movement Stat").gameObject;
        tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].movementPoints;
        tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].movementPoints.ToString();
        tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = "+" + dataScript.playerList[activePlayer].movementBonus;

        tempObject = pointPlacementPanel.transform.FindChild("Attack Stat").gameObject;
        tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].attackPoints;
        tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackPoints.ToString();
        tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackBonus.ToString();

        tempObject = pointPlacementPanel.transform.FindChild("Defense Stat").gameObject;
        tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].defensePoints;
        tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].defensePoints.ToString();
        tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].defenseBonus.ToString();

        tempObject = pointPlacementPanel.transform.FindChild("Health Stat").gameObject;
        tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].hpPoints;
        tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].hpPoints.ToString();
        tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].maxHP.ToString();

        tempObject = null;
    }


    public void InitializeSelectedPlayerItems()
    {
        tempObject = selectedCharacterPanel.transform.FindChild("Items Panel/Item Buttons").gameObject;
        for (int ii = 0; ii < DataControl.NUM_ITEMS_PER_CHARACTER; ii++)
        {
            if ((dataScript.playerList[activePlayer].itemIndices.Length > ii) && (dataScript.playerList[activePlayer].itemIndices[ii] != 0))
            {
                tempObject.transform.GetChild(ii).FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToItemImages + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].imageName);
                if (!dataScript.playerList[activePlayer].identifiedItems[dataScript.playerList[activePlayer].itemIndices[ii]])
                {
                    tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = "???";
                }
                else
                {
                    switch (dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].itemEffect)
                    {
                        case (int)ItemEffect.None:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\n";
                            break;
                        case (int)ItemEffect.Attack:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nAtk +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Defense:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nDef +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Movement:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nMv +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Evade:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nEv Trap +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount + "%";
                            break;
                        case (int)ItemEffect.Escape:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nEscape +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.CritAttack:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCrit Attack x2";
                            break;
                        case (int)ItemEffect.CritDefend:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCrit Defense x2";
                            break;
                        case (int)ItemEffect.CritEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCrit Empty Hand";
                            break;
                        case (int)ItemEffect.CritStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCrit Stun";
                            break;
                        case (int)ItemEffect.CritLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCrit Leg Dmg";
                            break;
                        case (int)ItemEffect.Voodoo:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nVoodoo";
                            break;
                        case (int)ItemEffect.MoveHeal:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nMove Heal +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.RestUp:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRest Heal +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.Crutch:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nLeg Dmg Mv +" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.NoPanic:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nNo Panic";
                            break;
                        case (int)ItemEffect.NoStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nNo Stun";
                            break;
                        case (int)ItemEffect.NoLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nNo Leg Dmg";
                            break;
                        case (int)ItemEffect.NoEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nNo Empty Hand";
                            break;
                        case (int)ItemEffect.RollCap5:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRoll Max is 4" + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].effectAmount;
                            break;
                        case (int)ItemEffect.CausePanic:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRandom Panic";
                            break;
                        case (int)ItemEffect.CauseEmpty:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRandom Empty Hand";
                            break;
                        case (int)ItemEffect.CauseStun:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRandom Stun";
                            break;
                        case (int)ItemEffect.CauseLegDmg:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRandom Leg Dmg";
                            break;
                        case (int)ItemEffect.NPC0NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nGON No Counter";
                            break;
                        case (int)ItemEffect.NPC1NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nCAL No Counter";
                            break;
                        case (int)ItemEffect.NPC2NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nBRO No Counter";
                            break;
                        case (int)ItemEffect.NPC3NoCounter:
                            tempObject.transform.GetChild(ii).FindChild("Text").GetComponent<Text>().text = dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[ii]].name + "\nRAD No Counter";
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


    public void AddPoint(string stat)
    {
        if (dataScript.playerList[activePlayer].pointsAvailable > 0)
        {
            dataScript.playerList[activePlayer].pointsAvailable--;
            pointPlacementPanel.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].pointsAvailable;
            pointPlacementPanel.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].pointsAvailable.ToString();

            tempObject = pointPlacementPanel.transform.FindChild(stat + " Stat").gameObject;
            switch(stat)
            {
                case "Movement":
                    dataScript.playerList[activePlayer].movementPoints++;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Movement);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].movementPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].movementPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = "+" + dataScript.playerList[activePlayer].movementBonus;
                    break;
                case "Attack":
                    dataScript.playerList[activePlayer].attackPoints++;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Attack);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].attackPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackBonus.ToString();
                    break;
                case "Defense":
                    dataScript.playerList[activePlayer].defensePoints++;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Defense);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].defensePoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].defensePoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].defenseBonus.ToString();
                    break;
                case "Health":
                    dataScript.playerList[activePlayer].hpPoints++;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Health);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].hpPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].hpPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].maxHP.ToString();
                    break;
            }
            tempObject = null;
        }
    }


    public void SubtractPoint(string stat)
    {
        tempBool = false;
        tempObject = pointPlacementPanel.transform.FindChild(stat + " Stat").gameObject;
        switch (stat)
        {
            case "Movement":
                if (dataScript.playerList[activePlayer].movementPoints > dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].movementPoints)
                {
                    dataScript.playerList[activePlayer].movementPoints--;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Movement);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].movementPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].movementPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = "+" + dataScript.playerList[activePlayer].movementBonus;
                    tempBool = true;
                }
                break;
            case "Attack":
                if (dataScript.playerList[activePlayer].attackPoints > dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].attackPoints)
                {
                    dataScript.playerList[activePlayer].attackPoints--;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Attack);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].attackPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].attackBonus.ToString();
                    tempBool = true;
                }
                break;
            case "Defense":
                if (dataScript.playerList[activePlayer].defensePoints > dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].defensePoints)
                {
                    dataScript.playerList[activePlayer].defensePoints--;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Defense);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].defensePoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].defensePoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].defenseBonus.ToString();
                    tempBool = true;
                }
                break;
            case "Health":
                if (dataScript.playerList[activePlayer].hpPoints > dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].hpPoints)
                {
                    dataScript.playerList[activePlayer].hpPoints--;
                    dataScript.UpdateStatBonus(activePlayer, Stat.Health);
                    tempObject.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].hpPoints;
                    tempObject.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].hpPoints.ToString();
                    tempObject.transform.FindChild("Bonus").GetComponent<Text>().text = dataScript.playerList[activePlayer].maxHP.ToString();
                    tempBool = true;
                }
                break;
        }

        if (tempBool)
        {
            dataScript.playerList[activePlayer].pointsAvailable++;
            pointPlacementPanel.transform.FindChild("Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].pointsAvailable;
            pointPlacementPanel.transform.FindChild("Points").GetComponent<Text>().text = dataScript.playerList[activePlayer].pointsAvailable.ToString();
        }

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
        pointPlacementPanel.SetActive(false);
        cannotLevelUpPanel.SetActive(false);
        restoreHPPanel.SetActive(false);
        removePlayerPanel.SetActive(false);
        deleteCharacterPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);
        cannotIdentifyPanel.SetActive(false);
    }


    public void AddPlayerClicked()
    {
        Debug.Log("AddPlayerClicked()");
        DeactivateAllPanels();
        addPlayerPanel.SetActive(true);
        InitializeSavedCharacterBoxes();
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

        // TODO Load character model here

        InitializeSelectedPlayerItems();

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
        
        tempInt = 0;
        tempInt = dataScript.playerList[activePlayer].level;
        tempInt = 500 * ((((tempInt - 1) * tempInt) / 2) + 2);

        if(dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].pointsAvailable > 0)
        {
            dataScript.LoadSavedCharacters();
            InitializePlayerBoxes();
            InitializePointsPlacementPanel();
            pointPlacementPanel.SetActive(true);
        }
        else if(dataScript.playerList[activePlayer].credits < tempInt)
        {
            cannotLevelUpPanel.transform.FindChild("Text").GetComponent<Text>().text = "You need " + tempInt + " Cr to Level Up!";
            cannotLevelUpPanel.SetActive(true);
        }
        else
        {
            levelUpPanel.transform.FindChild("Text").GetComponent<Text>().text = "Spend " + tempInt + " Cr to Level Up?";
            levelUpPanel.SetActive(true);
        }        
    }


    public void LevelUpConfirmed()
    {
        Debug.Log("LevelUpConfirmed(" + activePlayer + ")");
        levelUpPanel.SetActive(false);

        tempInt = 0;
        tempInt = dataScript.playerList[activePlayer].level;
        tempInt = 500 * ((((tempInt - 1) * tempInt) / 2) + 2);
        dataScript.playerList[activePlayer].credits -= tempInt;
        dataScript.playerList[activePlayer].level++;
        dataScript.playerList[activePlayer].pointsAvailable++;
        dataScript.UpdateStatBonus(activePlayer, Stat.Health);  // Adds 1 to player total health, because the level was increased since last update
        dataScript.playerList[activePlayer].tempMaxHP = dataScript.playerList[activePlayer].maxHP;
        dataScript.playerList[activePlayer].currentHP = dataScript.playerList[activePlayer].maxHP;

        dataScript.PostPlayerToSavedCharacter(activePlayer);
        InitializePlayerBoxes();
        InitializePointsPlacementPanel();
        pointPlacementPanel.SetActive(true);
    }


    public void PointPlacementConfirmed()
    {
        dataScript.playerList[activePlayer].tempMaxHP = dataScript.playerList[activePlayer].maxHP;
        dataScript.playerList[activePlayer].currentHP = dataScript.playerList[activePlayer].maxHP;

        dataScript.PostPlayerToSavedCharacter(activePlayer);
        InitializePlayerBoxes();
        pointPlacementPanel.SetActive(false);
    }


    public void RestoreHPClicked()
    {
        Debug.Log("RestoreHPClicked(" + activePlayer + ")");
        activeItem = -1;
        itemOptionsPanel.SetActive(false);
        sellItemPanel.SetActive(false);
        identifyItemPanel.SetActive(false);

        tempInt = 0;
        tempInt = dataScript.playerList[activePlayer].level;
        tempInt = 50 * ((((tempInt - 1) * tempInt) / 2) + 2);
        restoreHPPanel.transform.FindChild("Text").GetComponent<Text>().text = "It costs " + tempInt + " Cr to heal 1 HP. How much would you like to heal?";
        dataScript.playerList[activePlayer].currentHP = dataScript.playerList[activePlayer].tempMaxHP;
        restoreHPPanel.transform.FindChild("Health/Slider").GetComponent<Slider>().maxValue = dataScript.playerList[activePlayer].maxHP;
        restoreHPPanel.transform.FindChild("Health/Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].tempMaxHP;
        restoreHPPanel.transform.FindChild("Health/Cost").GetComponent<Text>().text = dataScript.playerList[activePlayer].credits + " Cr";
        restoreHPPanel.transform.FindChild("Health/HealthText").GetComponent<Text>().text = dataScript.playerList[activePlayer].tempMaxHP + "/" + dataScript.playerList[activePlayer].maxHP;
        tempHPPoints = 0;
        restoreHPPanel.SetActive(true);
    }


    public void AddHP()
    {
        if(((dataScript.playerList[activePlayer].tempMaxHP + tempHPPoints) < dataScript.playerList[activePlayer].maxHP) && ((dataScript.playerList[activePlayer].credits - (tempInt * (tempHPPoints + 1))) > 0))
        {
            tempHPPoints++;
            restoreHPPanel.transform.FindChild("Health/Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].tempMaxHP + tempHPPoints;
            restoreHPPanel.transform.FindChild("Health/Cost").GetComponent<Text>().text = (dataScript.playerList[activePlayer].credits - (tempInt * tempHPPoints)) + " Cr";
            restoreHPPanel.transform.FindChild("Health/HealthText").GetComponent<Text>().text = (dataScript.playerList[activePlayer].tempMaxHP + tempHPPoints) + "/" + dataScript.playerList[activePlayer].maxHP;
        }
    }


    public void SubtractHP()
    {
        if(tempHPPoints > 0)
        {
            tempHPPoints--;
            restoreHPPanel.transform.FindChild("Health/Slider").GetComponent<Slider>().value = dataScript.playerList[activePlayer].tempMaxHP + tempHPPoints;
            restoreHPPanel.transform.FindChild("Health/Cost").GetComponent<Text>().text = (dataScript.playerList[activePlayer].credits - (tempInt * tempHPPoints)) + " Cr";
            restoreHPPanel.transform.FindChild("Health/HealthText").GetComponent<Text>().text = (dataScript.playerList[activePlayer].tempMaxHP + tempHPPoints) + "/" + dataScript.playerList[activePlayer].maxHP;
        }
    }


    public void RestoreHPConfirmed()
    {
        dataScript.playerList[activePlayer].credits -= tempInt * tempHPPoints;
        dataScript.playerList[activePlayer].tempMaxHP += tempHPPoints;
        dataScript.playerList[activePlayer].currentHP = dataScript.playerList[activePlayer].tempMaxHP;

        dataScript.PostPlayerToSavedCharacter(activePlayer);
        InitializePlayerBoxes();
        InitializeSavedCharacterBoxes();
        restoreHPPanel.SetActive(false);
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
        dataScript.playerList[activePlayer].inParty = false;
        dataScript.savedCharacterListClass.list[dataScript.playerList[activePlayer].savedCharacterID].inParty = false;
        dataScript.SaveCharacters();
        dataScript.LoadSavedCharacters();
        CheckStartMissionStatus();
        InitializePlayerBoxes();
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
        dataScript.savedCharacterListClass.list[activeCharacter].inParty = true;
        dataScript.SaveCharacters();
        dataScript.LoadSavedCharacters();
        CheckStartMissionStatus();
        InitializePlayerBoxes();
        addPlayerPanel.SetActive(false);
    }


    public void ItemClicked(int itemIndex)
    {
        activeItem = itemIndex;
        Debug.Log("ItemClicked(" + itemIndex + ")");
        if(dataScript.playerList[activePlayer].identifiedItems[dataScript.playerList[activePlayer].itemIndices[itemIndex]])
        {
            itemOptionsPanel.transform.FindChild("Identify Item Button").gameObject.SetActive(false);
        }
        else
        {
            itemOptionsPanel.transform.FindChild("Identify Item Button").gameObject.SetActive(true);
        }
        itemOptionsPanel.SetActive(true);
    }


    public void SellItemClicked()
    {
        Debug.Log("SellItemClicked(" + activeItem + ")");

        sellItemPanel.transform.FindChild("Text").GetComponent<Text>().text = "Sell " + dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[activeItem]].name + " for " + (dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[activeItem]].basePrice + (dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[activeItem]].pricePerLevel * dataScript.playerList[activePlayer].level)) + " Cr?";

        sellItemPanel.SetActive(true);
    }


    public void SellItemConfirmed()
    {
        Debug.Log("SellItemConfirmed(" + activeItem + ")");

        dataScript.playerList[activePlayer].credits += dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[activeItem]].basePrice + (dataScript.masterItemListClass.list[dataScript.playerList[activePlayer].itemIndices[activeItem]].pricePerLevel * dataScript.playerList[activePlayer].level);
        dataScript.playerList[activePlayer].itemIndices[activeItem] = 0;
        dataScript.UpdateStatBonus(activePlayer);
        dataScript.PostPlayerToSavedCharacter(activePlayer);

        activeItem = -1;
        sellItemPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
        InitializePlayerBoxes();
        InitializeSelectedPlayerItems();
    }


    public void IdentifyItemClicked()
    {
        Debug.Log("IdentifyItemClicked(" + activeItem + ")");

        if (dataScript.playerList[activePlayer].credits < (identifyFeePerLevel * dataScript.playerList[activePlayer].level))
        {
            cannotIdentifyPanel.transform.FindChild("Text").GetComponent<Text>().text = "You need " + (identifyFeePerLevel * dataScript.playerList[activePlayer].level) + " Cr to Identify this Item!";
            cannotIdentifyPanel.SetActive(true);
        }
        else
        {
            identifyItemPanel.transform.FindChild("Text").GetComponent<Text>().text = "Identify this Item for " + (identifyFeePerLevel * dataScript.playerList[activePlayer].level) + " Cr?";
            identifyItemPanel.SetActive(true);
        }        
    }


    public void IdentifyItemConfirmed()
    {
        Debug.Log("IdentifyItemConfirmed(" + activeItem + ")");

        dataScript.playerList[activePlayer].credits -= identifyFeePerLevel * dataScript.playerList[activePlayer].level;
        dataScript.playerList[activePlayer].identifiedItems[dataScript.playerList[activePlayer].itemIndices[activeItem]] = true;
        dataScript.UpdateStatBonus(activePlayer);
        dataScript.PostPlayerToSavedCharacter(activePlayer);

        activeItem = -1;
        identifyItemPanel.SetActive(false);
        itemOptionsPanel.SetActive(false);
        InitializePlayerBoxes();
        InitializeSelectedPlayerItems();
    }






}
