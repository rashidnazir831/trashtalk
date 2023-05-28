using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public PlayerBidUI bidUI;
    public Text myBids;
    public Text cardsCountText;


    System.Action<int> callBack;

    public void UpdateCardCount(int cardCount)
    {
        cardsCountText.text = $"{cardCount}";
    }

   public void ShowBidUI(int bidCount = -1,System.Action<int> callBack=null)
   {
        this.callBack = callBack;
        this.bidUI.gameObject.SetActive(true);
        this.bidUI.UpdateUI(bidCount, SelectBid);
   }

    void SelectBid(int bid)
    {
        myBids.text = $"{bid}";
        this.bidUI.gameObject.SetActive(false);
        this.callBack(bid);
    }

    public void HideBidUI()
    {
        this.bidUI.gameObject.SetActive(false);
    }
}
