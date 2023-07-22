using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TrashTalk;

public class FriendItem : MonoBehaviour
{
    public GameObject addButton;
    public GameObject inviteButton;
    private Transform selectionBtton;

    public Text nameText;
    public Text wonText;


    bool isSelected = false;

    Action<GameObject, bool> selectionCallBack;

    User user;

    public void SetData(int type, User user, Action<GameObject, bool> selectCallBack)
    {
        this.user = user;
        this.selectionCallBack = selectCallBack;
        addButton.SetActive(type == 0 || type==1);
        inviteButton.SetActive(type == 2);
        selectionBtton = (type == 0 || type == 1) ? addButton .transform: inviteButton.transform;

        nameText.text = this.user.FullName;
        wonText.text = $"Won {this.user.winCount} matches" ;
    }

    public void SelectUnselect()
    {
        this.selectionCallBack(gameObject, isSelected = !isSelected);
        selectionBtton.GetChild(0).gameObject.SetActive(!isSelected);
        selectionBtton.GetChild(1).gameObject.SetActive(isSelected);

    }
}
