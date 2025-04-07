using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 卡牌集合
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Card Collection", menuName = "Card System/Card Collection")]
    public class CardCollection : ScriptableObject
    {
        public List<CardData> cards = new List<CardData>();

        // 按类型索引的卡牌字典
        [NonSerialized] private Dictionary<CardType, List<CardData>> _cardsByType;

        // 按ID索引的卡牌字典
        [NonSerialized] private Dictionary<string, CardData> _cardsById;

        // 初始化索引
        public void Initialize()
        {
            _cardsByType = new Dictionary<CardType, List<CardData>>();
            _cardsById = new Dictionary<string, CardData>();

            foreach (var card in cards)
            {
                // 按类型索引
                if (!_cardsByType.ContainsKey(card.cardType))
                {
                    _cardsByType[card.cardType] = new List<CardData>();
                }
                _cardsByType[card.cardType].Add(card);

                // 按ID索引
                _cardsById[card.cardID] = card;
            }
        }

        // 获取指定类型的所有卡牌
        public List<CardData> GetCardsByType(CardType type)
        {
            if (_cardsByType is null) Initialize();

            if (_cardsByType.TryGetValue(type, out var result))
                return result;

            return new List<CardData>();
        }

        // 通过ID获取卡牌
        public CardData GetCardById(string id)
        {
            if (_cardsById is null) Initialize();

            if (_cardsById.TryGetValue(id, out var card))
                return card;

            return null;
        }
    }
}