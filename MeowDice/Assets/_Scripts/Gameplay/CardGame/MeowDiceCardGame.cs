using Engine.Runtime;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class MeowDiceCardGame: TSingleton<MeowDiceCardGame>
    {

        private Cat _cat;
        private Player _player;

        public Cat Cat => _cat;
        public Player Player => _player;
        public MeowDiceCardManager CardManager => _cardManager;

        public RoundStage RoundStage => _roundStage;
        private RoundStage _roundStage;
        private MeowDiceCardManager _cardManager;

        public void GameInit()
        {
            _cat = new Cat();
            _player = new Player();
            _roundStage = RoundStage.CardUse;
            _cardManager = new MeowDiceCardManager(this);
            
            _cardManager.OnGameInit();
        }

        public void RoundEnd()
        {
            _player.OnRoundEnd();
        }

        public bool IsGameEnd()
        {
            return false;
        }

        public void EnterCardUseStage()
        {
            _roundStage = RoundStage.CardUse;
            _cardManager.OnRoundStart();
        }

        public void EnterDiceStage()
        {
            if (_roundStage != RoundStage.CardUse)
            {
                Debug.LogError($"[MeowDiceCardGame][EnterDiceStage] enter stage fail, current stage {_roundStage.ToString()} is not CardUse");
                return;
            }

            _roundStage = RoundStage.Dice;
            GameEvent.Send(EventKey.EnterDiceStage);
        }

        public void EnterActStage()
        {
            if (_roundStage != RoundStage.Dice)
            {
                Debug.LogError($"[MeowDiceCardGame][EnterActStage] enter stage fail, current stage {_roundStage.ToString()} is not CardDice");
                return;
            }

            _roundStage = RoundStage.CardUse;

            _cardManager.DoCardEffect();

        }


        public void Destroy()
        {
            Release();
        }
        
    }
}