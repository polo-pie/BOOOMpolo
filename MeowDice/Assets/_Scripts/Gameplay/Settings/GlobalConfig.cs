using System;
using UnityEngine;

namespace MeowDice.GamePlay.Settings
{
    [Serializable]
    public class GlobalConfig
    {
        [Header("San值上下限")]
        public Vector2 SanLimit;
        [Header("警戒度上下限")]
        public Vector2 AlterLimit;
        [Header("初始san值")]
        public int SanInitValue;
        [Header("初始警戒度")]
        public int AlterInitValue;
        [Header("每回合发牌数量")]
        public int DealCount;
    }
}