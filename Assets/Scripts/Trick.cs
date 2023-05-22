using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trick
{
    public Player player;
    public List<Card> cards;

    public Trick(Player player)
    {
        this.player = player;
        this.cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }
}
