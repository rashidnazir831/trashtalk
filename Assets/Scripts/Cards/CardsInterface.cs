using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    void MoveToPlayerPosition(int index,bool isOwn, bool makeParent);
    void MoveCard(Transform transform, bool makeParent, System.Action onComplete);
}

//public interface ICardFactory
//{
//    ICard CreateCard(GameObject cardPrefab, Transform parent);
//}

//public interface ICardDeckData
//{
//    List<CardData> GetCardList();
//}

//public interface IInitialDeckCreator
//{
//    void CreateInitialDeck(GameObject gameObject,GameObject cardPrefab);
//}
