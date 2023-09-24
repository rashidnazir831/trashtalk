using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using TrashTalk;

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

    private Hashtable _myCustomProperties = new Hashtable();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private GameplayManager()
    {

    }

    private void OnEnable()
    {
        //print("onenable");
        //ResetPanelData();

        if (!Global.isMultiplayer)
        {
            PlayerManager.instance.ClearPlayers();
            PlayerManager.instance.AddPlayer("YOU", $"P{0}", null, true,true, false, 0);
            PlayerManager.instance.AddPlayer("Bot 1", $"BP{1}", null, false, false, true, 1);
            PlayerManager.instance.AddPlayer("Bot 2", $"BP{2}", null, false, false, true, 2);
            PlayerManager.instance.AddPlayer("Bot 3", $"BP{3}", null, false, false, true, 3);
        }
        else
        {
            ShowMultiplayerMessage(true, Photon.Pun.PhotonNetwork.IsMasterClient?"Waiting other players to join game.":"Waiting master to start game.");
        }

        cardDeck = GetComponentInChildren<CardDeck>();
        bidManager = GetComponent<BidManager>();
        botTrick = new BotTrick();
        RoundManager.instance.ClearAllRounds();
      //  this.totalPlayers = PlayerManager.instance.players.Count;
        //     this.totalPlayerPlayed = 0;
        SoundManager.Instance.PlayBackgroundMusic(Sound.Music);

        print("is multiplayer: " + Global.isMultiplayer);

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
        //playButton.SetActive(true);
        ResetGame();
    }


    public void ResetGame()
    {
        ResetContainers();
        bidManager.ClearPlayersBid();
        //playButton.SetActive(true);
        this.totalPlayers = PlayerManager.instance.players.Count;


        if (Global.isMultiplayer)
            this.currentPlayerIndex = PlayerManager.instance.GetMasterIndex();
        else
            this.currentPlayerIndex = Random.Range(0, totalPlayers);


        //    PlayerManager.instance.ClearPlayersData();

        TrickManager.ResetTrick();
      //  TableController.instance.ClearCards();

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ResetUI");

        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");
    }

    public void OnPlayGameButton()
    {
        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient) {

            Photon.Pun.PhotonNetwork.CurrentRoom.IsOpen = false;
            Photon.Pun.PhotonNetwork.CurrentRoom.IsVisible = false;
            string shuffledCards = cardDeck.GetShuffleCardsString();
            PhotonRPCManager.Instance.SpawnPlayers(shuffledCards);

          //  return; //will remove this line to start game
            string[] playerIds = PlayerManager.instance.GetMultiplayerIds();

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("MatchType", "Multiplayer");


            for (int i = 0; i < playerIds.Length; i++)
            {
                keyValuePairs.Add($"UserIDs[{i}]", playerIds[i]);
            }


            keyValuePairs.Add("CoinsToPlay", Global.coinsRequired);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.startGameFunction, Method.POST, null, keyValuePairs,

            (JObject resp, long arg2) => {
                var response = GameResponse.FromJson(resp.ToString());
                RoomOptions roomOptions = new RoomOptions();
                _myCustomProperties["GameId"] = response.data.GameID;
                roomOptions.CustomRoomPropertiesForLobby = new string[1] { "GameId" };
                roomOptions.CustomRoomProperties = _myCustomProperties;
                PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);

            }
            , (msg)=> {
                print(msg);
            }, CACHEABLE.NULL, false, null);

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
        if (Global.isMultiplayer)
        {
            PlayerProfile.Player_coins -= Global.coinsRequired;

            if (EventManager.UpdateUI != null)
                EventManager.UpdateUI.Invoke("UpdateCoins");
        }
      

        UIEvents.HidePanel(Panel.EndGamePanel);//it is needed to be closed, if other players are still on endgame screen and master started game already

        ShowMultiplayerMessage(false);
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
        int bp = 0;
        for(int i = 0; i < 4; i++)
        {
            if(PlayerManager.instance.player[i].id == null)
            {
                bp++;
                PlayerManager.instance.players[i].name = $"Bot Player {bp}";
                PlayerManager.instance.players[i].isOwn = false;
                PlayerManager.instance.players[i].isMaster = false;
                PlayerManager.instance.players[i].isBot = true;
                PlayerManager.instance.players[i].id = $"BP{bp}";
                //   PlayerManager.instance.players[i].tablePosition = 0;
             //   PlayerManager.instance.players[i].photonIndex = i;
            }
        }




        PlayerManager.instance.player = PlayerManager.instance.SortMultiplayerPositions();

        for (int i = 0; i < PlayerManager.instance.player.Count; i++)
        {
            print("and Here player ids: " + PlayerManager.instance.player[i].id);
            print("and Table Position is: " + PlayerManager.instance.player[i].tablePosition);

        }

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

            //    p.id = $"BP{p.photonIndex}";
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

    void SetPlayerTurnIndication(Player p=null, bool removeFromAll=false)
    {
        foreach(Player pl in PlayerManager.instance.players)
        {
            if (removeFromAll)
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetTurnIndication", pl.tablePosition, false);
            else
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetTurnIndication", pl.tablePosition, pl.tablePosition == p.tablePosition);
        }
    }

    public void StartGame()
    {
        this.currentPlayerIndex = PlayerManager.instance.GetMasterIndex();
        StartTricks();
    }

    public void StartTricks()
    {
        this.totalPlayerPlayed = 0;
        //currentPlayerIndex = (currentPlayerIndex + 1) % 4;


        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
        {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {      
                PhotonRPCManager.Instance.SetPlayerTurn(PlayerManager.instance.player[this.currentPlayerIndex].id, PlayerManager.instance.player[this.currentPlayerIndex].photonIndex);
            }
            return;
        }

        PlayTurn();
    }

    public void GetPlayerTurn(string playerID,int photonIndex)
    {
        print("It is: " + playerID + " Turn");
        print("and photon index is : " + photonIndex);
        this.currentPlayerIndex = PlayerManager.instance.GetPlayerIndexByID(playerID);
        print("palyer index here: " + this.currentPlayerIndex);
        PlayTurn();
    }

    public void PlayTurn()
    {
       Player currentPlayer = PlayerManager.instance.players[this.currentPlayerIndex];
        
       PlayerManager.instance.SetPlayerTurn(this.currentPlayerIndex);
       SetPlayerTurnIndication(currentPlayer); 
        if (currentPlayer.isBot)
        {

            if (Global.isMultiplayer)
            {
                if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
                    StartCoroutine(PlayBotTurnByMaster(currentPlayer));
            }
            else
                StartCoroutine(BotPlay(currentPlayer));
        }
        else
        {
            if (currentPlayer.isOwn)
            {
                cardHand.ActiveMainPlayerCards();
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", true);
                print("your turn");
            }
            else
            {
                //if we want to show anything for other player
            }
        }
    }

    IEnumerator PlayBotTurnByMaster(Player botPlayer)
    {
        yield return new WaitForSeconds(1f);
        List<Card> hand = botPlayer.hand;
        //Card playedCard = botPlayer.PlayCard(0);
        Card playedCard = botTrick.GetBestCard(botPlayer.hand);

        SendCardPlacedData(botPlayer.id, playedCard.data.shortCode);
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
        print("currentPlayerIndex" + this.currentPlayerIndex);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(botPlayer.tablePosition),2.5f,true,false, ()=> {
            TrickManager.AddCard(playedCard);

            botPlayer.hand.Remove(playedCard);
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", this.currentPlayerIndex, botPlayer.hand.Count);

            TrickManager.HighlightLowCards();

            DecideNext();

        });
    //    yield return new WaitForSeconds(1f);

    }

    public void PlaceCardOnTable(Player player, Card playedCard)
    {
        //cardHand.UpdateCardArrangement();
        cardHand.ActiveMainPlayerCards(false);

        if (Global.isMultiplayer)
        {
            SendCardPlacedData(player.id, playedCard.data.shortCode);
            return;
        }
            

//        cardHand.ActiveMainPlayerCards(false);

        playedCard.MoveCard(TableController.instance.GetPlayerShowCardTransform(player.tablePosition), 2.5f,true,false,()=> {
            TrickManager.HighlightLowCards();
            cardHand.UpdateCardArrangement();
        });

        player.hand.Remove(playedCard);
        cardHand.OnUseHandCard(playedCard);
        TrickManager.AddCard(playedCard);
        DecideNext();
    }

    void SendCardPlacedData(string playerId, string cardCode)
    {
        PhotonRPCManager.Instance.PlacedCardByPlayer(playerId, cardCode);
    }

    public void OnPlacedCardByMultiplayer(string playerId, string cardCode)
    {
        //
        print("Player placed was : " + playerId);
        print("Player placed card: " + cardCode);
        Card card = cardDeck.GetCard(cardCode);
        Player currentPlayer = PlayerManager.instance.GetPlayerById(playerId);
        print("and table position of that player is: " + currentPlayer.tablePosition);


        currentPlayer.hand.Remove(card);
        cardHand.OnUseHandCard(card);
        TrickManager.AddCard(card);

        if (currentPlayer.isOwn)
        {
            card.MoveCard(TableController.instance.GetPlayerShowCardTransform(currentPlayer.tablePosition), 2.5f, true, false, () => {
                TrickManager.HighlightLowCards();
                cardHand.UpdateCardArrangement();
            });
        }
        else
        {
            card.gameObject.SetActive(true);
            card.SwitchSide(true);

            card.MoveCard(TableController.instance.GetPlayerShowCardTransform(currentPlayer.tablePosition), 2.5f, true, false, () => {
                //    TrickManager.AddCard(card);

                //    currentPlayer.hand.Remove(card);
                UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", currentPlayer.tablePosition, currentPlayer.hand.Count);
                TrickManager.HighlightLowCards();
                //    DecideNext();

            });
        }

        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            DecideNext();
        }
        else
        {
            this.totalPlayerPlayed++;
            print("in else total played player: " + this.totalPlayerPlayed);
            if (this.totalPlayerPlayed == this.totalPlayers)
            {
                Invoke("OnWinTrick", 1);
            }
        }
    }

    void DecideNext()
    {
        this.totalPlayerPlayed++;

        if (this.totalPlayerPlayed == this.totalPlayers)
        {
            Invoke("OnWinTrick", 1);
            return;
        }

        this.currentPlayerIndex = (this.currentPlayerIndex + 1) % this.totalPlayers;


        if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
        {
            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                PhotonRPCManager.Instance.SetPlayerTurn(PlayerManager.instance.player[this.currentPlayerIndex].id, PlayerManager.instance.player[this.currentPlayerIndex].photonIndex);
            }
            return;
        }

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
            RoundManager.instance.AddCurrentRoundProgress();
            SetPlayerTurnIndication(null, true);

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

        this.currentPlayerIndex = this.trickWinner.tablePosition;

        StartTricks();
    }

    //here we will show the animations or bonuses which players gets
    void OnRoundOver()
    {
        print("round over");

        Invoke("ShowRoundResult", 1);
        //UIEvents.ShowPanel(Panel.GameOverPanel);
    }

    void ShowRoundResult()
    {
  //    if(needNextRound) logic will be here
        StartNextRound();
        UIEvents.ShowPanel(Panel.EndGamePanel);
    }

    public void StartNextRound()
    {
        //  if Need next round
        //         logic
      //  UIEvents.HidePanel(Panel.EndGamePanel);

        if (!Global.isMultiplayer)
        {
            SetPlayButton(true);
            StartNewGame();
            return;
        }

    //    TableController.instance.ShowSideTable(false);

        if (Photon.Pun.PhotonNetwork.InRoom && Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            SetPlayButton(true);
          //  ResetGame();
        }
        else
        {
            ShowMultiplayerMessage(true, "Waiting master to start round.");
        }
        ResetGame();
    }

    public void StartRound()
    {
        ShowMultiplayerMessage(false);
    }

    public void ShowMultiplayerMessage(bool show, string message="")
    {
        UIEvents.UpdateData(Panel.GameplayPanel, null, "ShowHideMessage", show, message);
    }

    public void OnWinningGame(Player winner)
    {
        print("1 Winner is: " + winner.name);
        if (!Global.isMultiplayer)
            return;

        int coins = (this.totalPlayers / 2) * Global.coinsRequired;
        print("2 Winner is: " + winner.name);

        if (winner.isOwn || winner.partner.isOwn)
        {
            print("3 Winner is: " + winner.name);

            PlayerProfile.Player_coins += coins;

            if (EventManager.UpdateUI != null)
                EventManager.UpdateUI.Invoke("UpdateCoins");

            string[] winners = GetWinnerIds(winner);

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("GameID", Global.currentGameId);


             for (int i = 0; i < winners.Length; i++)
            {
                keyValuePairs.Add($"UserIDs[{i}]", winners[i]);
            }

            keyValuePairs.Add("winCoins", coins);

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.endGameFunction, Method.POST, null, keyValuePairs,

            (JObject resp, long arg2) =>
            {


            }
            , (msg) =>
            {
                print(msg);
            }, CACHEABLE.NULL, false, null);
        }
    }

    public string[] GetWinnerIds(Player winner)
    {
        List<string> ids = new List<string>();

        if (!winner.isBot)
            ids.Add(winner.id);

        if(!winner.partner.isBot)
            ids.Add(winner.partner.id);

        return ids.ToArray();
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