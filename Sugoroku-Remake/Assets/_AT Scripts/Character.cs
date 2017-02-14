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
    public List<Item> itemList;

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
        itemList = new List<Item>();
    }

}
