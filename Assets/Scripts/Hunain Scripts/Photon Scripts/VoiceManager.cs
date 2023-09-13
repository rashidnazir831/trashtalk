using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceManager : MonoBehaviourPun
{

    private Recorder recorder;


    public static VoiceManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        recorder = GetComponent<Recorder>();
        if (recorder != null)
        {
            recorder.TransmitEnabled = true; // Enable voice transmission
        }
    }

    
    private void Update()
    {
        // You can use input or events to control voice transmission, e.g., push-to-talk
        if (Input.GetKey(KeyCode.Space))
        {
            StartVoiceTransmission();
        }
        else
        {
            StopVoiceTransmission();
        }
    }


    public void EnableDisableAudioTransmition()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
    }

    private void StartVoiceTransmission()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = true;
        }
    }

    private void StopVoiceTransmission()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = false;
        }
    }
}