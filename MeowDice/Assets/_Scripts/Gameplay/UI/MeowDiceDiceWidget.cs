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
        private Image _banIcon;
        private Button _button;
        private MeowDiceDiceCardWidget _card;

        private int _index;
        private Dictionary<string, object> _cardData = new Dictionary<string, object>();
        
        protected override void OnCreate()
        {
            _banIcon = Go.transform.Find("Icon").GetComponent<Image>();
            var cardGo = Go.transform.Find("Card").gameObject;
            _card = AddUIElement<MeowDiceDiceCardWidget>(cardGo);
            
            _button = Go.GetComponent<Button>();
            GameEvent.AddEventListener<MeowDiceCard>(EventKey.SelectCard, OnSelectCard);
            GameEvent.AddEventListener<MeowDiceCard>(EventKey.UnselectCard, OnUnselectCard);
            _button.onClick.AddListener(OnDiceClick);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<MeowDiceCard>(EventKey.SelectCard, OnSelectCard);
            GameEvent.RemoveEventListener<MeowDiceCard>(EventKey.UnselectCard, OnUnselectCard);
        }

        protected override void BindProperty()
        {
            // _banIcon.InitData(_iconData);
            _banIcon.gameObject.SetActive(false);
            
        }

        protected override void OnRefreshData()
        {
            _cardData["cardId"] = GetCardId();
        }

        protected override void OnRefresh()
        {
            // _banIcon.gameObject.SetActive(MeowDiceCardManager.DiceCount - MeowDiceCardGame.Instance.Player.RemainDiceCount > _index);
            // _banIcon.RefreshUIElement(_iconData);
            if ((int)_cardData["cardId"] != -1)
            {
                _cardData["cardId"] = uint.Parse(_cardData["cardId"].ToString());
                _card.SetVisible(true);
                _card.RefreshUIElement(_cardData);
                // _banIcon.gameObject.SetActive(false);
            }
            else
            {
                _card.SetVisible(false);
                // _banIcon.gameObject.SetActive(true);
            }
        }

        protected override void OnInit()
        {
            _index = (int)inputData["index"];
        }

        private int GetCardId()
        {
            var card = MeowDiceCardGame.Instance.Player.selectedCards[_index];

            if (card != null)
            {
                return (int)card.cardId;
            }

            if (_index < MeowDiceCardManager.DiceCount - MeowDiceCardGame.Instance.Player.RemainDiceCount)
            {
                return 0;
            }

            return -1;
        }

        private void OnDiceClick()
        {
            if (MeowDiceCardGame.Instance.Player.selectedCards[_index] != null)
            {
                MeowDiceCardGame.Instance.Player.UnSelectCard(MeowDiceCardGame.Instance.Player.selectedCards[_index]);
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