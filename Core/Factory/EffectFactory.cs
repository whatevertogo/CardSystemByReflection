using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 效果工厂 - 负责创建和执行效果
    /// </summary>
    public static class EffectFactory
    {
        // 执行单个效果
        public static void ExecuteEffect(ICardTarget target, EffectData data)
        {
            if (data is null || string.IsNullOrEmpty(data.effectTypeID))
            {
                Debug.LogError("无效的效果数据");
                return;
            }

            var logic = EffectRegistry.Instance.GetEffect(data.effectTypeID);
            if (logic != null)
            {
                logic.Execute(target, data);
            }
        }

        // 批量执行效果
        public static void ExecuteEffects(ICardTarget target, EffectData[] effects)
        {
            if (effects is null || effects.Length is 0) return;

            for (int i = 0; i < effects.Length; i++)
            {
                ExecuteEffect(target, effects[i]);
            }
        }

        // 预加载一组效果类型
        public static void PreloadEffects(string[] effectTypes)
        {
            EffectRegistry.Instance.PreloadEffects(effectTypes);
        }
    }
}