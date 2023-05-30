using System.Collections;
using System.Collections.Generic;
using System;

public class PlayersUIPanel : UIPanel
{
    private PlayerUI[] playerUI;
    private Action<object[]> callBack;
    object[] data;

    private void Start()
    {
        playerUI = GetComponentsInChildren<PlayerUI>();
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {
        string type = (string)parameters[0];
        int playerNumber = 0;
        switch (type)
        {
            case "ShowBidUI":
                playerNumber = (int)parameters[1];
                int bidCount = (int)parameters[2];
                this.callBack = callBack;
                ShowBidUI(playerNumber, bidCount);
                break;
            case "HideAllBidsUIs":
                HideAllBidsUIs();
                break;
            case "UpdateCardCount":
                playerNumber = (int)parameters[1];
                int count = (int)parameters[2];
                UpdateCardCount(playerNumber,count);
                break;
            case "UpdateBidCount":
                playerNumber = (int)parameters[1];
                UpdateBidCount(playerNumber, (int)parameters[2], (int)parameters[3]);
                break;

        }
    }

    void ShowBidUI(int playerNumber, int bidCout=-1)
    {
        playerUI[playerNumber].ShowBidUI(bidCout,SelectBid);
    }

    void UpdateCardCount(int playerNumber, int count)
    {
        playerUI[playerNumber].UpdateCardCount(count);
    }

    void UpdateBidCount(int playerNumber, int bidWon, int totalBid)
    {
        playerUI[playerNumber].UpdateBids(totalBid, bidWon);
    }

    void SelectBid(int bid)
    {
        this.data = new object[] { bid };
        this.callBack(this.data);
    }

    void HideAllBidsUIs()
    {
        foreach(PlayerUI player in playerUI)
        {
            player.HideBidUI();
        }
    }


}