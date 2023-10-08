using System.Collections.Generic;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay.Settings;
using MeowDice.GamePlay.UI;
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

        private bool _gameEnd;

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
            _cat.OnRoundEnd();

            if (_cat.AlterValue >= SettingModule.Instance.GlobalConfig.AlterLimit.y)
            {
                _gameEnd = true;
                GameEvent.Send(EventKey.OnGameEnd, false);
            }
            else if (_cat.SanValue <= SettingModule.Instance.GlobalConfig.SanLimit.x)
            {
                _gameEnd = true;
                GameEvent.Send(EventKey.OnGameEnd, true);
            }
            else
            {
                GameEvent.Send(EventKey.OnEnterNextRound);
            }
            
        }

        public bool IsGameEnd()
        {
            return false;
        }

        public void EnterCardUseStage()
        {
            _roundStage = RoundStage.CardUse;
            _cat.OnRoundStart();
            _cardManager.OnRoundStart();
            UIModule.Instance.GetWindow<MeowDiceCatInfoWindow>()?.RefreshUIElement(new Dictionary<string, object>());
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