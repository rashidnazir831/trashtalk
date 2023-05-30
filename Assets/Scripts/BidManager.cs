using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BidManager : MonoBehaviour
{
    int currentPlayerIndex;
    int totalBidsPlaced;
    int totalPlayers;
    Player currentPlayer;


    public void StartBid(int totalPlayers, int currentPlayerIndex)
    {
        totalBidsPlaced = 0;
        this.totalPlayers = totalPlayers;
        this.currentPlayerIndex = currentPlayerIndex;

        SelectBid();
    }

    void SelectBid()
    {

        //string panel = "Panel1";
      //  System.Action<object[]> callback = PlacePlayerBid;

        //UpdateData(panel, callback, param1, param2, param3);

        currentPlayer = PlayerManager.instance.players[currentPlayerIndex];

        if (currentPlayerIndex == 0)//it is temproray later it will be decided by ID or other property
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, PlacePlayerBid,  "ShowBidUI", currentPlayerIndex,-1);
            
        }
        else
        {
            StartCoroutine(BotPlaceBid());
        }
    }

    public void PlacePlayerBid(object[] parameters)
    {
        this.currentPlayer.SetBid((int)parameters[0]);
        DecideNext();
    }


    IEnumerator BotPlaceBid()
    {
        Player botPlayer = this.currentPlayer;
        List<Card> hand = botPlayer.hand;
       

        int spadeCount = 0;
        int handStrength = 0;

        foreach (Card card in hand)
        {
            if (card.data.suit == Card.Suit.Spades.ToString())
            {
                spadeCount++;
                handStrength += card.data.rank;
            }
        }

        int bid = 0;

        if (handStrength >= 13 && spadeCount >= 1)
        {
            bid = spadeCount + 1;
        }
        else if (handStrength >= 10 && spadeCount >= 2)
        {
            bid = spadeCount;
        }
        else if (handStrength >= 7 && spadeCount >= 3)
        {
            bid = spadeCount - 1;
        }

        botPlayer.SetBid(bid);
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowBidUI", currentPlayerIndex,bid);
        UpdateBidCount(botPlayer);
        yield return new WaitForSeconds (1);
        DecideNext();
    }

    public void UpdateBidCount(Player player)
    {
        int playerPos = player.tablePosition;
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateBidCount", playerPos, player.bidWon,player.bidPlaced);
    }

    void DecideNext()
    {
        totalBidsPlaced++;

        if(totalBidsPlaced == totalPlayers)
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "HideAllBidsUIs");
            GameplayManager.instance.StartGame();
            return;
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % 4;

        SelectBid();

    }
}
