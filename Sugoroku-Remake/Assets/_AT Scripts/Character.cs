using System;
using System.Collections.Generic;

[Serializable]
public class Character
{
    public string name;
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
    public int[] itemIdexes;

    public Character()
    {
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
        itemIdexes = new int[6];
    }

    public Character(int mockDataIndex)
    {
        switch(mockDataIndex)
        {
            case 0:
                name = "Mock_0";
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
                itemIdexes = new int[6];
                break;
            case 1:
                name = "Mock_1";
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
                credits = 0;
                itemIdexes = new int[6] {3, 0, 0, 0, 0, 0};
                break;
            case 2:
                name = "Mock_2";
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
                attackBonus = 2;
                defenseBonus = 6;
                credits = 0;
                itemIdexes = new int[6] { 1, 2, 2, 3, 0, 0 };
                break;
        }
        
    }

}
