using System.Collections.Generic;
using Engine.Runtime;
using Engine.SettingModule;
using Engine.UI;
using UnityEngine;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceDiceCardWidget: UIWidget
    {
        private GameObject _cardNameTextGo;
        private UIText _cardNameText;
        private GameObject _diceCostTextGo;
        private UIText _diceCostText;
        private GameObject _border;

        private MeowDiceCardButton _button;
        private bool _selected;
        
        protected uint CardId => (uint)inputData["cardId"];
        
        private Dictionary<string, object> _cardNameTextData = new Dictionary<string, object>();
        private Dictionary<string, object> _diceCostTextData = new Dictionary<string, object>();
        
        protected override void OnCreate()
        {
            _cardNameTextGo = Go.transform.Find("CardName/Text").gameObject;
            _cardNameText = AddUIElement<UIText>(_cardNameTextGo);

            _diceCostTextGo = Go.transform.Find("DiceCost/Text").gameObject;
            _diceCostText = AddUIElement<UIText>(_diceCostTextGo);

            _border = Go.transform.Find("Border").gameObject;

        }

        protected override void BindProperty()
        {
            _cardNameText.InitData(_cardNameTextData);
            _diceCostText.InitData(_diceCostTextData);
        }

        protected override void OnRefreshData()
        {
            var table = TableModule.Get("Card");
            _cardNameTextData["text"] = table.GetData(CardId, "Name");
            _diceCostTextData["text"] = table.GetData(CardId, "DiceCost");
        }

        protected override void OnRefresh()
        {
            _cardNameText.RefreshUIElement(_cardNameTextData);
            _diceCostText.RefreshUIElement(_diceCostTextData);
            _border.SetActive(_selected);
        }

        protected override void OnInit()
        {
        }
    }
}