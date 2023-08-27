using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public GameObject playButton;
    public HandCardsUI cardHand;
  //  private Trick currentTrick;

    int totalPlayers;
    int currentPlayerIndex;
    int totalPlayerPlayed;  //total Player played one round

    CardDeck cardDeck;
    BidManager bidManager;
    BotTrick botTrick;

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

    private void OnEnable()
    {
        //print("onenable");
        //ResetPanelData();
        PlayerManager.instance.ClearPlayers();

        if (!Global.isMultiplayer)
        {
            PlayerManager.instance.AddPlayer("Player 1",null, null, true,true, false, 0);
            PlayerManager.instance.AddPlayer("Bot 1", null, null, false, false, true, 1);
            PlayerManager.instance.AddPlayer("Bot 2", null, null, false, false, true, 2);
            PlayerManager.instance.AddPlayer("Bot 3", null, null, false, false, true, 3);
        }

        cardDeck = GetComponentInChildren<CardDeck>();
        bidManager = GetComponent<BidManager>();
        botTrick = new BotTrick();

      //  this.totalPlayers = PlayerManager.instance.players.Count;
      //  this.currentPlayerIndex = Random.Range(0, totalPlayers);
        //     this.totalPlayerPlayed = 0;
        SoundManager.Instance.PlayBackgroundMusic(Sound.Music);
        SetPlayButton();
        StartNewGame();
    }

    private void Start()
    {
        SetPlayersData();
    }

    public void SetPlayersData()
    {
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }



    void ResetContainers()
    {
        cardDeck.ClearDeck();
        cardHand.ClearHandCards();
        TableController.instance.ClearAllContainers();
    }

    public void SetPlayButton()
    {
        playButton.SetActive(!Global.isMultiplayer);
    }


    public void StartNewGame()
    {
        TableController.instance.ShowSideTable(false);
      //  playButton.SetActive(true);
        ResetGame();
    }


    public void ResetGame()
    {
        ResetContainers();
        //playButton.SetActive(true);
        this.totalPlayers = PlayerManager.instance.players.Count;
        this.currentPlayerIndex = Random.Range(0, totalPlayers);

        PlayerManager.instance.ClearPlayersData();

        TrickManager.ResetTrick();
      //  TableController.instance.ClearCards();

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ResetUI");
        
    }

    public void OnPlayGameButton()
    {
        playButton.SetActive(false);
        UIEvents.UpdateData(Panel.GameplayPanel, OnCardsCoverScreen, "ShowCardIntro");
    }

    void OnCardsCoverScreen(object[] parameters = null)
    {
        ResetGame();
        cardDeck.CreateInitialDeck();
        TableController.instance.ShowSideTable();
        Invoke("Deal", 1.5f);

    }

    public void Deal()
    {
     //   playButton.SetActive(false);
        cardDeck.DistributeCards();
    }

    public void StartBid()
    {
        cardHand.SortHandCards();
        cardHand.StartCoroutine(cardHand.ShowPlayerCards());
        bidManager.StartBid(this.totalPlayers, this.currentPlayerIndex);
    }

    public void StartGame()
    {
        StartTricks();
    }

    public void StartTricks()
    {
        this.totalPlayerPlayed = 0;
//        currentPlayerIndex = (currentPlayerIndex + 1) % 4;

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
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", true);
            print("your turn");
        }
    }

    IEnumerator BotPlay(Player botPlayer)
    {
        yield return new WaitForSeconds(1f);
        List<Card> hand = botPlayer.hand;

        //Card playedCard = botPlayer.PlayCard(0);
        Card playedCard = botTrick.GetBestCard(botPlayer.hand);
        print("bot has choosen: " + playedCard.name);
        playedCard.gameObject.SetActive(true);

        playedCard.SwitchSide(true);
        print("currentPlayerIndex" + currentPlayerIndex);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(botPlayer.tablePosition),2.5f,true,false, ()=> {
            TrickManager.AddCard(playedCard);

            botPlayer.hand.Remove(playedCard);
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", currentPlayerIndex, botPlayer.hand.Count);

            TrickManager.HighlightLowCards();

            DecideNext();

        });


    //    yield return new WaitForSeconds(1f);

    }

    public void PlaceCardOnTable(Player player, Card playedCard)
    {
        //cardHand.UpdateCardArrangement();

        cardHand.ActiveMainPlayerCards(false);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(player.tablePosition), 2.5f,true,false,()=> {
            TrickManager.HighlightLowCards();
            cardHand.UpdateCardArrangement();
        });
        player.hand.Remove(playedCard);
        cardHand.OnUseHandCard(playedCard);
        TrickManager.AddCard(playedCard);
        DecideNext();
    }

    void DecideNext()
    {
        this.totalPlayerPlayed++;

        if (this.totalPlayerPlayed == this.totalPlayers)
        {
            Invoke("OnWinTrick", 1);
            return;
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % 4;

        PlayTurn();
    }

    Player trickWinner;

    void OnWinTrick()
    {
        Player player = TrickManager.GetTrickWinner();
        this.trickWinner = player;
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "WinnerAnimation", player.tablePosition);
        TrickManager.GiveCardsToWinner(player);
        CompleteTrick();
    }


    void CompleteTrick()
    {
        if (IsRoundOver())
        {
            Invoke("OnRoundOver", 1);
        }
        else
        {
            Invoke("ResetTrick", 1);
        }

    }

    void ResetTrick()
    {
        TrickManager.ResetTrick();

        currentPlayerIndex = this.trickWinner.tablePosition;

        StartTricks();
    }

    void OnRoundOver()
    {
        print("round over");
        UIEvents.ShowPanel(Panel.EndGamePanel);
        //    UIEvents.ShowPanel(Panel.GameOverPanel);

    }

    void ResetRound()
    {

    }

    private bool IsRoundOver()
    {
        foreach (Player player in PlayerManager.instance.players)
        {
            if (player.hand.Count > 0)
            {
                return false;
            }
        }
        return true;

    }
}
