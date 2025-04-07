using System.Collections.Generic;
using UnityEngine;
using CDTU.Utils;

namespace CardSystem
{
    /// <summary>
    /// 卡牌管理器 - 管理卡牌集合和创建实例
    /// </summary>
    public class CardSystemManager : SingletonDD<CardSystemManager>
    {
        [SerializeField] private CardCollection cardCollection;
        [SerializeField] private GameObject cardPrefab;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Initialize()
        {
            // 确保EffectRegistry已初始化
            EffectRegistry.Instance.Initialize();

            // 初始化卡牌集合
            if (cardCollection != null)
            {
                cardCollection.Initialize();

                // 预加载所有卡牌的效果类型
                PreloadAllCardEffects();
            }
            else
            {
                Debug.LogError("CardManager: 未设置卡牌集合!");
            }
        }

        /// <summary>
        /// 预加载所有卡牌效果
        /// </summary>
        private void PreloadAllCardEffects()
        {
            HashSet<string> allEffectTypes = new HashSet<string>();

            foreach (var card in cardCollection.cards)
            {
                string[] effectTypes = card.GetEffectTypes();
                foreach (var type in effectTypes)
                {
                    allEffectTypes.Add(type);
                }
            }

            string[] uniqueEffectTypes = new string[allEffectTypes.Count];
            allEffectTypes.CopyTo(uniqueEffectTypes);
            EffectRegistry.Instance.PreloadEffects(uniqueEffectTypes);
        }

        /// <summary>
        /// 创建卡牌实例
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject CreateCardInstance(string cardId, Vector3 position = default, Transform parent = null)
        {
            if (cardPrefab is null)
            {
                Debug.LogError("CardManager: 未设置卡牌预制体!");
                return null;
            }

            var cardData = cardCollection.GetCardById(cardId);
            if (cardData is null)
            {
                Debug.LogError($"CardManager: 找不到ID为 {cardId} 的卡牌!");
                return null;
            }

            GameObject cardObject = Instantiate(cardPrefab, position, Quaternion.identity, parent);
            var cardRuntime = cardObject.GetComponent<CardRuntime>();
            if (cardRuntime != null)
            {
                cardRuntime.SetCardData(cardData);
            }
            else
            {
                Debug.LogError("CardManager: 卡牌预制体缺少CardRuntime组件!");
            }

            return cardObject;
        }

        /// <summary>
        /// 创建随机卡牌
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject CreateRandomCard(CardType cardType, Vector3 position = default, Transform parent = null)
        {
            var cards = cardCollection.GetCardsByType(cardType);
            if (cards.Count is 0)
            {
                Debug.LogWarning($"CardManager: 没有类型为 {cardType} 的卡牌!");
                return null;
            }

            int randomIndex = Random.Range(0, cards.Count);
            return CreateCardInstance(cards[randomIndex].cardID, position, parent);
        }

        /// <summary>
        /// 获取卡牌数据
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public CardData GetCardData(string cardId)
        {
            return cardCollection.GetCardById(cardId);
        }
    }
}