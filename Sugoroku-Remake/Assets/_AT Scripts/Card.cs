using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Move, 
    Trap,
    Defense,
    Attack
}

public class Card
{
    public CardType type;
    public int amount;

    public Card(CardType inType, int inAmount)
    {
        type = inType;
        amount = inAmount;
    }
}
