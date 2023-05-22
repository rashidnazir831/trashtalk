using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IPlayer
{
    //public string Name { get; set; }
    //public List<Card> Hand { get; set; }
    //public int Score { get; set; }

    public string name;
    public List<Card> hand;
    public int score;
    public int tablePosition;
    public int bid;
    public bool isBot = false;
    public bool isOwn = false;


    public Player(string playerName, bool isBot,bool isOwn, int tablePos)
    {
        this.name = playerName;
        this.isBot = isBot;
        this.tablePosition = tablePos;
        this.isOwn = isOwn;
        this.bid = 0;
        hand = new List<Card>();
    }

    public void AddCardToHand(Card card)
    {
        hand.Add(card);
    }

    public Card PlayCard(int index)
    {
        if (index >= 0 && index < hand.Count)
        {
            Card card = hand[index];
            hand.RemoveAt(index);
            return card;
        }
        return null;
    }

    public void SetBid(int bid)
    {
        Debug.Log("set Bid: " + bid);
        this.bid = bid;
    }



    //public Player(string name)
    //{
    //    Name = name;
    //    Hand = new List<Card>();
    //    Score = 0;
    //}
}
