using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayPanel : UIPanel
{
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
    }

    public void OnHomeButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (obj) => {

            if ((int)obj[0] == 1)//on yes
            {
                Hide();
                UIEvents.ShowPanel(Panel.GameSelectPanel);
            }

        }, "SetData", "Are you sure you want to leave game?", "Yes", "No");

    }
}
