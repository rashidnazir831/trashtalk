using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SignupPanel : UIPanel
{
    public void ConnectWithFacebook()
    {
        print("ConnectWithFacebook");
    }

    public void PlayAsGuest()
    {
        UIEvents.ShowPanel(Panel.TabPanels);
        Hide();
    }

    public void ConnectWithApple()
    {
        print("ConnectWithApple");
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
    }
}
