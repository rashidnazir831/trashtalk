using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class TrickManager
{
    static List<Card> cards;
    static Card leadingCard;
   // static Card.Suit leadingSuit;

    static TrickManager()
    {
        cards = new List<Card>();
    }

    public static void AddCard(Card card)
    {
        if (cards.Count == 0)
        {
            leadingCard = card;
      //      leadingSuit = card.suit;
        }

        cards.Add(card);
    }

    public static void HighlightLowCards()
    {
        if (cards.Count == 0)
            return;

        Card bestCard = GetBestCard();

        foreach (Card card in cards)
        {
            card.HighlightCard(false);
        }

        bestCard.HighlightCard(true);

        Debug.Log("best card: " + bestCard.name);
    }

    static Card GetBestCard()
    {
        Card bestCard = null;

        foreach (Card card in cards)
        {

            if (bestCard == null)
            {
                bestCard = card;
                Debug.Log("if " + card.name); ;
            }
            else if (card.suit == Card.Suit.Spades && bestCard.suit != Card.Suit.Spades)
            {
             
                bestCard = card; // Spades card is the best
                Debug.Log("else if 1 " + card.name); ;

            }
            else if (card.suit == bestCard.suit && card.data.score > bestCard.data.score)
            {
                bestCard = card; // Higher rank of leading suit is the best
                Debug.Log("else if 2 " + card.name);

            }
            else if (card.data.score > bestCard.data.score && bestCard.suit != Card.Suit.Spades)
            {
                bestCard = card; // Higher rank of leading suit is the best
                Debug.Log("else if 3 " + card.name);

            }

        }
        return bestCard;
    }

    public static Player GetTrickWinner()
    {
        Player roundWinner = leadingCard.cardOwner;

        foreach (Card card in cards)
        {
            if (card.suit == leadingCard.suit && card.data.score >= leadingCard.data.score)
            {
                roundWinner = card.cardOwner;
                leadingCard = card;
            }
        }

        return roundWinner;
    }

    public static void ResetTrick()
    {
        cards.Clear();
    }
}
