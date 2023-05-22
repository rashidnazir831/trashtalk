using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPositions : MonoBehaviour
{
    public Transform[] playerCardsPositions;
    public Transform[] showCardPositions;

    public static CardsPositions instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Transform GetPlayerCardsTransform(int playerNumber)
    {
        return playerCardsPositions[playerNumber-1];
    }

    public Transform GetPlayerShowCardTransform(int playerNumber)
    {
        return showCardPositions[playerNumber - 1];
    }
}
