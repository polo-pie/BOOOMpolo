using System.Collections.Generic;
using Engine.Runtime;
using Engine.SettingModule;
using Engine.UI;
using MeowDice.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class SelectCardWindowItem: UIWidget
    {
        private GameObject _cardNameTextGo;
        private UIText _cardNameText;
        private GameObject _diceCostTextGo;
        private UIText _diceCostText;
        private GameObject _border;
        
        private GameObject _cardImageGo;
        private UIImage _cardImage;

        private MeowDiceCardButton _button;
        private GameObject _cardDescription;
        private Text _cardTitleText;
        private TextMeshProUGUI _cardDescriptionText;
        private bool _selected;
        
        private Canvas _canvas;
        
        protected uint CardId => (uint)inputData["cardId"];
        
        private Dictionary<string, object> _cardNameTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _diceCostTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _cardImageData = new Dictionary<string, object>();

        protected override void OnCreate()
        {
            _cardNameTextGo = Go.transform.Find("CardName/Text").gameObject;
            _cardNameText = AddUIElement<UIText>(_cardNameTextGo);

            _diceCostTextGo = Go.transform.Find("DiceCost/Text").gameObject;
            _diceCostText = AddUIElement<UIText>(_diceCostTextGo);

            _button = Go.GetComponent<MeowDiceCardButton>();
            _button.onClick.AddListener(OnClickCard);
            _button.OnPointerEnterCallback += OnPointerEnter;
            _button.OnPointerExitCallback += OnPointerExit;
            
            _cardImageGo = Go.transform.Find("Image").gameObject;
            _cardImage = AddUIElement<UIImage>(_cardImageGo);

            _border = Go.transform.Find("Border").gameObject;
            
            _cardDescription = Go.transform.Find("CardDescription").gameObject;
            _cardTitleText = Go.transform.Find("CardDescription/Title/Text").GetComponent<Text>();
            _cardDescriptionText = Go.transform.Find("CardDescription/Description/Text").GetComponent<TextMeshProUGUI>();
            
            _canvas = Go.GetComponent<Canvas>();
            GameEvent.AddEventListener<uint>(EventKey.ChooseCard, OnSelectCard);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<uint>(EventKey.ChooseCard, OnSelectCard);
        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _diceCostText.InitData(_diceCostTextData);
            _cardImage.InitData(_cardImageData);
            
            var table = TableModule.Get("Card");
            _cardTitleText.text = table.GetData(CardId, "Name").ToString();
            _cardDescriptionText.text = table.GetData(CardId, "CardDes").ToString();
        }

        protected override void OnRefreshData()
        {
            var table = TableModule.Get("Card");
            _cardNameTextData["text"] = table.GetData(CardId, "Name");
            _diceCostTextData["text"] = table.GetData(CardId, "DiceCost");
            _cardImageData["path"] = table.GetData(CardId, "CardImage");
        }

        public void UnSelect()
        {
            _selected = false;
        }

        protected override void OnRefresh()
        {
            _cardNameText.RefreshUIElement(_cardNameTextData);
            _diceCostText.RefreshUIElement(_diceCostTextData);
            _cardImage.RefreshUIElement(_cardImageData);
            _border.SetActive(_selected);
            var table = TableModule.Get("Card");
            _cardTitleText.text = table.GetData(CardId, "Name").ToString();
            _cardDescriptionText.text = table.GetData(CardId, "CardDes").ToString();
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
        
        private void OnPointerEnter()
        {
            _cardDescription.SetActive(true);
            _canvas.sortingOrder = 10;
        }

        private void OnPointerExit()
        {
            _cardDescription.SetActive(false);
            _canvas.sortingOrder = 1;
        }
    }
}