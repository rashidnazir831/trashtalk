using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPartnerPanel : MonoBehaviour
{
    public Transform container;

    private void OnEnable()
    {
        DisableAll();
        SetData(2);
    }

    public void SetData(int n)
    {
        for(int i = 0; i < n; i++)
        {
            if (i <= container.childCount)
            {
                container.GetChild(i).gameObject.SetActive(true);
            }
        }
    }


    void DisableAll()
    {
        foreach(Transform child in container)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OnSkipButton()
    {
        this.gameObject.SetActive(false);
    }

}
