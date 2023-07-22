using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TrashTalk;

public class InviteFriendsPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;

    public Button inviteButton;

  //  List<GameObject> selectedList;
    List<User> selectedUsers;

    private void Start()
    {
        //    selectedList = new List<GameObject>();
        selectedUsers = new List<User>();
        inviteButton.interactable = false;
        ShowList();
    }

    void ShowList()
    {
        ClearContianer(container);

        List<User> users = PlayerProfile.instance.globalUsers;

        foreach(User user in users)
        {
            GameObject obj = Instantiate(friendItem, container, false);
            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(),user, OnSelect);
        }

        //for (int i = 0; i < users.Count; i++)
        //{
        //    GameObject obj = Instantiate(friendItem, container, false);
        //    obj.GetComponent<FriendItem>().SetData( transform.GetSiblingIndex(), OnSelect);
        //}
    }

    void OnSelect(User user, bool isSelected)
    {
        if (isSelected)
            selectedUsers.Add(user);
        else
            selectedUsers.Remove(user);

        inviteButton.interactable = (selectedUsers.Count > 0);
    }

    public void OnInviteButton()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Invite sent to your selected friends", "","OK");
    }

    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
