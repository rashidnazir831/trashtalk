using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ProfilePanel : UIPanel
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

    public void OnSubmitButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Profile updated", "OK", "");
    }
}
