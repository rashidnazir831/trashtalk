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
    public int bidPlaced;
    public int bidWon;
    public bool isBot = false;
    public bool isOwn = false;
    public List<Card> winningCards;


    public Player(string playerName, bool isBot,bool isOwn, int tablePos)
    {
        this.name = playerName;
        this.isBot = isBot;
        this.tablePosition = tablePos;
        this.isOwn = isOwn;
        this.bidPlaced = 0;
        this.bidWon = 0;
        hand = new List<Card>();
    }

    public void AddWinningCard(Card card)
    {
        winningCards.Add(card);
    }

    public void AddCardToHand(Card card)
    {
        hand.Add(card);
    }

    public void ResetCards()    //Reset cards in each rounds
    {
        winningCards.Clear();
        hand.Clear();
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
        this.bidPlaced = bid;
    }



    //public Player(string name)
    //{
    //    Name = name;
    //    Hand = new List<Card>();
    //    Score = 0;
    //}
}
