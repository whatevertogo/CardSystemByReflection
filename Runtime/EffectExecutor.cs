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
        private bool _isInitialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized) return;
            
            if (effects != null && effects.Length > 0)
            {
                PreloadEffects();
            }
            _isInitialized = true;
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

            if (effectTypes.Count > 0)
            {
                string[] uniqueEffectTypes = new string[effectTypes.Count];
                effectTypes.CopyTo(uniqueEffectTypes);
                EffectFactory.PreloadEffects(uniqueEffectTypes);
                _effectsPreloaded = true;
            }
        }

        // 执行指定效果
        public bool ExecuteEffect(int effectIndex, ICardTarget target)
        {
            if (target == null)
            {
                Debug.LogError("执行效果失败：目标为空");
                return false;
            }

            if (effects == null || effectIndex < 0 || effectIndex >= effects.Length)
            {
                Debug.LogError($"无效的效果索引: {effectIndex}");
                return false;
            }

            if (!_effectsPreloaded)
            {
                PreloadEffects();
            }

            try 
            {
                EffectFactory.ExecuteEffect(target, effects[effectIndex]);
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"执行效果时发生错误: {ex.Message}");
                return false;
            }
        }

        // 执行所有效果
        public bool ExecuteAllEffects(ICardTarget target)
        {
            if (target == null)
            {
                Debug.LogError("执行效果失败：目标为空");
                return false;
            }

            if (effects == null || effects.Length == 0)
            {
                Debug.LogWarning("没有可执行的效果");
                return false;
            }

            if (!_effectsPreloaded)
            {
                PreloadEffects();
            }

            bool allSuccess = true;
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] != null)
                {
                    try
                    {
                        EffectFactory.ExecuteEffect(target, effects[i]);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"执行效果 {i} 时发生错误: {ex.Message}");
                        allSuccess = false;
                    }
                }
            }
            return allSuccess;
        }

        // 获取效果数量
        public int EffectCount => effects?.Length ?? 0;

        // 检查效果索引是否有效
        public bool IsValidEffectIndex(int index)
        {
            return effects != null && index >= 0 && index < effects.Length;
        }
    }
}