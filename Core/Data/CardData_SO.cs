using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem{
    /// <summary>
    /// 卡牌数据 - 可序列化为JSON或ScriptableObject
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card Data")]
    public class CardData : ScriptableObject
    {
        public string cardID;             // 唯一标识
        public string cardName;           // 卡牌名称
        public string description;        // 卡牌描述
        public CardType cardType;         // 卡牌类型
        public CardRarity rarity;         // 稀有度
        public int cost;                  // 费用
        public Sprite artwork;            // 卡牌图像
        public List<EffectData> effects;  // 效果列表
        public List<string> tags;         // 标签列表（如"攻击"，"防御"）
        public bool isStackable;          // 是否可堆叠
        public int maxStackCount = 1;     // 最大堆叠数

        // 预缓存卡牌所有效果类型ID
        [NonSerialized] private string[] _cachedEffectTypes;

        public string[] GetEffectTypes()
        {
            if (_cachedEffectTypes is null)
            {
                HashSet<string> types = new HashSet<string>();
                if (effects != null)
                {
                    foreach (var effect in effects)
                    {
                        types.Add(effect.effectTypeID);
                    }
                }
                _cachedEffectTypes = new string[types.Count];
                types.CopyTo(_cachedEffectTypes);
            }
            return _cachedEffectTypes;
        }
    }
}