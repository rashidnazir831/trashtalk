using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardsUI : MonoBehaviour
{
    public GameObject cardPrefab;
    public float radius = 50f;

  //  private List<GameObject> cardObjects = new List<GameObject>();


    // Call this method to update the card arrangement whenever a card is removed
    public void UpdateCardArrangement()
    {
        int cardCount = transform.childCount;

        if (cardCount == 0)
            return;

        // Calculate the angle between each card based on the card count
        float angleStep = 360f / cardCount;

        for (int i = 0; i < cardCount; i++)
        {
            // Calculate the position of each card along the circle
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            // Update the position and rotation of the card
            transform.GetChild(i).localPosition = new Vector3(x, y, 0f);
            transform.GetChild(i).transform.localRotation = Quaternion.Euler(0f, 0f, -angle * Mathf.Rad2Deg);
        }
    }
}
