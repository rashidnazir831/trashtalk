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
            }
            else if (card.suit == Card.Suit.Spades && bestCard.suit != Card.Suit.Spades)
            {
                bestCard = card; // Spades card is the best
            }
            else if (card.suit == bestCard.suit && card.data.score > bestCard.data.score)
            {
                bestCard = card; // Higher rank of leading suit is the best
            }
            else if (card.data.score > bestCard.data.score && bestCard.suit != Card.Suit.Spades)
            {
                bestCard = card; // Higher rank of leading suit is the best
            }

        }
        return bestCard;
    }

    public static Player GetTrickWinner()
    {
        Player roundWinner = GetBestCard().cardOwner;

        return roundWinner;
    }

    public static void GiveCardsToWinner(Player winner)
    {

        Transform transform = TableController.instance.GetPlayerWonCardTransform(winner.tablePosition);

        foreach(Card card in cards)
        {
            card.MoveCard(transform,1,true,true);
        }

     //   winner.bidWon++;

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateBidCount", winner.tablePosition, ++winner.bidWon, winner.bidPlaced);

    }

    public static void ResetTrick()
    {
        cards.Clear();
    }
}
