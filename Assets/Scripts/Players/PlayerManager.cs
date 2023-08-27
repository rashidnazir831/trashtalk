using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
 //   public Transform[] playerDeckPosition;
    public List<Player> players;
    private int currentPlayerIndex;

    public static PlayerManager instance;

    private void Awake()
    {
        print("players");
        if(instance == null)
        {
            instance = this;
        }

        players = new List<Player>();
        currentPlayerIndex = 0;
    }

    public void AddPlayer(string name, string id,string image, bool isOwn, bool isMaster, bool isBot, int tablePos)
    {
        Player player = new Player(name, id,image, isOwn, isMaster, isBot, tablePos);
        players.Add(player);
    }

    public Player GetPlayer(int index)
    {
       return players[index];
    }

    public void ClearPlayers()
    {
        players.Clear();
    }

    public void ClearPlayersData()
    {
        foreach (Player player in players)
        {
            player.ResetCards();
        }
    }

    //public void AddCardToHand(Card card)
    //{
    //    hand.Add(card);
    //}

    //public Transform GetPlayerTableTransform(int playerNumber)
    //{
    //    return playerDeckPosition[playerNumber];
    //}

    //public void GetTablePosition()
    //{

    //}

    //public PlayerManager(List<Player> playerList)
    //{
    //    players = playerList;
    //    currentPlayerIndex = 0;
    //}

    public void SetPlayerTurn(int playerIndex)
    {
        //logic if player has turn
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void GoToNextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }
}
