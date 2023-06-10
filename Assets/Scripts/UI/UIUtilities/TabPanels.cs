using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TabPanels : UIPanel
{
    public Transform buttons;
    public Transform panels;

    int lastActiveIndex = 0;

    private void OnEnable()
    {
        SelectPanel(1);
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

        switch (type)
        {
            case "SelectPanel":
                int panelIndex = (int)parameters[1];
                SelectPanel(panelIndex);
                break;
        }

     }

    public void SelectPanel(int index)
    {
        if (index == lastActiveIndex)
            return;

        if(index == 3)
        {
            Hide();
            UIEvents.ShowPanel(Panel.GameSelectPanel);
            return;
        }

        if (index == 4)
        {
            Hide();
            UIEvents.ShowPanel(Panel.FriendsPanel);
            UIEvents.UpdateData(Panel.FriendsPanel, null, "SelectPanel", 2);
            return;
        }

        panels.GetChild(index).gameObject.SetActive(true);
        panels.GetChild(lastActiveIndex).gameObject.SetActive(false);

        lastActiveIndex = index;
    }

}
