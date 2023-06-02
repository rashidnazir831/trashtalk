using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTrick
{


    public Card GetBestCard(List<Card> cardsInHand)
    {
        //Card.Suit leadingSuit = Card.Suit.Spades;
        //bool isFirstTurn = true;
        //Card bestCard = null;

        //if (TrickManager.cards.Count > 0)
        //{
        //    leadingSuit = TrickManager.cards[0].suit;
        //    isFirstTurn = false;
        //}

        //bool hasNormalCards = HasNormalCards(cardsInHand);
        //bool hasLeadingSuit = HasLeadingSuit(cardsInHand, leadingSuit);

        //Debug.Log("Printing bot cards list: " + isFirstTurn);

        //foreach (Card card in cardsInHand)
        //{
        //    Debug.Log("bot has: " + card.name);
        //}

        //foreach (Card card in cardsInHand)
        //{

        //        if (isFirstTurn)
        //        {
        //            if (hasNormalCards && (card.suit != Card.Suit.Spades && (bestCard == null || card.data.rank > bestCard.data.rank)))
        //            {
        //                bestCard = card;
        //            }
        //        }
        //        else
        //        {
        //            if (hasLeadingSuit && (bestCard == null || card.data.rank > bestCard.data.rank))
        //            {
        //                bestCard = card;
        //            }
        //        else
        //            {
        //            if (hasNormalCards && (bestCard == null || card.data.rank > bestCard.data.rank))
        //            {
        //                bestCard = card;
        //            }
        //            else if(card.suit == Card.Suit.Spades && (bestCard == null || card.data.rank > bestCard.data.rank))
        //            {
        //                bestCard = card;
        //            }
        //        }
        //        }
            
        //}
        return cardsInHand[0];
    }

    bool HasNormalCards(List<Card> cardsInHand)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.suit != Card.Suit.Spades)
            {
                return true;
            }
        }
        return false;
    }

    bool HasLeadingSuit(List<Card> cardsInHand,Card.Suit leadingSuit)
    {
        foreach (Card card in cardsInHand)
        {
            if (card.suit == leadingSuit)
            {
                return true;
            }
        }
        return false;
    }

}
