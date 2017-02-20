using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();

        // Add E Move cards
        cards.Add(new Card(CardType.Move, 0));
        cards.Add(new Card(CardType.Move, 0));

        // Add Special Def cards
        cards.Add(new Card(CardType.Defense, 1));
        cards.Add(new Card(CardType.Defense, 2));

        // Add Special Atk cards
        cards.Add(new Card(CardType.Attack, 1));
        cards.Add(new Card(CardType.Attack, 2));

        // Add all other cards
        AddCards(CardType.Move, 6, 1, 3);
        AddCards(CardType.Trap, 5, 0, 3);
        AddCards(CardType.Defense, 4, 3, 9);
        AddCards(CardType.Attack, 4, 3, 9);
    }

    private void AddCards(CardType type, int numCopies, int minAmount, int maxAmount)
    {
        for (int amount = minAmount; amount < (maxAmount+1); amount++)
        {
            for (int ii = 0; ii < numCopies; ii++)
            {
                cards.Add(new Card(type, amount));
            }
        }
    }
}
