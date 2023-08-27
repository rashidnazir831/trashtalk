using UnityEngine;
using Photon.Pun;
using System;

public class PhotonRPCManager : MonoBehaviourPunCallbacks
{
    private static PhotonRPCManager instance;


    // Singleton pattern to ensure only one instance of PhotonRPCManager exists
    public static PhotonRPCManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonRPCManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PhotonRPCManager");
                    instance = obj.AddComponent<PhotonRPCManager>();
                }
            }
            return instance;
        }
    }

    // Method to send an RPC to all relevant clients
    public void SendRPC(string methodName, RpcTarget target, params object[] parameters)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC(methodName, target, parameters);
        }
    }

    [ContextMenu("Test")]
    public void TestRPC()
    {
        SendRPC("TestCallBack_Rpc",RpcTarget.All,"Test");
    }

    [PunRPC]
    public void TestCallBack_Rpc(string data)
    {
        Debug.Log("data: " + data);
    }

    [PunRPC]
    public void SpawnPlayers_Rpc()
    {
        //Call spawning herh
    }



    internal void SpawnPlayers()
    {
        SendRPC("SpawnPlayers_Rpc", RpcTarget.All);
    }

    //TODO This function should be in a photon chat class
    public void OnGetGameRequest(string senderId, string roomId)
    {
        print("Got Invitation from: " + senderId);
        print("Invitation for Room Id: " + roomId);


        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (data) => {

            if ((int)data[0] == 2)//on yes
            {
                print("Accepted");
                PhotonChat.Instance.AcceptGameInvitation(PlayerProfile.Player_UserID,roomId);
            }
            else
            {
                print("Rejected");

            }

        }, "SetData", "You have received game invitation", "Reject", "Accept");
    }
}
