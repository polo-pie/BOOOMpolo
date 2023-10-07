using System.Collections.Generic;
using System.Linq;
using Engine.Runtime;
using Engine.SettingModule;
using MeowDice.GamePlay.Settings;
using Unity.Mathematics;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace MeowDice.GamePlay
{
    public class CardEffectTime
    {
        public const int CardUse = 1;
        public const int CardSelect = 2;
        public const int CardInDice = 3;
    }
    
    public class MeowDiceCardManager
    {
        public List<MeowDiceCard> PlayerCards => MeowDiceCardGame.Instance.Player.cards;
        public readonly Queue<MeowDiceCard> cards;
        public readonly List<MeowDiceCard> droppedCards;

        public const int DiceCount = 6;
        private int _lastRandomSelectCardIndex;

        public int RandomSelectCardIndex => _lastRandomSelectCardIndex;

        private MeowDiceCardGame _game;
        private uint _cardUid;

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
            SoundModule.Instance.PlayAudio(15);
            // 卡牌数小于每回合发牌数
            if (cards.Count < SettingModule.Instance.GlobalConfig.DealCount)
            {
                for (int i = droppedCards.Count - 1; i >= 0; i--)
                {
                    var index = Random.Range(0, i + 1);
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
            _cardUid = 0;

            foreach (var cardId in table.csvData.Keys)
            {
                var count = (int)table.GetData((uint)cardId, "Count");
                for (int i = 0; i < count; i++)
                {
                    var card = new MeowDiceCard((uint)cardId, _cardUid++);
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
                card.DoEffect(CardEffectTime.CardUse ,out var alterChange, out var sanChange, out var comfortChange);
                _game.Cat.ComfortChange(comfortChange);
                _game.Cat.AlterChange(alterChange);
                _game.Cat.SanChange(sanChange);
                GameEvent.Send(EventKey.OnStartAct, card.cardId, alterChange, sanChange);
                GameEvent.Send(EventKey.DoCatAct, card.cardId);
            }
        }

        public uint[] RandomCardsToSelect()
        {
            var table = TableModule.Get("Card");
            var cardIds = table.csvData.Keys.ToArray();
            for (int i = cardIds.Length - 1; i > 0; i--)
            {
                var pos = Random.Range(0, i - 1);
                (cardIds[pos], cardIds[i]) = (cardIds[i], cardIds[pos]);
            }
            
            var result = new uint[3];
            for (int i = 0; i < 3; i++)
                result[i] = (uint)cardIds[i];
            return result;
        }

        public void AddCard(uint cardId)
        {
            var card = new MeowDiceCard((uint)cardId, _cardUid++);
            var pos = Random.Range(0, cards.Count);
            for (int i = 0; i < cards.Count; i++)
            {
                if (pos == i)
                {
                    cards.Enqueue(card);
                }

                var tmp = cards.Dequeue();
                cards.Enqueue(tmp);
            }
        }
    }
}