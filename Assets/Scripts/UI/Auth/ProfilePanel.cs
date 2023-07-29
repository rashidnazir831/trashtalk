using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class ProfilePanel : UIPanel
{

    public InputField displayName;
    public InputField email;
    public InputField password;

    PlayerProfile profile;

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

    private void OnEnable()
    {

        UpdateUI();
    }

    void UpdateUI()
    {
        displayName.text = PlayerProfile.Player_UserName;
        email.text = PlayerProfile.Player_Email;
        password.text = PlayerProfile.Player_Password;
    }

    public void OnSubmitButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Profile updated", "", "OK");
    }


}
