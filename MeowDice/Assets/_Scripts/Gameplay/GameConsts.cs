using System;
using Engine.Runtime;

namespace MeowDice.GamePlay
{
    /// <summary>
    /// 回合阶段
    /// </summary>
    public enum RoundStage
    {
        CardUse,
        Dice,
        Act,
    }

    public class EventKey
    {
        public static readonly int SelectCard = StringId.StringToHash("SelectCard");
        public static readonly int UnselectCard = StringId.StringToHash("UnselectCard");
        public static readonly int EnterDiceStage = StringId.StringToHash("EnterDiceStage");
        public static readonly int OnStartAct = StringId.StringToHash("StartAct");
    }
    
}