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
        public static readonly int OnEnterNextRound = StringId.StringToHash("EnterNextRound");
        public static readonly int OnNextRound = StringId.StringToHash("OnNextRound");
        public static readonly int OnGameEnd = StringId.StringToHash("GameEnd");
        public static readonly int ChooseCard = StringId.StringToHash("ChooseCard");
        public static readonly int DoCatAct = StringId.StringToHash("DoCatAct");
        public static readonly int OnStartDialogue = StringId.StringToHash("StartDialogue");
    }
    
}