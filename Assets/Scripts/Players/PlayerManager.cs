using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
 //   public Transform[] playerDeckPosition;
    public List<Player> players;
    private int currentPlayerIndex;

    public static PlayerManager instance;

    public List<Player> player   // property
    {
        get { return players; }   // get method
        set { players = value; }  // set method
    }

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

    public List<Player> SortMultiplayerPositions()
    {
        List<Player> pl = new List<Player>();
        pl = this.players;


        int myIdIndex = pl.FindIndex(x => x.id == PlayerProfile.Player_UserID);
        print("SortMultiplayerPositions: and count here is " + pl.Count);
        if (myIdIndex == -1)
        {
            Debug.Log("ID not found in the list.");
            return pl;
        }

        //for(int j=0;j< pl.Count; j++)
        //{
        //    print("Before RRR: " + j  + " :  " + pl[j].id);
        //}

        List<Player> sortedList = new List<Player>();

        // Add elements after your ID
        for (int i = myIdIndex; i < pl.Count; i++)
        {
            //PlayerManager.instance.players[i].tablePosition = 0;
            pl[i].tablePosition = i;
            sortedList.Add(pl[i]);
        }

        // Add elements before your ID
        for (int i = 0; i < myIdIndex; i++)
        {
            pl[i].tablePosition = i;
            sortedList.Add(pl[i]);
        }
        print("sorted list count: " + sortedList.Count);

        //for (int j = 0; j < sortedList.Count; j++)
        //{
        //    print("After RRR: " + j + " :  " + sortedList[j].id);
        //}


        return sortedList;
    }
}
