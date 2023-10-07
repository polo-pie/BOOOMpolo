using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Engine;
using Engine.Runtime;
using Engine.SettingModule;
using Engine.UI;
using MeowDice.GamePlay.UI;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class CardGameEntry : MonoBehaviour
    {
        public List<CatPosition> catPositions;

        private Dictionary<int, GameObject> cats;

        private MeowDiceCardGameWindow _window;
        private MeowDiceCatInfoWindow _catInfoWindow;
        private CurtainWindow _curtainWindow;
        private SelectCardWindow _selectCardWindow;
        
        private void Awake()
        {
            MeowDiceCardGame.Instance.GameInit();
            _window = UIModule.Instance.ShowUI<MeowDiceCardGameWindow>(new Dictionary<string, object>());

            cats = new Dictionary<int, GameObject>();
            foreach (var catPosition in catPositions)
            {
                cats[catPosition.index] = catPosition.transform.gameObject;
                catPosition.transform.gameObject.SetActive(false);
            }

            GameEvent.AddEventListener(EventKey.OnNextRound, OnRoundEnd);
            GameEvent.AddEventListener<uint>(EventKey.DoCatAct, DoCatAct);
        }

        private void OnDestroy()
        {
            GameEvent.RemoveEventListener(EventKey.OnNextRound, OnRoundEnd);
            GameEvent.RemoveEventListener<uint>(EventKey.DoCatAct, DoCatAct);
            _window.Close();
            _catInfoWindow.Close();
            _curtainWindow.Close();
            _selectCardWindow.Close();
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

        public void DoCatAct(uint cardId)
        {
            var table = TableModule.Get("Card");
            var acts = table.GetData(cardId, "CatPerformance") as List<int>;
            foreach (var act in acts)
            {
                if (cats.ContainsKey(act))
                {
                    cats[act].SetActive(true);
                }
            }
        }

        public void OnRoundEnd()
        {
            foreach (var catPosition in catPositions)
            {
                catPosition.transform.gameObject.SetActive(false);
            }
        }
    }
}

