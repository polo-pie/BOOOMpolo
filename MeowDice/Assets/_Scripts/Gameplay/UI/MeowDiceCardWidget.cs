using System.Collections.Generic;
using Engine.SettingModule;
using Engine.UI;
using UnityEngine;
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

            _button = Go.GetComponent<MeowDiceCardButton>();
            _button.OnPointerEnterCallback += OnPointerEnter;
            _button.OnPointerExitCallback += OnPointerExit;
            _button.onClick.AddListener(OnClickCard);
        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _diceCostText.InitData(_diceCostTextData);
            _cardImage.InitData(_cardImageData);
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
            SoundModule.Instance.PlayAudio(14);
            MeowDiceCardGame.Instance.Player.SelectCard(_index);
        }
    }
}