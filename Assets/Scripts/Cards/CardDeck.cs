using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardDeckDB cardsDB;
    public GameObject cardPrefab;
    public Transform deckTransform;

    private List<Card> cards;

    int totalCardsInDeck;

    void Start()
    {
    //   CreateInitialDeck();
    }

    public void CreateInitialDeck()
    {
        Vector2 position = deckTransform.position;
        List<CardData> cardsList = new List<CardData>(cardsDB.list);
        
        cardsList = ShuffleCards(cardsList);
        cards = new List<Card>();


        totalCardsInDeck = cardsList.Count;

        for (int i=0;i< totalCardsInDeck; i++)
        {
            CardData cardData = cardsList[i];
            GameObject cardObj = Instantiate(cardPrefab, this.transform);
            Card card = cardObj.GetComponent<Card>();
            card.SetData(cardData);
            cardObj.transform.SetSiblingIndex(0);
            cardObj.name = cardData.name;
            cardObj.transform.eulerAngles = new Vector3(0, 0, 90);
            cardObj.transform.position = position;
            position.y -= 1f;

            cards.Add(card);
        }
    }

    public List<T> ShuffleCards<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int j = Random.Range(i, count);
            T temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
        
        return list;
    }

    public void DistributeCards()
    {
        StartCoroutine(DistributeCardsWithDelay());
    }


    public IEnumerator DistributeCardsWithDelay()
    {

        //   int playerCount = playerPositions.Length;
        int cardsPerPlayer = 13;
        int cardNumber = 0;

        int currentPlayerIndex = 0;

        // Iterate through the deck and distribute the cards to players
        for (int i = 0; i < cardsPerPlayer; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // Get the next card from the deck

                // Set the target position for the card to move to (player's position)
                //   card.targetPosition = playerPositions[j].position;
                SoundManager.Instance.PlaySoundEffect(Sound.Hint);
                Card card = GetNextCard(cardNumber);
                cardNumber++;

                //Transform playerPosition = playerPositions[currentPlayerIndex];
                //Vector3 targetPosition = playerPosition.position;

                //card.targetPosition = targetPosition;

                Player player =  PlayerManager.instance.GetPlayer(j);
            //    bool isOwn = player.isOwn;

                player.AddCardToHand(card);
                card.SetOwner(player);
             //   if (isOwn)
             //       card.SwitchSide();

                // Move the card to the target position using Lean Tween
                card.MoveToPlayerPosition(player.tablePosition);
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;

                yield return new WaitForSeconds(0.1f);

            }
        }

        GameplayManager.instance.Invoke("StartBid", 1);
    }



    private Card GetNextCard(int index)
    {
        return cards[index];
    }
}
