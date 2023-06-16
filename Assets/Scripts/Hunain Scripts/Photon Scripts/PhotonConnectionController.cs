using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PhotonConnectionController : MonoBehaviourPunCallbacks
{
    public static bool isChatConnected;
    //[Space]
    //[Header("Invitation Panel")]
    //public InvitationPanel invitationPanel;
    public static PhotonConnectionController Instance;


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
        if (isChatConnected == false)
        {
            PhotonChat.Instance.Connect();
        }
    }


    public void ConnectingToPhoton()
    {
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
