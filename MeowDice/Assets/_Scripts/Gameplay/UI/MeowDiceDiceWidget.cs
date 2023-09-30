using System.Collections.Generic;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using MeowDice.GamePlay.UI;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
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
            GameEvent.AddEventListener<MeowDiceCard>(EventKey.SelectCard, OnSelectCard);
            GameEvent.AddEventListener<MeowDiceCard>(EventKey.UnselectCard, OnUnselectCard);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<MeowDiceCard>(EventKey.SelectCard, OnSelectCard);
            GameEvent.RemoveEventListener<MeowDiceCard>(EventKey.SelectCard, OnUnselectCard);
        }

        protected override void BindProperty()
        {
            _icon.InitData(_iconData);
        }

        protected override void OnRefreshData()
        {
            _iconData["path"] = "";
            // foreach (var card in MeowDiceCardGame.Instance.Player.selectedCards)
            for(int i = 0; i < MeowDiceCardGame.Instance.Player.selectedCards.Count; i ++)
            {
                var card = MeowDiceCardGame.Instance.Player.selectedCards[i];
                if(card != null)
                { 
                    if (i == _index)
                    {
                        _iconData["path"] = "";
                        break;
                    }
                }
            }
        }

        protected override void OnRefresh()
        {
            _icon.SetVisible(MeowDiceCardManager.DiceCount - MeowDiceCardGame.Instance.Player.RemainDiceCount > _index);
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

        private void OnSelectCard(MeowDiceCard card)
        {
            OnRefreshData();
            OnRefresh();
        }

        private void OnUnselectCard(MeowDiceCard card)
        {
            OnRefreshData();
            OnRefresh();
        }
    }
}