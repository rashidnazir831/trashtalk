using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class EndGamePanel : UIPanel
{
    public  Button nextButton;
    public Button previousButton;
    public Sprite[] botImages;

    public Text roundText;

    public Text myBidText;
    public Text roundBagsText;
    public Text myBounusText;
    public Text myRoundPointsText;
    public Text myTotalBags;
    public Text myGabPenalty;
    public Text myTotalPoints;

    public Text opponentBidText;
    public Text opponentroundBagsText;
    public Text opponentBounusText;
    public Text opponentRoundPointsText;
    public Text opponentTotalBags;
    public Text opponentGabPenalty;
    public Text opponentTotalPoints;

    public EndGamePlayerUI myProfile;
    public EndGamePlayerUI myPartnerProfile;
    public EndGamePlayerUI opponentProfile1;
    public EndGamePlayerUI opponentProfile2;

    int currentRoundShowing = 1;
    List<Round> rounds;

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

    private void OnEnable()
    {
        rounds = RoundManager.instance.rounds;
        //for(int i = 0; i < r.Count; i++)
        //{
        //    print("Round:::::  " + (i+1));

        //    for(int j = 0; j < r[i].players.Count; j++)
        //    {
        //        print("Round Player:::::: " + r[i].players[j].bidPlaced);
        //    }
        //}

        currentRoundShowing = rounds.Count;

        SetUI();
    }

    public void OnNextButton()
    {
        currentRoundShowing++;
        SetUI();
    }

    public void OnPreviousButton()
    {
        currentRoundShowing--;
        SetUI();
    }

    void SetUI()
    { 
        nextButton.interactable = rounds.Count > currentRoundShowing;
        previousButton.interactable = currentRoundShowing > 1;

        Utility.DisableButtonChilds(nextButton);
        Utility.DisableButtonChilds(previousButton);

        Round currentRound = rounds[this.currentRoundShowing-1];
        Player myMainPlayer = currentRound.players[0];
        Player opponentMainPlayer = currentRound.players[1];

        print("appp: " + opponentMainPlayer.name);
        print("appp2: " + opponentMainPlayer.partner.name);

        roundText.text = $"ROUND {this.currentRoundShowing}";

        myBidText.text = $"{currentRound.players[0].bidPlaced}/{currentRound.players[0].bidWon}";
        roundBagsText.text = $"{0}";
        myBounusText.text = $"{0}";
        myRoundPointsText.text = $"{0}";
        myTotalBags.text = $"{0}";
        myGabPenalty.text = $"{0}";
        myTotalPoints.text = $"{this.currentRoundShowing * 200}";

        opponentBidText.text = $"{0}";
        opponentroundBagsText.text = $"{0}";
        opponentBounusText.text = $"{0}";
        opponentRoundPointsText.text = $"{0}";
        opponentRoundPointsText.text = $"{0}";
        opponentGabPenalty.text = $"{0}";
        opponentTotalPoints.text = $"{this.currentRoundShowing * -100}";

        myProfile.SetUI(myMainPlayer.name, null, 0, "https://i.pravatar.cc/300");
        myPartnerProfile.SetUI(myMainPlayer.partner.name, myMainPlayer.partner.isBot ? botImages[2] : null, 0, "https://i.pravatar.cc/300");
        opponentProfile1.SetUI(opponentMainPlayer.name, opponentMainPlayer.partner.isBot ? botImages[1] : null, 0, "https://i.pravatar.cc/300");
        opponentProfile2.SetUI(opponentMainPlayer.partner.name, opponentMainPlayer.partner.isBot ? botImages[3] : null, 0, "https://i.pravatar.cc/300");

    }

    public void OnCloseButton()
    {
    //    GameplayManager.instance.StartNextRound();
        Hide();
    }
}
