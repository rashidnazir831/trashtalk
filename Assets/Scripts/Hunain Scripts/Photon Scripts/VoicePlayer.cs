using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class VoicePlayer : MonoBehaviour
{
    public bool isLocalPlayer = false;
    public string userId = "";




    public void SetData(Photon.Realtime.Player player)
    {
        isLocalPlayer = player.IsLocal;
        userId = player.UserId;
        EnableDisableAudioSource(!isLocalPlayer);
    }

    public void EnableDisableAudioSource(bool state)
    {
        GetComponent<AudioSource>().enabled = GetComponent<Speaker>().enabled = state;
    }
}