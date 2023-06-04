using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InviteFriendsPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;

    public Button inviteButton;

    List<GameObject> selectedList;

    private void Start()
    {
        selectedList = new List<GameObject>();
        inviteButton.interactable = false;
        ShowList();
    }

    void ShowList()
    {
        ClearContianer(container);

        for (int i = 0; i < 6; i++)
        {
            GameObject obj = Instantiate(friendItem, container, false);
            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(), OnSelect);
        }
    }

    void OnSelect(GameObject item, bool isSelected)
    {
        if (isSelected)
            selectedList.Add(item);
        else
            selectedList.Remove(item);

        inviteButton.interactable = (selectedList.Count > 0);
    }

    public void OnInviteButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Invite sent to your selected friends", "OK","");
    }

    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

   
}
