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
            var cardWidgetGo = Go.transform.Find("CardWidget").gameObject;

            _cardGameWindowCardWidget = AddUIElement<MeowDiceCardGameWindowCardWidget>(cardWidgetGo);
        }
        
        
    }
}