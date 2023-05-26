using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour,ICard
{
    public CardData data;
    public GameObject front;
    public GameObject back;

    Transform parent;

    public enum Suit
    {
        Spades,
        Hearts,
        Diamonds,
        Clubs
    }

    private void Awake()
    {
        parent = transform.parent;
    }

    public  void SetData(CardData data)
    {
        this.data = data;
    }

    public void SwitchSide()
    {
        //if(back.activeSelf)

        bool isFrontActive = front.activeSelf;

        front.SetActive(!isFrontActive);
        back.SetActive(isFrontActive);
    }

    public void SetInitialParent()
    {
        transform.SetParent(parent);
    }

    public void MoveCard(Transform trans, bool makeParent=true, System.Action onComplete=null)
    {
        LeanTween.scale(gameObject, Vector3.one, 0.2f);

        LeanTween.move(gameObject, trans.position, 0.2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            if (makeParent)
                transform.SetParent(trans);

            onComplete?.Invoke();
        });
    }

    public void MoveToPlayerPosition(int playerIndex,bool isOwn, bool makeParent=true)
    {
        Transform currentPlayerDeckTransform = CardsPositions.instance.GetPlayerCardsTransform(playerIndex);

        LeanTween.rotate(gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(gameObject, Vector3.one * (isOwn?3.5f:1),0.2f);
        LeanTween.move(gameObject, currentPlayerDeckTransform.position, 0.2f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            if (makeParent)
            {
                transform.SetParent(currentPlayerDeckTransform);
                transform.localEulerAngles = Vector3.zero;
                HandCardsUI hand = transform.GetComponentInParent<HandCardsUI>();
                if (hand != null)
                {
                    hand.UpdateCardArrangement();
                }
            }

           
        });
    }
}
