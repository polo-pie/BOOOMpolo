using System.Collections.Generic;
using Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCardWidget: UIWidget
    {
        private GameObject _cardNameTextGo;
        private UIText _cardNameText;
        private GameObject _cardTypeTextGo;
        private UIText _cardTypeText;
        private GameObject _diceCostTextGo;
        private UIText _diceCostText;

        private MeowDiceCardButton _button;
        
        protected MeowDiceCard Card => inputData["card"] as MeowDiceCard;
        private int _index;
        
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
            _button.OnPointerEnterCallback += OnPointerEnter;
            _button.OnPointerExitCallback += OnPointerExit;
            _button.onClick.AddListener(OnClickCard);
        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _cardTypeText.InitData(_cardTypeTextData);
            _diceCostText.InitData(_diceCostTextData);
        }

        protected override void OnRefreshData()
        {
            _cardNameTextData["text"] = Card.cardData.name;
            _cardTypeTextData["text"] = Card.cardData.cardType.ToString();
            _diceCostTextData["text"] = Card.cardData.DiceCost.ToString();
        }

        protected override void OnRefresh()
        {
            _cardNameText.RefreshUIElement(_cardNameTextData);
            _cardTypeText.RefreshUIElement(_cardTypeTextData);
            _diceCostText.RefreshUIElement(_diceCostTextData);
        }

        protected override void OnInit()
        {
            _index = (int)inputData["index"];
        }

        private void OnPointerEnter()
        {
            RectTransform.localScale = Vector3.one * 1.5f;
        }

        private void OnPointerExit()
        {
            RectTransform.localScale = Vector3.one;
        }

        private void OnClickCard()
        {
            MeowDiceCardGame.Instance.Player.SelectCard(_index);
        }
    }
}