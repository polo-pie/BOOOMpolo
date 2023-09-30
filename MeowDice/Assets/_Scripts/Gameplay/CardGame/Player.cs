using System.Collections.Generic;
using Engine.Runtime;
using MeowDice.GamePlay.Settings;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class Player
    {
        public readonly List<MeowDiceCard> cards;
        public readonly List<MeowDiceCard> selectedCards;

        public int RemainDiceCount => _remainDiceCount;
        private int _remainDiceCount;
        private Queue<MeowDiceCard> _tmpQueue;

        public Player()
        {
            cards = new List<MeowDiceCard>();
            selectedCards = new List<MeowDiceCard>();
            _tmpQueue = new Queue<MeowDiceCard>();
            _remainDiceCount = MeowDiceCardManager.DiceCount;

            for(int i = 0; i < MeowDiceCardManager.DiceCount; i++)
            {
                selectedCards.Add(null);
            }
        }

        public bool CanSelectCard(int index)
        {
            if (index >= cards.Count)
            {
                Debug.Log($"[CardGame][Player][CanSelectCard] no this card: {index}.");
                return false;
            }

            var card = cards[index];
            if (card.cardData.DiceCost > _remainDiceCount)
            {
                Debug.Log($"[CardGame][Player][CanSelectCard] remain dice not enough, need {card.cardData.DiceCost}, remain {_remainDiceCount}.");
                return false;
            }

            return true;
        }

        public void SelectCard(int index)
        {
            if (!CanSelectCard(index))
                return;

            selectedCards[MeowDiceCardManager.DiceCount - _remainDiceCount] = cards[index];
            _remainDiceCount -= cards[index].cardData.DiceCost;
            // TODO: BroadCast select card event
            GameEvent.Send(EventKey.SelectCard, cards[index]);
        }

        public void UnSelectCard(MeowDiceCard card)
        {
            if (selectedCards.Remove(card))
            {
                selectedCards.Add(null);
                for(int i = 0; i < MeowDiceCardManager.DiceCount; i++)
                {
                    var selectedCard = selectedCards[i];
                    if(selectedCard != null)
                    {
                        _tmpQueue.Enqueue(selectedCard);
                        selectedCards[i] = null;
                    }
                }

                _remainDiceCount = MeowDiceCardManager.DiceCount;
                while (_tmpQueue.Count > 0)
                {
                    var selectedCard = _tmpQueue.Dequeue();
                    selectedCards[MeowDiceCardManager.DiceCount - _remainDiceCount] = selectedCard;
                    _remainDiceCount -= selectedCard.cardData.DiceCost;
                }
                
                // TODO: BroadCast Dice Refresh
                GameEvent.Send(EventKey.UnselectCard, card);
            }
        }

        public void OnRoundEnd()
        {
            MeowDiceCardGame.Instance.CardManager.DropCards();
            cards.Clear();
            for (int i = 0; i < selectedCards.Count; i++)
            {
                selectedCards[i] = null;
            }

            _remainDiceCount = MeowDiceCardManager.DiceCount;
        }

        public MeowDiceCard GetCard(int index)
        {
            if (cards.Count <= index)
                return null;
            return cards[index];
        }
    }
}