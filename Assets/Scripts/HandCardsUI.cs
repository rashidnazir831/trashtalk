using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandCardsUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public float radius = 50f;

    public RectTransform cardRect;

    private float CardWidth;
    public Transform centerPoint;

    //private List<Card> cardsInHands;

    private void Start()
    {
        //int total = transform.childCount;
        //float distance = 100 / 2; //size / 2
        //float x = 0;
        //float y = 0;

        //for (int i = 0; i < total; i++)
        //{
        //    transform.GetChild(i).transform.localPosition = new Vector2(x, y);
        //    x += distance;
        //}
        CardWidth = cardRect.rect.width;
    
    }

    // Call this method to update the card arrangement whenever a card is removed
    public void UpdateCardArrangement()
    {
        int total = transform.childCount;
        float maxAngle = 30;
        float fullAngle = -maxAngle;
        float anglePerCard = fullAngle / total;
        float firstAngle = CalcFirstAngle(fullAngle);
        float handWidth = CalcHandWidth(total);

     //   float percTwistedAngleAddYPos = 0.5f;
        float initialXPos =  -handWidth * 0.5f;

        for (int i = 0; i < total; i++)
        {
            Transform card = transform.GetChild(i).transform;
          //  cardsInHands.Add(card);
            float angleTwist = firstAngle + i * anglePerCard;

            float yDistance = Mathf.Abs(angleTwist);
            float xPos = initialXPos + i * (CardWidth + 20);

            float yPos = (-25) - yDistance;

            card.localRotation = Quaternion.Euler(0, 0, angleTwist);

            card.localPosition = new Vector3(xPos, yPos, 0);

            //if(i == total-1)
            //{
            //    Invoke("ShowCardsFront", 1);
            //}
        }
    }
    Card[] cardsInHands;
    public IEnumerator ShowPlayerCards()
    {
        cardsInHands = GetComponentsInChildren<Card>();

        foreach(Card card in cardsInHands)
        {
            SoundManager.Instance.PlaySoundEffect(Sound.Click);
            card.SwichSideAnimated();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ActiveMainPlayerCards()
    {
        foreach (Card card in cardsInHands)
        {
            card.ActiveButton();
            card.ActiveDragable();
        }
    }

    private float CalcHandWidth(int quantityOfCards)
    {
        var widthCards = quantityOfCards * (CardWidth - 30);
        var widthSpacing = (quantityOfCards) * (CardWidth / 2);
        return widthCards + widthSpacing;
    }

    private float CalcFirstAngle(float fullAngle)
    {
        var magicMathFactor = 0.1f;
        return -(fullAngle / 2) + fullAngle * magicMathFactor;
    }
}
