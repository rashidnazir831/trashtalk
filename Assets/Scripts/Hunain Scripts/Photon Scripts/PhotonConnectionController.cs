﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using TrashTalk;
using System.Linq;

public class PhotonConnectionController : MonoBehaviourPunCallbacks
{
    public static bool isChatConnected;
    //[Space]
    //[Header("Invitation Panel")]
    //public InvitationPanel invitationPanel;
    public static PhotonConnectionController Instance;

    public TabPanels tabPanels;

    private void Awake()
    {
        Instance = this;
    }


    internal void JoinPhotonLobbyAgain()
    {
        if (PhotonNetwork.InRoom)
        {
            WaitingLoader.instance.ShowHide(true);
            PhotonNetwork.LeaveRoom();
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }
        if (!PhotonNetwork.InLobby)
        {
            WaitingLoader.instance.ShowHide(true);
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            //Debug.LogError("OnConnectedToMaster Region: " + PhotonNetwork.CloudRegion);
            PhotonNetwork.JoinLobby(Default);

        }
        if (!PhotonNetwork.InLobby)
        {
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            Debug.Log("OnConnectedToMaster Region: " + PhotonNetwork.CloudRegion);
            PhotonNetwork.JoinLobby(Default);
            //invitationPanel.ChatSetting();
        }

        PhotonRoomCreator.instance.setPhotonProps();
        if (isChatConnected == false)
        {
            PhotonChat.Instance.Connect();
        }
    }


    public void ConnectingToPhoton()
    {
        tabPanels.UpdateUI("coins");
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("PageNo", 1);
        
        //  PageNo
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.globalDatabaseUsers, Method.POST, null, keyValuePairs, OnGetGlobalUsers, OnFailGlobalUser, CACHEABLE.NULL, true, null);

        Debug.Log("ConnectingToPhoton. . .");
        string gameVersion = "0.0.1";
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AutomaticallySyncScene = true;   // was set to true
        PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.AuthValues.UserId = PlayerProfile.Player_UserID; // alternatively set by server
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public void OnGetGlobalUsers(JObject resp, long arg2)
    {
        var globalUsers = GlobalUsers.FromJson(resp.ToString());

        PlayerProfile.instance.globalUsers = globalUsers.data.data;

        Debug.Log("Total Global users: " + PlayerProfile.instance.globalUsers.Count);
    }

    void OnFailGlobalUser(string msg)
    {
        Debug.Log("Fail Global Users:  " + msg);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
