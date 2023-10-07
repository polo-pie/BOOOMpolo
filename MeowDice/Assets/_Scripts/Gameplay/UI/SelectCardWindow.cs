using System.Collections.Generic;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class SelectCardWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/SelectCardWindow";

        private Button _btnConfirm;
        private Button _btnSkip;

        private SelectCardWindowItem _card1;
        private SelectCardWindowItem _card2;
        private SelectCardWindowItem _card3;

        private Dictionary<string, object> _card1Data = new Dictionary<string, object>();
        private Dictionary<string, object> _card2Data = new Dictionary<string, object>();
        private Dictionary<string, object> _card3Data = new Dictionary<string, object>();

        private int _selectCardId = 0;

        protected override void OnCreate()
        {
            var card1Go = Go.transform.Find("Cards/Card1").gameObject;
            var card2Go = Go.transform.Find("Cards/Card2").gameObject;
            var card3Go = Go.transform.Find("Cards/Card3").gameObject;

            _card1 = AddUIElement<SelectCardWindowItem>(card1Go);
            _card2 = AddUIElement<SelectCardWindowItem>(card2Go);
            _card3 = AddUIElement<SelectCardWindowItem>(card3Go);

            _btnConfirm = Go.transform.Find("BtnConfirm").GetComponent<Button>();
            _btnConfirm.interactable = false;
            _btnSkip = Go.transform.Find("BtnSkip").GetComponent<Button>();

            GameEvent.AddEventListener<uint>(EventKey.ChooseCard, OnChooseCard);
            GameEvent.AddEventListener(EventKey.OnEnterNextRound, OnEnterNextRound);

            _btnConfirm.onClick.AddListener(OnClickConfirm);
            _btnSkip.onClick.AddListener(OnClickSkip);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<uint>(EventKey.ChooseCard, OnChooseCard);
            GameEvent.RemoveEventListener(EventKey.OnEnterNextRound, OnEnterNextRound);

        }

        private void OnChooseCard(uint cardId)
        {
            _selectCardId = (int)cardId;
            _btnConfirm.interactable = true;
        }

        public void StartSelectCard()
        {
            var cardIds = MeowDiceCardGame.Instance.CardManager.RandomCardsToSelect();
            _card1Data["cardId"] = cardIds[0];
            _card2Data["cardId"] = cardIds[1];
            _card3Data["cardId"] = cardIds[2];
            
            _card1.UnSelect();
            _card1.RefreshUIElement(_card1Data);
            _card2.UnSelect();
            _card2.RefreshUIElement(_card2Data);
            _card3.UnSelect();
            _card3.RefreshUIElement(_card3Data);
            _selectCardId = -1;
        }

        public void OnEnterNextRound()
        {
            SetVisible(true);
            StartSelectCard();
        }

        private void OnClickConfirm()
        {
            if (_selectCardId == -1)
            {
                Debug.LogError("[Game][SelectCardWindow][OnClickConfirm] selectCardId is 0");
                return;
            }

            MeowDiceCardGame.Instance.CardManager.AddCard((uint)_selectCardId);
            SetVisible(false);
            var window = UIModule.Instance.GetWindow<CurtainWindow>();
            window.OnEnterNextRound();
        }

        private void OnClickSkip()
        {
            var window = UIModule.Instance.GetWindow<CurtainWindow>();
            window.OnEnterNextRound();
            SetVisible(false);
        }
    }
}