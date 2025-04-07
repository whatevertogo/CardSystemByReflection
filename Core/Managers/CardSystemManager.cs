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

        private bool _isInitialized;
        public bool IsInitialized => _isInitialized;

        // 场景加载时重置状态
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance?.Reset();
        }

        private void Reset()
        {
            _isInitialized = false;
        }

        protected override void Awake()
        {
            base.Awake();
            ValidateRequiredComponents();
        }

        private void ValidateRequiredComponents()
        {
            if (cardCollection == null)
            {
                Debug.LogError("CardSystemManager: 未设置卡牌集合!");
                return;
            }

            if (cardPrefab == null)
            {
                Debug.LogError("CardSystemManager: 未设置卡牌预制体!");
                return;
            }

            // 验证预制体是否包含必要组件
            if (!cardPrefab.GetComponent<CardRuntime>())
            {
                Debug.LogError("CardSystemManager: 卡牌预制体缺少CardRuntime组件!");
            }
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("CardSystemManager: 已经初始化过了!");
                return;
            }

            if (!ValidateSetup()) return;

            try
            {
                // 确保EffectRegistry已初始化
                EffectRegistry.Instance.Initialize();

                // 初始化卡牌集合
                cardCollection.Initialize();

                // 预加载所有卡牌的效果类型
                PreloadAllCardEffects();

                _isInitialized = true;
                Debug.Log("CardSystemManager: 初始化完成");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"CardSystemManager: 初始化失败: {ex.Message}");
                _isInitialized = false;
            }
        }

        private bool ValidateSetup()
        {
            if (cardCollection == null)
            {
                Debug.LogError("CardSystemManager: 未设置卡牌集合!");
                return false;
            }

            if (cardPrefab == null)
            {
                Debug.LogError("CardSystemManager: 未设置卡牌预制体!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 预加载所有卡牌效果
        /// </summary>
        private void PreloadAllCardEffects()
        {
            if (cardCollection == null || cardCollection.cards == null) return;

            HashSet<string> allEffectTypes = new HashSet<string>();
            foreach (var card in cardCollection.cards)
            {
                if (card != null)
                {
                    string[] effectTypes = card.GetEffectTypes();
                    foreach (var type in effectTypes)
                    {
                        allEffectTypes.Add(type);
                    }
                }
            }

            if (allEffectTypes.Count > 0)
            {
                string[] uniqueEffectTypes = new string[allEffectTypes.Count];
                allEffectTypes.CopyTo(uniqueEffectTypes);
                EffectRegistry.Instance.PreloadEffects(uniqueEffectTypes);
            }
        }

        /// <summary>
        /// 创建卡牌实例
        /// </summary>
        public GameObject CreateCardInstance(string cardId, Vector3 position = default, Transform parent = null)
        {
            if (!_isInitialized)
            {
                Debug.LogError("CardSystemManager: 系统未初始化!");
                return null;
            }

            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogError("CardSystemManager: 卡牌ID不能为空!");
                return null;
            }

            var cardData = cardCollection.GetCardById(cardId);
            if (cardData == null)
            {
                Debug.LogError($"CardSystemManager: 找不到ID为 {cardId} 的卡牌!");
                return null;
            }

            try
            {
                GameObject cardObject = Instantiate(cardPrefab, position, Quaternion.identity, parent);
                var cardRuntime = cardObject.GetComponent<CardRuntime>();
                if (cardRuntime != null)
                {
                    cardRuntime.SetCardData(cardData);
                    return cardObject;
                }
                else
                {
                    Destroy(cardObject);
                    Debug.LogError("CardSystemManager: 卡牌预制体缺少CardRuntime组件!");
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"CardSystemManager: 创建卡牌实例时发生错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建随机卡牌
        /// </summary>
        public GameObject CreateRandomCard(CardType cardType, Vector3 position = default, Transform parent = null)
        {
            if (!_isInitialized)
            {
                Debug.LogError("CardSystemManager: 系统未初始化!");
                return null;
            }

            var cards = cardCollection.GetCardsByType(cardType);
            if (cards == null || cards.Count == 0)
            {
                Debug.LogWarning($"CardSystemManager: 没有类型为 {cardType} 的卡牌!");
                return null;
            }

            int randomIndex = Random.Range(0, cards.Count);
            return CreateCardInstance(cards[randomIndex].cardID, position, parent);
        }

        /// <summary>
        /// 获取卡牌数据
        /// </summary>
        public CardData GetCardData(string cardId)
        {
            if (!_isInitialized)
            {
                Debug.LogError("CardSystemManager: 系统未初始化!");
                return null;
            }

            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogError("CardSystemManager: 卡牌ID不能为空!");
                return null;
            }

            return cardCollection.GetCardById(cardId);
        }
    }
}