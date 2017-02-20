using System;

[Serializable]
public class Character
{
    public int savedCharacterID;
    public string name;
    public string pathToModel;
    public int maxHP;
    public int tempMaxHP;
    public int currentHP;
    public int level;
    public int pointsAvailable;
    public int hpPoints;
    public int movementPoints;
    public int attackPoints;
    public int defensePoints;
    public int movementBonus;
    public int attackBonus;
    public int defenseBonus;
    public int credits;
    public int[] itemIndices;
    public bool[] identifiedItems;
    public bool inParty;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public Character()
    {
        savedCharacterID = -1;
        pathToModel = "Models/Characters/model_0";
        maxHP = 10;
        tempMaxHP = 10;
        currentHP = 10;
        level = 1;
        pointsAvailable = 11;
        hpPoints = 1;
        movementPoints = 1;
        attackPoints = 1;
        defensePoints = 1;
        movementBonus = 0;
        attackBonus = 1;
        defenseBonus = 0;
        credits = 0;
        itemIndices = new int[6];
        identifiedItems = new bool[DataControl.NUM_ITEM_SECTIONS * DataControl.ITEMS_PER_SECTION];
        inParty = true; 
    }

    public Character(int mockDataIndex)
    {
        switch(mockDataIndex)
        {
            case 0:
                savedCharacterID = 0;
                name = "Mock_0";
                pathToModel = "Models/Characters/model_0";
                maxHP = 19;
                tempMaxHP = 19;
                currentHP = 19;
                level = 1;
                pointsAvailable = 0;
                hpPoints = 4;
                movementPoints = 3;
                attackPoints = 6;
                defensePoints = 2;
                movementBonus = 1;
                attackBonus = 6;
                defenseBonus = 1;
                credits = 0;
                itemIndices = new int[6];
                identifiedItems = new bool[DataControl.NUM_ITEM_SECTIONS * DataControl.ITEMS_PER_SECTION];
                inParty = true;
                break;
            case 1:
                savedCharacterID = 1;
                name = "Mock_1";
                pathToModel = "Models/Characters/model_1";
                maxHP = 10;
                tempMaxHP = 10;
                currentHP = 10;
                level = 1;
                pointsAvailable = 0;
                hpPoints = 1;
                movementPoints = 12;
                attackPoints = 1;
                defensePoints = 1;
                movementBonus = 4;
                attackBonus = 1;
                defenseBonus = 0;
                credits = 236;
                itemIndices = new int[6] {3, 0, 0, 0, 0, 0};
                identifiedItems = new bool[DataControl.NUM_ITEM_SECTIONS * DataControl.ITEMS_PER_SECTION];
                inParty = false;
                break;
            case 2:
                savedCharacterID = 2;
                name = "Mock_2";
                pathToModel = "Models/Characters/model_2";
                maxHP = 13;
                tempMaxHP = 10;
                currentHP = 8;
                level = 1;
                pointsAvailable = 0;
                hpPoints = 2;
                movementPoints = 3;
                attackPoints = 1;
                defensePoints = 9;
                movementBonus = 1;
                attackBonus = 1;
                defenseBonus = 6;
                credits = 2734;
                itemIndices = new int[6] { 1, 2, 2, 3, 0, 0 };
                identifiedItems = new bool[DataControl.NUM_ITEM_SECTIONS * DataControl.ITEMS_PER_SECTION];
                identifiedItems[2] = true;
                inParty = true;
                break;
        }
        
    }

}
