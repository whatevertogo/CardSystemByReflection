using UnityEngine;
using System.Collections.Generic;

namespace CardSystem
{

    /// <summary>
    /// 所有卡牌必须实现的接口
    /// </summary>
    public interface ICard
    {
        string CardID { get; }
        string Name { get; }
        void OnPlay(ICardTarget target);
        void OnDiscard();
        CardData GetCardData();
    }



}