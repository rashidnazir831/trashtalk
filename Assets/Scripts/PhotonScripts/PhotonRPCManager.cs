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

    //[ContextMenu("Test")]
    //public void TestRPC()
    //{
    //    SendRPC("TestCallBack_Rpc",RpcTarget.All,"Test");
    //}

    //[PunRPC]
    //public void TestCallBack_Rpc(string data)
    //{
    //    Debug.Log("data: " + data);
    //}

    [PunRPC]
    public void SpawnPlayers_Rpc(string shuffledCards)
    {
        //Call spawning herh
        print("Got Shuffled cards from Master: " + shuffledCards);
        GameplayManager.instance.AnimateCardsScreen(shuffledCards);
    }



    internal void SpawnPlayers(string shuffledCards)
    {
        SendRPC("SpawnPlayers_Rpc", RpcTarget.All, shuffledCards);
    }
}
