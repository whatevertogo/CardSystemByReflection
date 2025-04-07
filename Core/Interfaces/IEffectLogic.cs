namespace CardSystem
{
    /// <summary>
    /// 效果逻辑接口
    /// </summary>
    public interface IEffectLogic
    {
        void Execute(ICardTarget target, EffectData data);
    }
}