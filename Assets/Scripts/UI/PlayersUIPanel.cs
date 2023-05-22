using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
        switch(type)
        {
            case "ShowBidUI":
                int playerNumber = (int)parameters[1];
                int bidCount = (int)parameters[2];
                this.callBack = callBack;
                ShowBidUI(playerNumber, bidCount);
                break;
            case "HideAllBidsUIs":
                HideAllBidsUIs();
                break;

        }
    }

    void ShowBidUI(int playerNumber, int bidCout=-1)
    {
        playerUI[playerNumber].ShowBidUI(bidCout,SelectBid);
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
