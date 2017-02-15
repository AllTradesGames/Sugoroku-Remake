using System;
using System.Collections.Generic;

[Serializable]
public class ItemListClass
{
    public List<Item> list;
    public bool[] loadedSections;

    public ItemListClass()
    {
        // TODO Place entire item list here?
        list = new List<Item>();
        list.Add(null);
        list.Add(new Item(
            "Item1",
            "Item1",
            50,
            50,
            (int)ItemEffect.Attack,
            1,
            false
        ));
        list.Add(new Item(
            "Item2",
            "Item2",
            100,
            50,
            (int)ItemEffect.Defense,
            1,
            false
        ));
        list.Add(new Item(
            "Item3",
            "Item3",
            50,
            25,
            (int)ItemEffect.None,
            0,
            false
        ));

        loadedSections = new bool[DataControl.NUM_ITEM_SECTIONS] { false, false, false };
    }

}
