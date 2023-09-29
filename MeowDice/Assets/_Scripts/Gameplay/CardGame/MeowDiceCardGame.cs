using Engine.Runtime;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class MeowDiceCardGame: TSingleton<MeowDiceCardGame>
    {

        private Cat _cat;
        private Player _player;

        public Cat Cat => _cat;
        public Player Player => _player;

        public RoundStage RoundStage => _roundStage;
        private RoundStage _roundStage;
        private MeowDiceCardManager _cardManager;

        public void GameInit()
        {
            _cat = new Cat();
            _player = new Player();
            _roundStage = RoundStage.CardUse;
            _cardManager = new MeowDiceCardManager();
            
            _cardManager.OnGameInit();
        }

        public void RoundEnd()
        {
            
        }

        public bool IsGameEnd()
        {
            return false;
        }

        public void EnterDiceStage()
        {
            _cardManager.OnRoundStart();
        }


        public void Destroy()
        {
            Release();
        }
        
    }
}