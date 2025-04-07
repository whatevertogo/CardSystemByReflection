using UnityEngine;
using System;
using System.Collections.Generic;

namespace CardSystem
{
    /// <summary>
    /// 效果数据 - 可序列化
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "New Effect", menuName = "Card System/Effect Data")]
    public class EffectData : ScriptableObject
    {
        public string effectName;           // 效果名称
        public string effectTypeID;         // 效果类型ID，对应到具体效果实现
        public List<EffectParameter> parameters = new List<EffectParameter>();

        // 根据名称获取参数
        private EffectParameter GetParam(string name)
        {
            return parameters.Find(p => p.paramName == name);
        }

        // 获取不同类型参数的便捷方法
        public string GetString(string name, string defaultValue = "")
        {
            var param = GetParam(name);
            return param != null && param.paramType is EffectParameter.ParameterType.String ? param.stringValue : defaultValue;
        }

        public int GetInt(string name, int defaultValue = 0)
        {
            var param = GetParam(name);
            return param != null && param.paramType is EffectParameter.ParameterType.Integer ? param.intValue : defaultValue;
        }

        public float GetFloat(string name, float defaultValue = 0f)
        {
            var param = GetParam(name);
            return param != null && param.paramType is EffectParameter.ParameterType.Float ? param.floatValue : defaultValue;
        }

        public bool GetBool(string name, bool defaultValue = false)
        {
            var param = GetParam(name);
            return param != null && param.paramType is EffectParameter.ParameterType.Boolean ? param.boolValue : defaultValue;
        }

        public Vector2 GetVector2(string name, Vector2 defaultValue = default)
        {
            var param = GetParam(name);
            return param != null && param.paramType is EffectParameter.ParameterType.Vector2 ? param.vector2Value : defaultValue;
        }
    }
}