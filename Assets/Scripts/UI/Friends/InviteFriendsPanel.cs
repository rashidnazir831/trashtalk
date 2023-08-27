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
        //UIEvents.ShowPanel(Panel.Popup);
        //UIEvents.UpdateData(Panel.Popup, null, "SetData", "Invite sent to your selected friends", "","OK");

        string roomName = "SAND_" + Random.Range(99, 9999);

        SendGameRequest(roomName, PlayerProfile.Player_UserName, PlayerProfile.Player_UserID, "guest_a6ce9afb7f8d47a5bc2aa25c50c6b655_userID");

        PhotonRoomCreator.instance.CreateRoomOnPhoton(true, roomName);
       

        //foreach (var item in selectedUsers)
        //{
        //    SendGameRequest(roomName, PlayerProfile.Player_UserName, PlayerProfile.Player_UserID,item.UserId);
        //}
    }


    void SendGameRequest(string roomName, string playerName, string playerUserID, string friendUserID)
    {
        Debug.Log(roomName);
        Debug.Log(playerName);
        Debug.Log(playerUserID);
        Debug.Log(friendUserID);

        //SendGameRequest to friend using Pun chat
        if (PhotonChat.Instance.chatClient == null)
        {
            Debug.LogError("Chat Client was null");
            PhotonChat.Instance.Connect();
        }
        PhotonChat.Instance.RequestAndSendMessage(friendUserID, roomName,()=>{

         //   UIEvents.HidePanel(Panel.TabPanels);
         //   UIEvents.ShowPanel(Panel.GameplayPanel);

            //UIEvents.ShowPanel(Panel.Popup);
            //UIEvents.UpdateData(Panel.Popup, (data)=> {
            //if ((int)data[0] == 2)//on ok
            //{
            //        Global.isMultiplayer = true;
            //    //    UIEvents.HidePanel(Panel.TabPanels);
            //    //    UIEvents.ShowPanel(Panel.GameplayPanel);
            //}

            //}, "SetData", "Invite sent to your selected friends", "", "OK");
        });
    }



    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
