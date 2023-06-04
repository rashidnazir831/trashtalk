using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameSelectPanel : UIPanel
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

    public void OnPractice()
    {
        OpenTabPanel();
    }

    public void OnMultiplayer()
    {
        OpenTabPanel();
    }

    void OpenTabPanel()
    {
        UIEvents.ShowPanel(Panel.TabPanels);
        Hide();
    }
}
