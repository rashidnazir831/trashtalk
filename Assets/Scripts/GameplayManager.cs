using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public GameObject dealButton;
    public HandCardsUI cardHand;

    int totalPlayers;
    int currentPlayerIndex;
    int totalPlayerPlayed;  //total Player played one round

    CardDeck cardDeck;
    BidManager bidManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private GameplayManager()
    {
        //totalPlayers = PlayerManager.instance.players.Count;
        //currentPlayerIndex = Random.Range(0, totalPlayers);

    }

    private void Start()
    {
        PlayerManager.instance.AddPlayer("Player 1",true,true,1);
        PlayerManager.instance.AddPlayer("Player 2", true,false,2);
        PlayerManager.instance.AddPlayer("Player 3", true, false, 3);
        PlayerManager.instance.AddPlayer("Player 4", true, false, 4);

        cardDeck = GetComponentInChildren<CardDeck>();
        bidManager = GetComponent<BidManager>();

        this.totalPlayers = PlayerManager.instance.players.Count;
        this.currentPlayerIndex = Random.Range(0, totalPlayers);
        this.totalPlayerPlayed = 0;
    }

    public void Deal()
    {
        dealButton.SetActive(false);
        cardDeck.DistributeCards();
    }

    public void StartBid()
    {
        cardHand.StartCoroutine(cardHand.ShowPlayerCards());
        bidManager.StartBid(this.totalPlayers, this.currentPlayerIndex);
    }

    public void StartGame()
    {
        StartTricks();
    }

    public void StartTricks()
    {
        PlayTurn();
    }

    public void PlayTurn()
    {
       Player currentPlayer = PlayerManager.instance.players[currentPlayerIndex];

       StartCoroutine(BotPlay(currentPlayer));
    }

    IEnumerator BotPlay(Player botPlayer)
    {
        List<Card> hand = botPlayer.hand;
        Card playedCard = botPlayer.PlayCard(0);


        playedCard.SwitchSide(true);


        playedCard.SetInitialParent();
        playedCard.MoveCard(CardsPositions.instance.GetPlayerShowCardTransform(botPlayer.tablePosition));
        yield return new WaitForSeconds(1f);

        DecideNext();

        //if (currentPlayerIndex < PlayerManager.instance.players.Count-1)
        //{
        //    this.totalPlayerPlayed++;
        //    PlayTurn();
        //}
    }

    void DecideNext()
    {
        this.totalPlayerPlayed++;

        if (this.totalPlayerPlayed == this.totalPlayers)
        {
            print("round over");
            return;
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % 4;

        PlayTurn();

    }



    private bool IsGameOver()
    {
        return false;
    }
}
