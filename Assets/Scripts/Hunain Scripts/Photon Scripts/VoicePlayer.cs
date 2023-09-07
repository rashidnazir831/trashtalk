using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;

public class VoicePlayer : MonoBehaviour
{
    public bool isLocalPlayer = false;
    public string userId = "";


    public void SetData(Photon.Realtime.Player player)
    {
        isLocalPlayer = player.IsLocal;
        userId = player.UserId;
        GetComponent<AudioSource>().enabled = GetComponent<Speaker>().enabled = !isLocalPlayer;
    }
}