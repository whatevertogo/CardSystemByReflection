using System;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 卡牌运行时 - 卡牌的MonoBehaviour组件
    /// todo-卡牌一定要带这个组件
    /// </summary>
    public class CardRuntime : MonoBehaviour, ICard
    {
        [SerializeField] private CardData cardData;

        // UI引用
        [SerializeField] private TMPro.TextMeshProUGUI nameText;
        [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
        [SerializeField] private TMPro.TextMeshProUGUI costText;
        [SerializeField] private UnityEngine.UI.Image artworkImage;

        // 事件
        public event EventHandler<ICardTarget> OnCardPlayed;
        public event EventHandler OnCardDiscarded;

        // 缓存
        [ReadOnly]
        private string[] _effectTypes;
        [ReadOnly]
        private bool _effectsPreloaded = false;

        // ICard接口实现
        public string CardID => cardData != null ? cardData.cardID : string.Empty;
        public string Name => cardData != null ? cardData.cardName : "Unknown Card";

        private void Awake()
        {
            if (cardData != null)
            {
                UpdateVisuals();
                PreloadEffects();
            }
        }

        // 设置卡牌数据
        public void SetCardData(CardData data)
        {
            cardData = data;
            UpdateVisuals();
            PreloadEffects();
        }

        // 更新卡牌视觉效果
        private void UpdateVisuals()
        {
            if (cardData is null) return;

            if (nameText != null)
                nameText.text = cardData.cardName;

            if (descriptionText != null)
                descriptionText.text = cardData.description;

            if (costText != null)
                costText.text = cardData.cost.ToString();

            if (artworkImage != null && cardData.artwork != null)
                artworkImage.sprite = cardData.artwork;
        }

        /// <summary>
        /// 预加载效果
        /// </summary>
        private void PreloadEffects()
        {
            if (cardData is null || _effectsPreloaded) return;

            _effectTypes = cardData.GetEffectTypes();
            EffectFactory.PreloadEffects(_effectTypes);
            _effectsPreloaded = true;
        }

        /// <summary>
        /// 播放卡牌
        /// </summary>
        /// <param name="target"></param>
        public void OnPlay(ICardTarget target)
        {
            if (cardData is null || cardData.effects is null) return;

            // 确保效果已预加载
            if (!_effectsPreloaded)
            {
                PreloadEffects();
            }

            // 执行所有效果
            foreach (var effect in cardData.effects)
            {
                EffectFactory.ExecuteEffect(target, effect);
            }

            // 触发事件
            OnCardPlayed?.Invoke(this,target);
        }

        /// <summary>
        /// 丢弃卡牌
        /// </summary>
        public void OnDiscard()
        {
            OnCardDiscarded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 获取卡牌数据
        /// </summary>
        /// <returns></returns>
        public CardData GetCardData()
        {
            return cardData;
        }

        /// <summary>
        /// 显示详细信息（用于UI悬停等）
        /// </summary>
        public void ShowDetails()
        {
            // 实现卡牌详情显示逻辑
        }
    }
}