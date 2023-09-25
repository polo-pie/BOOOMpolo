using Engine.Runtime;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class MeowDiceCardGame: TSingleton<MeowDiceCardGame>
    {

        public Cat cat;

        public RoundStage RoundStage => _roundStage;
        private RoundStage _roundStage;

        public void GameInit()
        {
            cat = new Cat();
            _roundStage = RoundStage.CardUse;
        }


        public void Destroy()
        {
            Release();
        }
        
    }
}