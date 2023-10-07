using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PartnerObject : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Text gameScore;
    public Image profileImage;
    public GameObject imageLoader;


    public void OnClick()
    {
        print("Selected Partner");
    }


}
