using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceManager : MonoBehaviourPun
{

    public Recorder recorder;


    public static VoiceManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void EnableDisableVoiceManager()
    {

        if (recorder != null && Global.isMultiplayer)
        {
            gameObject.SetActive(true);
            recorder.TransmitEnabled = true; // Enable voice transmission
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public bool EnableDisableAudioTransmition()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
        return recorder.TransmitEnabled;
    }
}