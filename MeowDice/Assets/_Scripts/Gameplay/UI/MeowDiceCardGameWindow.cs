using System.Collections.Generic;
using _Scripts.Gameplay.UI;
using Engine.Runtime;
using Engine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCardGameWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/MeowDiceCardGameWindow";
        
        private MeowDiceCardGameWindowCardWidget _cardGameWindowCardWidget;

        protected override void OnCreate()
        {
            outputData = new Dictionary<string, object>();
            var cardWidgetGo = Go.transform.Find("CardWidget").gameObject;

            _cardGameWindowCardWidget = AddUIElement<MeowDiceCardGameWindowCardWidget>(cardWidgetGo);
            GameEvent.AddEventListener(EventKey.EnterDiceStage, EnterDiceStage);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener(EventKey.EnterDiceStage, EnterDiceStage);
        }

        protected override void BindProperty()
        {
            _cardGameWindowCardWidget.InitData(outputData);
        }

        protected override void OnRefresh()
        {
            _cardGameWindowCardWidget.RefreshUIElement(outputData);
        }

        public void StartGame()
        {
            EnterCardUseStage();
        }

        public void EnterCardUseStage()
        {
            MeowDiceCardGame.Instance.EnterCardUseStage();
            _cardGameWindowCardWidget.RefreshUIElement(outputData);
        }

        private void EnterDiceStage()
        {
            var index = MeowDiceCardGame.Instance.CardManager.RandomCardFromSelectCards();
            var curtainWindow = UIModule.Instance.GetWindow<CurtainWindow>();
            curtainWindow.SetVisible(true);
            curtainWindow.PlayDiceAnimation();
        }
    }
}