using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay.UI;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class CardGameEntry : MonoBehaviour
    {
        public List<CatPosition> catPositions;

        private MeowDiceCardGameWindow _window;
        private MeowDiceCatInfoWindow _catInfoWindow;
        private CurtainWindow _curtainWindow;
        private SelectCardWindow _selectCardWindow;
        
        private void Awake()
        {
            MeowDiceCardGame.Instance.GameInit();
            _window = UIModule.Instance.ShowUI<MeowDiceCardGameWindow>(new Dictionary<string, object>());
        }

        private void Start()
        {
            _window.StartGame();
            _catInfoWindow = UIModule.Instance.ShowUI<MeowDiceCatInfoWindow>(new Dictionary<string, object>());
            // _catInfoWindow.SetVisible(false);
            _curtainWindow = UIModule.Instance.ShowUI<CurtainWindow>(new Dictionary<string, object>());
            _curtainWindow.SetVisible(false);
            _selectCardWindow = UIModule.Instance.ShowUI<SelectCardWindow>(new Dictionary<string, object>());
            _selectCardWindow.SetVisible(false);

        }
    }
}

