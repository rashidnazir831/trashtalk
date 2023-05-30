using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public Transform[] playerCardsPositions;
    public Transform[] showCardPositions;
    public GameObject cardAreaBorder;
    private RectTransform cardAreaRect;


    public static TableController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardAreaRect = cardAreaBorder.GetComponent<RectTransform>();
    }

    public Transform GetPlayerCardsTransform(int playerNumber)
    {
        return playerCardsPositions[playerNumber-1];
    }

    public Transform GetPlayerShowCardTransform(int playerNumber)
    {
        return showCardPositions[playerNumber - 1];
    }

    public bool IsCardPlaceable(Vector3 pos, Camera cam)
    {
    //    RectTransformUtility.RectangleContainsScreenPoint(cardAreaRect, pos, cam);
        return RectTransformUtility.RectangleContainsScreenPoint(cardAreaRect, pos, cam);


        //      // Check if the card is dropped on the table area
        //if (RectTransformUtility.RectangleContainsScreenPoint(TableRectTransform, eventData.position, eventData.pressEventCamera))
        //{
        //    // Card is dropped on the table, do something
        //    Debug.Log("Card dropped on the table!");
        //}
        //else
        //{
        //    // Card is dropped elsewhere, return it to the initial position
        //    transform.position = initialPosition;
        //}

    }

    //public Transform GetPlayerWinCardTransform()
    //{

    //}

    public void ShowTableCardArea(bool show)
    {
        cardAreaBorder.SetActive(show);
    }
}