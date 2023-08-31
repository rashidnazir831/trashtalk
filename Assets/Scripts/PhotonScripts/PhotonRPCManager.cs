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

    [PunRPC]
    public void Place_Bid(string playerId, int photonIndex, int selectedBid)
    {
        //Call spawning herh
        GameplayManager.instance.GetBidFromPlayers(playerId, photonIndex, selectedBid);
    }

    internal void PlaceBid(string playerId, int photonIndex, int selectedBid)
    {

        SendRPC("Place_Bid", RpcTarget.All, playerId, photonIndex, selectedBid);
    }

    [PunRPC]
    public void Set_Player_Turn(string playerId, int photonIndex, int selectedBid)
    {
        //Call spawning herh
        GameplayManager.instance.GetPlayerTurn(playerId, photonIndex);
    }

    internal void SetPlayerTurn(string playerId, int photonIndex)
    {

        SendRPC("Set_Player_Turn", RpcTarget.All, playerId, photonIndex);
    }

    [PunRPC]
    public void Placed_Card_By_Player(string playerId, string cardCode)
    {
        //Call spawning herh
        GameplayManager.instance.OnPlacedCardByMultiplayer(playerId, cardCode);
    }

    internal void PlacedCardByPlayer(string playerId, string cardCode)
    {

        SendRPC("Placed_Card_By_Player", RpcTarget.All, playerId, cardCode);
    }
}
