﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class PhotonRoomCreator : MonoBehaviourPunCallbacks
{
    private Hashtable _myCustomProperties = new Hashtable();


    [Space]
    public static bool IsPublicRoom = false;
    public string RoomID = "";

    [Header("Error Panel")]
    public GameObject roomFullPanel;
    //[Space]
    //[Header("PopUpAnimationController")]
    //public PopUpAnimationController popUpAnimationController;
    //[Space]
    //[Header("Lobby Screens")]
    //public GameObject loadingScreen;
    //public GameObject VsScreen;
    //[Space]
    //[Header("Player Details")]
    //public PlayerProfile playerProfile;
    //[Space]
    //[Header("ApiController")]
    //public ApiController apiController;
    //public static GameObject single;

    public static PhotonRoomCreator instance;

    private void Awake()
    {
        instance = this;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby()");
        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom() Player: "+ otherPlayer);
    }


    /// <summary>
    /// Main Method for creating Rooms
    /// </summary>
    /// <param name="isPublicRoom"> Room Category </param>
    private void CreateNewRoom(bool isPublicRoom = true, string roomName = "")
    {
        if (PhotonNetwork.InLobby)
        {
            if (isPublicRoom == false)
            {
                if (roomName == "" || string.IsNullOrEmpty(roomName))
                {
                    Debug.LogError("Please Provide Room Name");
                    return;
                }
                foreach (var item in RoomListCaching.cachedRoomList)
                {
                    if (item.Value.Name.Equals(roomName))
                    {
                        if (item.Value.PlayerCount == item.Value.MaxPlayers)
                        {
                            Debug.LogError("Game already exist, but the room is full");
                            roomFullPanel.SetActive(true);
                            return;
                            //Open screen (Room is full)
                        }
                        else
                        {
                            Debug.LogError("Room already Exist, Joining to the room");
                            break;
                        }
                    }
                }
                WaitingLoader.instance.ShowHide(true);


                //PhotonNetwork.GameVersion = "0.0.1";//MasterManager.GameSettings.GameVersion;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 7;//byte.Parse(roomLimit_InputField.text);
                roomOptions.PublishUserId = true;
                roomOptions.IsVisible = isPublicRoom;
                _myCustomProperties["Host"] = PhotonNetwork.LocalPlayer.UserId;
                roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Host" };
                roomOptions.CustomRoomProperties = _myCustomProperties;
                IsPublicRoom = false;
                RoomID = roomName.ToUpper();

                Debug.Log("RoomID: " + RoomID);

                PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
            }
            else
            {
                WaitingLoader.instance.ShowHide(true);

                IsPublicRoom = isPublicRoom;
                if (string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName))
                {
                    PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
                }
                string gameVersion = "0.0.1";
                PhotonNetwork.GameVersion = gameVersion;//MasterManager.GameSettings.GameVersion;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 4;//byte.Parse(roomLimit_InputField.text);
                roomOptions.PublishUserId = true;
                roomOptions.IsVisible = isPublicRoom;
                 _myCustomProperties["Host"] = PhotonNetwork.LocalPlayer.UserId;
                roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Host" };
                roomOptions.CustomRoomProperties = _myCustomProperties;
                int randomNo = UnityEngine.Random.Range(99, 9999);
                roomName = string.IsNullOrEmpty(roomName) ?"SAND_"+ randomNo:roomName;
                RoomID = roomName.ToUpper();
                PhotonNetwork.CreateRoom(RoomID , roomOptions , TypedLobby.Default);
            }
        }
        else
        {
            Debug.LogError("Player Not in Lobby");
        }
    }

    public void CreateRoomOnPhoton(bool publicRoom = true,string roomName = "")
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            CreateNewRoom(publicRoom , roomName);
        }
        else
        {
            if (!PhotonNetwork.IsConnected)
            {
                ReConnectingToPhoton();
            }
            else if (!PhotonNetwork.InLobby)
            {
                Debug.LogError("Player Not in Lobby");
            }
            else
            {
                Debug.LogError("Some thing else happend");
            }
        }
    }

    public void ReConnectingToPhoton()
    {
        Debug.Log("ConnectingToPhoton. . .");

        string gameVersion = "0.0.1";
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.UserId = PlayerProfile.Player_UserID; // alternatively set by server
        PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom successfully: Room ID: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("OnJoinRoomFailed");
        if (!PhotonNetwork.InLobby)
        {
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("GameObject Call LeaveRoom: " + gameObject.name, gameObject);
            PhotonNetwork.LeaveRoom();
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()");
        PlayerProfile.RoomId = RoomID;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            RoomOptions roomOptions = new RoomOptions();
            _myCustomProperties["RoomId"] = RoomID;
            roomOptions.CustomRoomPropertiesForLobby = new string[1] { "RoomId" };
            roomOptions.CustomRoomProperties = _myCustomProperties;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
        }

        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }

    }

    #region new code
    public void setPhotonProps()
    {
        string name = PlayerProfile.Player_UserName;
        string pic = TextureConverter.Texture2DToBase64(PlayerProfile.Player_rawImage_Texture2D);
        string email = PlayerProfile.Player_Email;
        string country = PlayerProfile.PlayerCountry;
        Hashtable hash = new Hashtable();
        hash["Name"] = name;
        hash["Picture"] = pic;
        hash["Email"] = email;
        hash["Country"] = country;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    #endregion

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }


    private void MoveToWaitingScreen()
    {
        Debug.Log("MoveToWaitingScreen()");
        //GetOponentPicture();
        //popUpAnimationController.PingPongAnimation(loadingScreen);
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //{
        //    StartCoroutine(Show_Vs_Screen_To_LocalPlayer());
        //}
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError("OnPlayerEnteredRoom playerName: " + newPlayer.NickName);
        //if (newPlayer != PhotonNetwork.LocalPlayer)
        //{
        //    StartCoroutine(MoveToVs_Screen());
        //}
    }

    public IEnumerator MoveToVs_Screen()
    {
        Debug.LogError("MoveToVs_Screen()");
        yield return null;
        ////yield return new WaitForSeconds(3f);
        //popUpAnimationController.PingPongAnimation(loadingScreen);
        //popUpAnimationController.PingPongAnimation(VsScreen);
        ////yield return new WaitForSeconds(4f);
        //yield return null;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.LoadLevel(1);
        //}

    }
}