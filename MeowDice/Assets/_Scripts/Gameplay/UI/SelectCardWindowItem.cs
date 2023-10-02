using System.Collections.Generic;
using Engine.Runtime;
using Engine.SettingModule;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class SelectCardWindowItem: UIWidget
    {
        private GameObject _cardNameTextGo;
        private UIText _cardNameText;
        private GameObject _cardTypeTextGo;
        private UIText _cardTypeText;
        private GameObject _diceCostTextGo;
        private UIText _diceCostText;

        private MeowDiceCardButton _button;
        private bool _selected;
        
        protected uint CardId => (uint)inputData["cardId"];
        
        private Dictionary<string, object> _cardNameTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _cardTypeTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _diceCostTextData = new Dictionary<string, object>();
        
        protected override void OnCreate()
        {
            _cardNameTextGo = Go.transform.Find("CardName/Text").gameObject;
            _cardNameText = AddUIElement<UIText>(_cardNameTextGo);

            _cardTypeTextGo = Go.transform.Find("CardType/Text").gameObject;
            _cardTypeText = AddUIElement<UIText>(_cardTypeTextGo);

            _diceCostTextGo = Go.transform.Find("DiceCost/Text").gameObject;
            _diceCostText = AddUIElement<UIText>(_diceCostTextGo);

            _button = Go.GetComponent<MeowDiceCardButton>();
            _button.onClick.AddListener(OnClickCard);

            GameEvent.AddEventListener<uint>(EventKey.ChooseCard, OnSelectCard);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<uint>(EventKey.ChooseCard, OnSelectCard);
        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _cardTypeText.InitData(_cardTypeTextData);
            _diceCostText.InitData(_diceCostTextData);
        }

        protected override void OnRefreshData()
        {
            var table = TableModule.Get("Card");
            _cardNameTextData["text"] = table.GetData(CardId, "Name");
            _cardTypeTextData["text"] = table.GetData(CardId, "Effect");
            _diceCostTextData["text"] = table.GetData(CardId, "DiceCost");
        }

        protected override void OnRefresh()
        {
            _cardNameText.RefreshUIElement(_cardNameTextData);
            _cardTypeText.RefreshUIElement(_cardTypeTextData);
            _diceCostText.RefreshUIElement(_diceCostTextData);
        }

        protected override void OnInit()
        {
        }

        private void OnClickCard()
        {
            GameEvent.Send(EventKey.ChooseCard, CardId);
        }

        private void OnSelectCard(uint cardId)
        {
            _selected = cardId == CardId;
            OnRefresh();
        }
    }
}