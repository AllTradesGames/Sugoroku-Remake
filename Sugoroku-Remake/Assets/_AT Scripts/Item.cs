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

}
