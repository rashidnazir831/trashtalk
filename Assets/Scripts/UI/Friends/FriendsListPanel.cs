using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsListPanel : MonoBehaviour
{
    public void OnStartButton()
    {
        UIEvents.ShowPanel(Panel.GameplayPanel);
        UIEvents.HidePanel(Panel.FriendsPanel);
    }
}
