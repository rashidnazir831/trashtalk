using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    public void OnStartButton()
    {
        UIEvents.ShowPanel(Panel.GameplayPanel);
        UIEvents.HidePanel(Panel.FriendsPanel);
    }
}
