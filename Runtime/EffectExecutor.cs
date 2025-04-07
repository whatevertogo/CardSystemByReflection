using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 执行效果的对象一定要带这个组件-效果执行器 - 可附加到任何对象，提供执行效果的功能
    /// </summary>
    public class EffectExecutor : MonoBehaviour
    {
        [SerializeField] private EffectData[] effects;

        private bool _effectsPreloaded = false;

        private void Start()
        {
            if (effects is not null && effects.Length > 0)
            {
                PreloadEffects();
            }
        }

        // 预加载所有效果
        private void PreloadEffects()
        {
            if (_effectsPreloaded) return;

            HashSet<string> effectTypes = new HashSet<string>();
            foreach (var effect in effects)
            {
                if (effect is not null && !string.IsNullOrEmpty(effect.effectTypeID))
                {
                    effectTypes.Add(effect.effectTypeID);
                }
            }

            string[] uniqueEffectTypes = new string[effectTypes.Count];
            effectTypes.CopyTo(uniqueEffectTypes);

            EffectFactory.PreloadEffects(uniqueEffectTypes);
            _effectsPreloaded = true;
        }

        // 执行指定效果
        public void ExecuteEffect(int effectIndex, ICardTarget target)
        {
            if (effects is null || effectIndex < 0 || effectIndex >= effects.Length)
            {
                Debug.LogError($"无效的效果索引: {effectIndex}");
                return;
            }

            if (!_effectsPreloaded)
            {
                PreloadEffects();
            }

            EffectFactory.ExecuteEffect(target, effects[effectIndex]);
        }

        // 执行所有效果
        public void ExecuteAllEffects(ICardTarget target)
        {
            if (effects is null || effects.Length == 0) return;

            if (!_effectsPreloaded)
            {
                PreloadEffects();
            }

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] is not null)
                {
                    EffectFactory.ExecuteEffect(target, effects[i]);
                }
            }
        }
    }
}