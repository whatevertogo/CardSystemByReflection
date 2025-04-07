
namespace CardSystem
{
    /// <summary>
    /// 卡牌目标接口
    /// </summary>
    public interface ICardTarget
    {
        void ApplyEffect(EffectData effect);
    }
}