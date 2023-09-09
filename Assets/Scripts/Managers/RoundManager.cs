using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public class RoundPlayer
//{
//    public string name;
//    public int score = 0;
// //   public int tablePosition;
//    public int bidPlaced;
//    public int bidWon;
//    public bool isBot = false;
//    public bool isOwn = false;
//    public int photonIndex = 0;
//    public string id;
////    public bool isMaster;
//    public string imageURL;

//    public RoundPlayer(string playerName, string id, string image, bool isOwn, bool isBot, int bidPlaced, int bidWon)
//    {
//        this.name = playerName;
//        this.isBot = isBot;
//        this.isOwn = isOwn;
//        this.bidPlaced = bidPlaced;
//        this.bidWon = bidWon;
//        this.id = id;
//        this.imageURL = image;
//    }
//}

public class Round
{
    public List<Player> players = new List<Player>();
    //   public List<RoundPlayer> players = new List<RoundPlayer>();

    public Round(List<Player> roundPlayers)
    {
        //foreach(Player player in roundPlayers)
        //{
        //    RoundPlayer p = new RoundPlayer(player.name,player.id,player.imageURL,player.isOwn,player.isBot, player.bidPlaced,player.bidWon);
        //    players.Add(p);
        //}

        foreach (Player player in roundPlayers)
        {
            Player p = new Player(player.name, player.id, player.imageURL, player.isOwn, player.isMaster,player.isBot,player.tablePosition);
            p.bidPlaced = player.bidPlaced;
            p.bidWon = player.bidWon;
            players.Add(p);
        }
    }
}

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    public List<Round> rounds;
    public int currentRound = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        rounds = new List<Round>();
    }

    public void AddCurrentRoundProgress()
    {
        Round round = new Round(PlayerManager.instance.player);

    rounds.Add(round);
    }

    public void ClearAllRounds()
    {
        rounds.Clear();
    }
}
