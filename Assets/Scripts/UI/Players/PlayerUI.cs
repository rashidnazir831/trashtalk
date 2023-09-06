using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public PlayerBidUI bidUI;
    public GameObject cardCountContainer;
    public Text myBids;
    public Text cardsCountText;
    public Text bidCount;

    public TextMeshProUGUI nameText;
    public Text gameScore;
    public Image profileImage;
    public GameObject imageLoader;
    public Image profileBG;
    //private string profileImageURL = "https://i.pravatar.cc/300";

    System.Action<int> callBack;

    //public MutiplayerData playerData;

    private void OnEnable()
    {
        if(Global.isMultiplayer)
            SetUI();
        else
            SetUI("Bot");
    }

    public void SetUI(string name="Waiting...",Sprite botSprite=null, int score=0, string imageUrl=null)
    {
        if (nameText!=null)
          nameText.text = name;
        if (gameScore != null)
            gameScore.text = score.ToString();


        if(profileImage !=null)
        {
            //Profile work
            if (botSprite !=null)
            {
                imageLoader.SetActive(false);

                this.profileImage.sprite = botSprite;

                return;
            }

            if (imageUrl != null && imageUrl != "")
            {
                ImageCacheManager.instance.CheckOrDownloadImage(imageUrl, this.profileImage, DownloadCallBack);
            }
            else
                imageLoader.SetActive(false);
        }


    }
    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

    public void UpdateCardCount(int cardCount)
    {
//        print(name);
        cardCountContainer.SetActive(cardCount > 0);

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
      //  myBids.text = $"{bid}";
        this.bidUI.gameObject.SetActive(false);
        this.callBack(bid);
    }

    public void UpdateBids(int totalBids, int bidWon)
    {

        if (bidCount != null)
            bidCount.text = $"{bidWon}/{totalBids}";

        if (myBids != null)
            myBids.text = $"{bidWon}/{totalBids}";
    }

    public void ResetUI()
    {
        if (bidCount != null)
            bidCount.text = "";
        if (myBids != null)
            myBids.text = "0";

        if(cardCountContainer)
            cardCountContainer.SetActive(false);

        HideBidUI();
    }

    public void WinAnimation()
    {
        LeanTween.scale(gameObject, new Vector2(1.1f,1.1f), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(()=> {
            LeanTween.scale(gameObject, Vector2.one, 0.5f).setEase(LeanTweenType.easeInOutQuad);

        });
    }

    public void SetTurnIndication(bool isTurn)
    {
        if (profileBG == null)
            return;
        profileBG.color = isTurn?Color.red:Color.white;
    }

    public void HideBidUI()
    {
        this.bidUI.gameObject.SetActive(false);
    }
}
