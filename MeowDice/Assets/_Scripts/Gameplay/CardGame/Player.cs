using System.Collections.Generic;
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

        public Player()
        {
            cards = new List<MeowDiceCard>();
            selectedCards = new List<MeowDiceCard>();
        }

        public bool CanSelectCard(int index)
        {
            if (index >= cards.Count)
            {
                Debug.Log($"[CardGame][Player][CanSelectCard] no this card: {index}.");
                return false;
            }

            var card = cards[index];
            if (card.cardData.DiceCost >= _remainDiceCount)
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
        }

        public void UnSelectCard(MeowDiceCard card)
        {
            if (selectedCards.Remove(card))
            {
                _remainDiceCount += card.cardData.DiceCost;
                // TODO: BroadCast Dice Refresh
            }
        }

        public void OnRoundEnd()
        {
            cards.Clear();
            selectedCards.Clear();
        }

        public MeowDiceCard GetCard(int index)
        {
            if (cards.Count <= index)
                return null;
            return cards[index];
        }
    }
}