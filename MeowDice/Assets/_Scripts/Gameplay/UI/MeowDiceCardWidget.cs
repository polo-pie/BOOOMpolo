using System.Collections.Generic;
using Engine.SettingModule;
using Engine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCardWidget: UIWidget
    {
        private GameObject _cardNameTextGo;
        private UIText _cardNameText;
        private GameObject _diceCostTextGo;
        private UIText _diceCostText;
        private GameObject _cardImageGo;
        private UIImage _cardImage;

        private MeowDiceCardButton _button;

        private GameObject _cardDescription;
        private Text _cardTitleText;
        private TextMeshProUGUI _cardDescriptionText;
        private Canvas _canvas;
        
        protected MeowDiceCard Card => inputData["card"] as MeowDiceCard;
        private int _index;
        
        private Dictionary<string, object> _cardNameTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _diceCostTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _cardImageData = new Dictionary<string, object>();

        protected override void OnCreate()
        {
            _cardNameTextGo = Go.transform.Find("CardName/Text").gameObject;
            _cardNameText = AddUIElement<UIText>(_cardNameTextGo);

            _diceCostTextGo = Go.transform.Find("DiceCost/Text").gameObject;
            _diceCostText = AddUIElement<UIText>(_diceCostTextGo);

            _cardImageGo = Go.transform.Find("Image").gameObject;
            _cardImage = AddUIElement<UIImage>(_cardImageGo);

            _cardDescription = Go.transform.Find("CardDescription").gameObject;
            _cardTitleText = Go.transform.Find("CardDescription/Title/Text").GetComponent<Text>();
            _cardDescriptionText = Go.transform.Find("CardDescription/Description/Text").GetComponent<TextMeshProUGUI>();

            _button = Go.GetComponent<MeowDiceCardButton>();
            _button.OnPointerEnterCallback += OnPointerEnter;
            _button.OnPointerExitCallback += OnPointerExit;
            _button.onClick.AddListener(OnClickCard);

            _canvas = Go.GetComponent<Canvas>();
        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _diceCostText.InitData(_diceCostTextData);
            _cardImage.InitData(_cardImageData);
            _cardTitleText.text = Card == null ? "" : Card.cardData.Name;
            _cardDescriptionText.text = Card == null ? "" : Card.cardData.Description;
        }

        protected override void OnRefreshData()
        {
            _cardNameTextData["text"] = Card == null? "" : Card.cardData.Name;
            _diceCostTextData["text"] = Card == null? "" : Card.cardData.DiceCost.ToString();
            var table = TableModule.Get("Card");
            _cardImageData["path"] = Card == null ? "" : table.GetData(Card.cardId, "CardImage");
        }

        protected override void OnRefresh()
        {
            _cardNameText.RefreshUIElement(_cardNameTextData);
            _diceCostText.RefreshUIElement(_diceCostTextData);
            _cardImage.RefreshUIElement(_cardImageData);
            _cardTitleText.text = Card == null ? "" : Card.cardData.Name;
            _cardDescriptionText.text = Card == null ? "" : Card.cardData.Description;
        }

        protected override void OnInit()
        {
            _index = (int)inputData["index"];
        }

        private void OnPointerEnter()
        {
            RectTransform.localScale = Vector3.one * 1.1f;
            _cardDescription.SetActive(true);
            _canvas.sortingOrder = 10;
        }

        private void OnPointerExit()
        {
            RectTransform.localScale = Vector3.one;
            _cardDescription.SetActive(false);
            _canvas.sortingOrder = 1;
        }

        private void OnClickCard()
        {
            SoundModule.Instance.PlayAudio(14);
            MeowDiceCardGame.Instance.Player.SelectCard(_index);
        }
    }
}