using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public GameObject dealButton;
    public HandCardsUI cardHand;
  //  private Trick currentTrick;

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
     //   currentTrick = new Trick();
    }

    private void Start()
    {
        //PlayerManager.instance.AddPlayer("Player 1", true, true, 1);
        PlayerManager.instance.AddPlayer("Player 1",false,true,1);
        PlayerManager.instance.AddPlayer("Player 2", true,false,2);
        PlayerManager.instance.AddPlayer("Player 3", true, false, 3);
        PlayerManager.instance.AddPlayer("Player 4", true, false, 4);

        cardDeck = GetComponentInChildren<CardDeck>();
        bidManager = GetComponent<BidManager>();

        this.totalPlayers = PlayerManager.instance.players.Count;
        this.currentPlayerIndex = Random.Range(0, totalPlayers);
        this.totalPlayerPlayed = 0;

        SoundManager.Instance.PlayBackgroundMusic(Sound.Music);

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
       PlayerManager.instance.SetPlayerTurn(currentPlayerIndex);

        if (currentPlayer.isBot)
        {
            StartCoroutine(BotPlay(currentPlayer));
        }
        else
        {
            cardHand.ActiveMainPlayerCards();
            print("your turn");
        }
    }

    IEnumerator BotPlay(Player botPlayer)
    {
        yield return new WaitForSeconds(1f);
        List<Card> hand = botPlayer.hand;

        Card playedCard = botPlayer.PlayCard(0);
        playedCard.gameObject.SetActive(true);

        playedCard.SwitchSide(true);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(botPlayer.tablePosition),2.5f,true, ()=> {

            TrickManager.HighlightLowCards();
        });
        TrickManager.AddCard(playedCard);

        botPlayer.hand.Remove(playedCard);

    //    yield return new WaitForSeconds(1f);

        DecideNext();
    }

    public void PlaceCardOnTable(Player player, Card playedCard)
    {
        cardHand.ActiveMainPlayerCards(false);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(player.tablePosition), 2.5f,true,()=> {
            TrickManager.HighlightLowCards();
        });
        player.hand.Remove(playedCard);
        TrickManager.AddCard(playedCard);
        DecideNext();
    }

    void DecideNext()
    {
        this.totalPlayerPlayed++;

        if (this.totalPlayerPlayed == this.totalPlayers)
        {
            Player player = TrickManager.GetTrickWinner();

            print("round complete, winner is: " + player.name);

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
