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

        if (!Global.isMultiplayer)
        {
            PlayerManager.instance.ClearPlayers();
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
        SetPlayButton(!Global.isMultiplayer);
        StartNewGame();
    }

    private void Start()
    {
    //    if (!Global.isMultiplayer)
            SetPlayersData();
    }

    public void SetPlayersData()
    {
        print("setting player data");
        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }



    void ResetContainers()
    {
        cardDeck.ClearDeck();
        cardHand.ClearHandCards();
        TableController.instance.ClearAllContainers();
    }

    public void SetPlayButton(bool isActive)
    {
        playButton.SetActive(isActive);
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


        if (Global.isMultiplayer)
            this.currentPlayerIndex = cardDeck.GetMasterIndex();
        else
            this.currentPlayerIndex = Random.Range(0, totalPlayers);


        //    PlayerManager.instance.ClearPlayersData();

        TrickManager.ResetTrick();
      //  TableController.instance.ClearCards();

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ResetUI");
        
    }

    public void OnPlayGameButton()
    {
        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient) {
            Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = false;
            Photon.Pun.PhotonNetwork.CurrentRoom.IsVisible = false;

            string shuffledCards = cardDeck.GetShuffleCardsString();

            //    cardDeck.ShowHideCardContainer(false);

            PhotonRPCManager.Instance.SpawnPlayers(shuffledCards);
        }
        else if(!Global.isMultiplayer)
        {
            AnimateCardsScreen();
        }
        //if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
        //{
        //    cardDeck.CreateInitialDeck();
        //    cardDeck.ShowHideCardContainer(false);
        //}


        //  playButton.SetActive(false);
        //  UIEvents.UpdateData(Panel.GameplayPanel, OnCardsCoverScreen, "ShowCardIntro");
    }


    string multiplayerCards = null;
    public void AnimateCardsScreen(string shuffledCards=null)
    {
        this.multiplayerCards = shuffledCards;
        playButton.SetActive(false);
        UIEvents.UpdateData(Panel.GameplayPanel, OnCardsCoverScreen, "ShowCardIntro");
    }

    void OnCardsCoverScreen(object[] parameters = null)
    {
        ResetGame();
        TableController.instance.ShowSideTable();

        if (Global.isMultiplayer)
        {
            //if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
            //{
            //    cardDeck.CreateInitialDeck();
            //    cardDeck.ShowHideCardContainer(false);
            //}
            List<CardData> c = cardDeck.CardsStringToList(this.multiplayerCards);
            cardDeck.CreateInitialDeck(c);
            AddRemainingPlayers();

            //    UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
            Invoke("Deal", 1.5f); //will open this

            //    if(Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
            //         Invoke("StartDealByMaster", 1.5f); //will open this


            return;
        }

        //creating this deck for practice
        cardDeck.CreateInitialDeck();
        Invoke("Deal", 1.5f); //will open this
    }

    //void StartDealByMaster(){

    //}

    //Add bot players if there are less than 4 players in photon room
    void AddRemainingPlayers()
    {

        for(int i = 0; i < 4; i++)
        {
            if(PlayerManager.instance.player[i].id == null)
            {
                PlayerManager.instance.players[i].name = $"Bot Player {i}";
                PlayerManager.instance.players[i].isOwn = false;
                PlayerManager.instance.players[i].isMaster = false;
                PlayerManager.instance.players[i].isBot = true;
                PlayerManager.instance.players[i].id = $"BP{i}";
                //   PlayerManager.instance.players[i].tablePosition = 0;
                PlayerManager.instance.players[i].photonIndex = i;
            }
        }

        PlayerManager.instance.player = PlayerManager.instance.SortMultiplayerPositions();

        //for(int r=0;r< PlayerManager.instance.players.Count; r++)
        //{
        //    print("on adding player: " + r +" : " + PlayerManager.instance.players[r].name);
        //}

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }

    public void ReplaceBotWithPlayer(string playerID)
    {
        for(int i=0;i< PlayerManager.instance.players.Count;i++)
        {
            Player p = PlayerManager.instance.players[i];
            if (p.id == playerID)
            {
                print("Bot has Taken over player Id: " + playerID);

                p.id = $"BP{p.photonIndex}";
                p.name = $"Bot Player {i}";
                p.isBot = true;
                p.isMaster = false;
                p.isOwn = false;
            }
        }

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }

    //public List<Player> SortMultiplayerPositions()
    //{
    //    List<Player> p = PlayerManager.instance.players;
    //    int myIdIndex = p.FindIndex(x=>x.id==PlayerProfile.Player_UserID);

    //    if (myIdIndex == -1)
    //    {
    //        Debug.Log("ID not found in the list.");
    //        return PlayerManager.instance.players;
    //    }

    //    List<Player> sortedList = new List<Player>();

    //    // Add elements after your ID
    //    for (int i = myIdIndex; i < p.Count; i++)
    //    {
    //        sortedList.Add(p[i]);
    //    }

    //    // Add elements before your ID
    //    for (int i = 0; i < myIdIndex; i++)
    //    {
    //        sortedList.Add(p[i]);
    //    }

    //    return sortedList;
    //}

    public void Deal()
    {
     //   playButton.SetActive(false);

        cardDeck.DistributeCards();
    }

    public void StartBid()
    {
        cardHand.SortHandCards();
        cardHand.StartCoroutine(cardHand.ShowPlayerCards());
        //
        bidManager.StartBid(this.totalPlayers, this.currentPlayerIndex);
    }

    //Only for multiplayer
    public void GetBidFromPlayers(string playerId, int photonIndex, int selectedBid)
    {
        bidManager.OnGetPlayerBid(playerId, photonIndex, selectedBid);
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

        currentPlayerIndex = (currentPlayerIndex + 1) % this.totalPlayers;

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
