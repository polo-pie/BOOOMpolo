using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public enum CardType
    {
        
    }
    
    /// <summary>
    /// 卡牌数据
    /// </summary>
    public struct MeowDiceCardData
    {
        public string name;
        public CardType cardType;
        public int DiceCost;
    }

    public class MeowDiceCard
    {
        public MeowDiceCardData cardData;
        public uint cardUid;
    }
}

