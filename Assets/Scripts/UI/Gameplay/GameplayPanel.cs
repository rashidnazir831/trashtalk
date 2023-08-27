using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayPanel : UIPanel
{
    public GameObject cardIntroPanel;
    private Action<object[]> callBack;


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

        switch (type)
        {
            case "ShowCardIntro":
                this.callBack = callBack;
                ShowCardIntro();
                break;
        }
    }

    void ShowCardIntro()
    {
        cardIntroPanel.SetActive(true);
        cardIntroPanel.GetComponentInChildren<AnimationEvents>().SetCallBack(this.callBack);
    }

    public void OnHomeButton()
    {
        Time.timeScale = 0;
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (obj) => {

            Time.timeScale = 1;

            if ((int)obj[0] == 2)//on yes
            {

                if (Global.isMultiplayer && Photon.Pun.PhotonNetwork.InRoom)
                {
                    Photon.Pun.PhotonNetwork.LeaveRoom();
                }

                Hide();
                UIEvents.ShowPanel(Panel.TabPanels);
                //   UIEvents.ShowPanel(Panel.GameSelectPanel);
            }
        }, "SetData", "Are you sure you want to leave game?", "NO", "YES");

    }
}
