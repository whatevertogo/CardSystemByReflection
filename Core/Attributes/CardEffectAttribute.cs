using System;

namespace CardSystem
{
    /// <summary>
    /// 标记效果实现类，用于自动注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CardEffectAttribute : Attribute
    {
        public string EffectTypeId { get; }
        
        public CardEffectAttribute(string effectTypeId)
        {
            EffectTypeId = effectTypeId;
        }
    }
}