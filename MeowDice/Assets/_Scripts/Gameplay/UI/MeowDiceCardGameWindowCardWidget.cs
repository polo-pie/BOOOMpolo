using System.Collections.Generic;
using _Scripts.Gameplay.UI;
using Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCardGameWindowCardWidget: UIWidget
    {
        protected MeowDiceCardWidget card1;
        protected MeowDiceCardWidget card2;
        protected MeowDiceCardWidget card3;
        protected MeowDiceCardWidget card4;

        protected Dictionary<string, object> card1Data = new Dictionary<string, object>();
        protected Dictionary<string, object> card2Data = new Dictionary<string, object>();
        protected Dictionary<string, object> card3Data = new Dictionary<string, object>();
        protected Dictionary<string, object> card4Data = new Dictionary<string, object>();
        
        
        protected MeowDiceDiceWidget dice1;
        protected MeowDiceDiceWidget dice2;
        protected MeowDiceDiceWidget dice3;
        protected MeowDiceDiceWidget dice4;
        protected MeowDiceDiceWidget dice5;
        protected MeowDiceDiceWidget dice6;

        protected Dictionary<string, object> dice1Data = new Dictionary<string, object>() { { "index",  0 } };
        protected Dictionary<string, object> dice2Data = new Dictionary<string, object>() { { "index",  1 } };
        protected Dictionary<string, object> dice3Data = new Dictionary<string, object>() { { "index",  2 } };
        protected Dictionary<string, object> dice4Data = new Dictionary<string, object>() { { "index",  3 } };
        protected Dictionary<string, object> dice5Data = new Dictionary<string, object>() { { "index",  4 } };
        protected Dictionary<string, object> dice6Data = new Dictionary<string, object>() { { "index",  5 } };

        protected Button button;


        protected override void OnCreate()
        {
            var card1Go = Go.transform.Find("CardLists/Card1").gameObject;
            var card2Go = Go.transform.Find("CardLists/Card2").gameObject;
            var card3Go = Go.transform.Find("CardLists/Card3").gameObject;
            var card4Go = Go.transform.Find("CardLists/Card4").gameObject;

            card1 = AddUIElement<MeowDiceCardWidget>(card1Go, false);
            card2 = AddUIElement<MeowDiceCardWidget>(card2Go, false);
            card3 = AddUIElement<MeowDiceCardWidget>(card3Go, false);
            card4 = AddUIElement<MeowDiceCardWidget>(card4Go, false);

            var dice1Go = Go.transform.Find("DiceLists/Dice1").gameObject;
            var dice2Go = Go.transform.Find("DiceLists/Dice2").gameObject;
            var dice3Go = Go.transform.Find("DiceLists/Dice3").gameObject;
            var dice4Go = Go.transform.Find("DiceLists/Dice4").gameObject;
            var dice5Go = Go.transform.Find("DiceLists/Dice5").gameObject;
            var dice6Go = Go.transform.Find("DiceLists/Dice6").gameObject;

            dice1 = AddUIElement<MeowDiceDiceWidget>(dice1Go);
            dice2 = AddUIElement<MeowDiceDiceWidget>(dice2Go);
            dice3 = AddUIElement<MeowDiceDiceWidget>(dice3Go);
            dice4 = AddUIElement<MeowDiceDiceWidget>(dice4Go);
            dice5 = AddUIElement<MeowDiceDiceWidget>(dice5Go);
            dice6 = AddUIElement<MeowDiceDiceWidget>(dice6Go);

            button = Go.transform.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(OnConfirmClick);
        }

        protected override void OnRefreshData()
        {
            var player = MeowDiceCardGame.Instance.Player;
            card1Data["card"] = player.GetCard(0);
            card1Data["index"] = 0;
            
            card2Data["card"] = player.GetCard(1);
            card2Data["index"] = 1;
            
            card3Data["card"] = player.GetCard(2);
            card3Data["index"] = 2;
            
            card4Data["card"] = player.GetCard(3);
            card4Data["index"] = 3;
            
            card1.SetVisible(card1Data["card"] != null);
            card2.SetVisible(card2Data["card"] != null);
            card3.SetVisible(card3Data["card"] != null);
            card4.SetVisible(card4Data["card"] != null);
            
        }

        protected override void BindProperty()
        {
            card1.InitData(card1Data);
            card2.InitData(card2Data);
            card3.InitData(card3Data);
            card4.InitData(card4Data);
            
            dice1.InitData(dice1Data);
            dice2.InitData(dice2Data);
            dice3.InitData(dice3Data);
            dice4.InitData(dice4Data);
            dice5.InitData(dice5Data);
            dice6.InitData(dice6Data);

        }

        protected override void OnRefresh()
        {
            card1.RefreshUIElement(card1Data);
            card2.RefreshUIElement(card2Data);
            card3.RefreshUIElement(card3Data);
            card4.RefreshUIElement(card4Data);
            
            dice1.RefreshUIElement(dice1Data);
            dice2.RefreshUIElement(dice2Data);
            dice3.RefreshUIElement(dice3Data);
            dice4.RefreshUIElement(dice4Data);
            dice5.RefreshUIElement(dice5Data);
            dice6.RefreshUIElement(dice6Data);
        }

        private void OnConfirmClick()
        {
            MeowDiceCardGame.Instance.EnterDiceStage();
        }
    }
}