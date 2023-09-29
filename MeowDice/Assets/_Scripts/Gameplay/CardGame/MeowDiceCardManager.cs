using System.Collections.Generic;
using MeowDice.GamePlay.Settings;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace MeowDice.GamePlay
{
    public class MeowDiceCardManager
    {
        public List<MeowDiceCard> PlayerCards => MeowDiceCardGame.Instance.Player.cards;
        public readonly Queue<MeowDiceCard> cards;
        public readonly List<MeowDiceCard> droppedCards;

        public const int DiceCount = 6;

        public MeowDiceCardManager()
        {
            cards = new Queue<MeowDiceCard>();
            droppedCards = new List<MeowDiceCard>();
        }

        /// <summary>
        /// 回合开始
        /// </summary>
        internal void OnRoundStart()
        {
            // 卡牌数小于每回合发牌数
            if (cards.Count < SettingModule.Instance.GlobalConfig.DealCount)
            {
                for (int i = droppedCards.Count - 1; i > 0; i++)
                {
                    var index = Random.Range(0, i);
                    cards.Enqueue(droppedCards[index]);
                    droppedCards[index] = droppedCards[i];
                }
                droppedCards.Clear();
            }

            for (int i = 0; i < SettingModule.Instance.GlobalConfig.DealCount; i++)
            {
                var card = cards.Dequeue();
                MeowDiceCardGame.Instance.Player.cards[i] = card;
            }
        }

        internal void OnGameInit()
        {
            
        }
        
    }
}