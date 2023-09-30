using System.Collections.Generic;
using Engine.Runtime;
using Engine.SettingModule;
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
        private int _lastRandomSelectCardIndex;

        public int RandomSelectCardIndex => _lastRandomSelectCardIndex;

        private MeowDiceCardGame _game;

        public MeowDiceCardManager(MeowDiceCardGame game)
        {
            _game = game;
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
                for (int i = droppedCards.Count - 1; i > 0; i--)
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
                MeowDiceCardGame.Instance.Player.cards.Add(card);
            }
        }

        internal void OnGameInit()
        {
            var table = TableModule.Get("InitializeDeck");
            droppedCards.Clear();
            uint cardUid = 0;

            foreach (var cardId in table.csvData.Keys)
            {
                var count = (int)table.GetData((uint)cardId, "Count");
                for (int i = 0; i < count; i++)
                {
                    var card = new MeowDiceCard((uint)cardId, cardUid++);
                    droppedCards.Add(card);
                }
            }

        }

        public int RandomCardFromSelectCards()
        {
            if (MeowDiceCardGame.Instance.RoundStage == RoundStage.Dice)
            {
                _lastRandomSelectCardIndex = Random.Range(0, 6);
                return _lastRandomSelectCardIndex;
            }

            return -1;
        }

        public void DropCards()
        {
            foreach (var card in PlayerCards)
            {
                droppedCards.Add(card);
            }
        }

        public void DoCardEffect()
        {
            var card = _game.Player.selectedCards[RandomSelectCardIndex];
            if (card == null)
            {
                GameEvent.Send(EventKey.OnStartAct, (uint)2,0, 0);
            }
            else
            {
                card.DoEffect(out var alterChange, out var sanChange);
                _game.Cat.AlterChange(alterChange);
                _game.Cat.SanChange(sanChange);
                GameEvent.Send(EventKey.OnStartAct, card.cardId, alterChange, sanChange);
            }
        }
    }
}