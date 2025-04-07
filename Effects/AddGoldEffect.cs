using UnityEngine;

namespace CardSystem.Effects
{
    [CardEffect("AddGold")]
    public class AddGoldEffect : IEffectLogic
    {
        public void Execute(ICardTarget target, EffectData data)
        {
            int amount = data.GetInt("amount", 0);

            // 检查目标是否为文明对象
            if (target is Civilization civilization)
            {
                //civilization.gold += amount;
               // Debug.Log($"增加 {amount} 金币，当前: {civilization.gold}");
            }
            else
            {
                Debug.LogWarning("AddGoldEffect: 目标不是文明对象");
            }
        }
    }
}