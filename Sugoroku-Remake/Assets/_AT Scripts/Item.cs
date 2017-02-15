using System;

[Serializable]
public class Item
{
    public string name;
    public string imageName;
    public int basePrice;
    public int pricePerLevel;
    public int itemEffect;
    public int effectAmount;
    public bool isIdentified;

    public Item()
    {
        
    }

    public Item(string inName, string inImageName, int inBasePrice, int inPricePerLevel, int inItemEffect, int inEffectAmount, bool inIsIdentified)
    {
        name = inName;
        imageName = inImageName;
        basePrice = inBasePrice;
        pricePerLevel = inPricePerLevel;
        itemEffect = inItemEffect;
        effectAmount = inEffectAmount;
        isIdentified = inIsIdentified;
    }

}
