using System.Collections.Generic;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine.UI;

namespace _Scripts.Gameplay.UI
{
    public class MeowDiceDiceWidget: UIWidget
    {
        private UIImage _icon;
        private Button _button;

        private int _index;
        private Dictionary<string, object> _iconData = new Dictionary<string, object>();

        protected override void OnCreate()
        {
            var icon = Go.transform.Find("Icon").gameObject;
            _button = Go.GetComponent<Button>();
            _icon = AddUIElement<UIImage>(icon);
            _button.onClick.AddListener(OnDiceClick);
        }

        protected override void BindProperty()
        {
            _icon.InitData(_iconData);
        }

        protected override void OnRefreshData()
        {
            var pos = 0;
            foreach (var card in MeowDiceCardGame.Instance.Player.selectedCards)
            {
                if (pos == _index)
                {
                    _iconData["path"] = "";
                    break;
                }

                pos += card.cardData.DiceCost;
            }
        }

        protected override void OnRefresh()
        {
            _icon.SetVisible(MeowDiceCardManager.DiceCount - MeowDiceCardGame.Instance.Player.RemainDiceCount >= _index);
            _icon.RefreshUIElement(_iconData);
        }

        protected override void OnInit()
        {
            _index = (int)inputData["index"];
        }

        private void GetCardImagePath()
        {
            

        }

        private void OnDiceClick()
        {
            var pos = 0;
            foreach (var card in MeowDiceCardGame.Instance.Player.selectedCards)
            {
                if (pos == _index)
                {
                    MeowDiceCardGame.Instance.Player.UnSelectCard(card);
                    break;
                }

                pos += card.cardData.DiceCost;
            }
        }
    }
}