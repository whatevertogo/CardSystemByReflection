using System;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// 效果参数 - 支持多种类型
    /// 如有需要请自行添加更多类型
    /// </summary>
    [Serializable]
    public class EffectParameter
    {
        public string paramName;
        public ParameterType paramType;
        public string stringValue;
        public int intValue;
        public float floatValue;
        public bool boolValue;
        public Vector2 vector2Value;

        public enum ParameterType
        {
            String,
            Integer,
            Float,
            Boolean,
            Vector2
        }

        // 获取参数值的便捷方法
        public T GetValue<T>()
        {
            if (typeof(T) == typeof(string) && paramType is ParameterType.String)
                return (T)(object)stringValue;
            else if (typeof(T) == typeof(int) && paramType is ParameterType.Integer)
                return (T)(object)intValue;
            else if (typeof(T) == typeof(float) && paramType is ParameterType.Float)
                return (T)(object)floatValue;
            else if (typeof(T) == typeof(bool) && paramType is ParameterType.Boolean)
                return (T)(object)boolValue;
            else if (typeof(T) == typeof(Vector2) && paramType is ParameterType.Vector2)
                return (T)(object)vector2Value;

            Debug.LogError($"参数类型不匹配: {paramName}，请求类型: {typeof(T)}，实际类型: {paramType}");
            return default;
        }
    }
}