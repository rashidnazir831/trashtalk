using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FriendItem : MonoBehaviour
{
    public GameObject addButton;
    public GameObject inviteButton;
    private Transform selectionBtton;

    bool isSelected = false;

    Action<GameObject, bool> selectionCallBack;

    public void SetData(int type,Action<GameObject, bool> selectCallBack)
    {
        this.selectionCallBack = selectCallBack;
        addButton.SetActive(type == 0 || type==1);
        inviteButton.SetActive(type == 2);
        selectionBtton = (type == 0 || type == 1) ? addButton .transform: inviteButton.transform;
    }

    public void SelectUnselect()
    {
        this.selectionCallBack(gameObject, isSelected = !isSelected);
        selectionBtton.GetChild(0).gameObject.SetActive(!isSelected);
        selectionBtton.GetChild(1).gameObject.SetActive(isSelected);

    }
}
