using System;
using System.Collections.Generic;
using System.Reflection;
using CDTU.Utils;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 效果注册表 - 管理所有效果逻辑实现
    /// </summary>
    public class EffectRegistry : SingletonDD<EffectRegistry>
    {
        [Header("效果注册表")]
        [SerializeField] private GameObject effectPrefab; // 效果预制体 - 可选

        /// <summary>
        /// 效果缓存 - 避免重复创建
        /// </summary>
        public Dictionary<string, IEffectLogic> EffectCache => _effectCache;

        // 效果缓存 - 避免重复创建
        private Dictionary<string, IEffectLogic> _effectCache = new Dictionary<string, IEffectLogic>();
        private bool _initialized = false;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 初始化所有效果
        /// </summary>
        public void Initialize()
        {
            if (_initialized) return;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            try
            {
                RegisterEffectsFromAssembly(Assembly.GetExecutingAssembly());

                // 如果需要从其他程序集加载，可以在这里添加
                // RegisterEffectsFromAssembly(Assembly.Load("MyOtherAssembly"));
            }
            catch (Exception ex)
            {
                Debug.LogError($"初始化效果注册表时出错: {ex.Message}");
            }

            stopwatch.Stop();
            Debug.Log($"效果注册表初始化完成，共注册 {_effectCache.Count} 个效果，耗时: {stopwatch.ElapsedMilliseconds}ms");
            _initialized = true;
        }

        /// <summary>
        /// 从程序集中注册所有标记了CardEffectAttribute的效果
        /// </summary>
        /// <param name="assembly"></param>
        private void RegisterEffectsFromAssembly(Assembly assembly)
        {
            var effectTypes = assembly.GetTypes();
            foreach (var type in effectTypes)
            {
                // 快速过滤不相关类型
                if (!typeof(IEffectLogic).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
                    continue;

                var attribute = type.GetCustomAttribute<CardEffectAttribute>();
                if (attribute != null)
                {
                    try
                    {
                        var instance = (IEffectLogic)Activator.CreateInstance(type);
                        RegisterEffect(attribute.EffectTypeId, instance);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"创建效果实例失败: {type.Name}, 错误: {ex.Message}");
                    }
                }
            }
        }

        // 手动注册效果
        public void RegisterEffect(string effectTypeId, IEffectLogic implementation)
        {
            _effectCache[effectTypeId] = implementation;
            Debug.Log($"注册效果: {effectTypeId}");
        }

        // 获取效果
        public IEffectLogic GetEffect(string effectTypeId)
        {
            if (!_initialized)
                Initialize();

            if (_effectCache.TryGetValue(effectTypeId, out var effect))
                return effect;

            Debug.LogWarning($"找不到效果类型: {effectTypeId}");
            return null;
        }

        // 预加载一组效果
        public void PreloadEffects(string[] effectTypeIds)
        {
            if (!_initialized)
                Initialize();

            if (effectTypeIds is null) return;

            foreach (var id in effectTypeIds)
            {
                if (!string.IsNullOrEmpty(id) && !_effectCache.ContainsKey(id))
                {
                    Debug.LogWarning($"未找到需要预加载的效果: {id}");
                }
            }
        }

    }
}