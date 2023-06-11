using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGamePanel : UIPanel
{
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(System.Action<object[]> callBack, params object[] parameters)
    {
    }

    public void OnCloseButton()
    {
        GameplayManager.instance.StartNewGame();
        Hide();
    }
}
