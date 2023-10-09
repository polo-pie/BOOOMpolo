using System.Collections;
using System.Collections.Generic;
using Engine.Runtime;
using Engine.SettingModule;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.UI
{
    public class ChatWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/ChatWindow";

        private Text _text;
        private Button _button;

        private static List<int> _dialogues = new List<int>();
        private bool _jump;
        
        protected override void OnCreate()
        {
            _text = Go.transform.Find("Content/Text").GetComponent<Text>();
            _button = Go.GetComponent<Button>();
            _dialogues.Clear();
            _jump = false;
            
            _button.onClick.AddListener(OnChatClick);

            GameEvent.AddEventListener<uint>(EventKey.OnStartDialogue, ShowDialogue);
        }

        protected override void OnDestroy()
        {
            GameEvent.RemoveEventListener<uint>(EventKey.OnStartDialogue, ShowDialogue);
        }

        public void ShowDialogue(uint cardId)
        {
            var table = TableModule.Get("Card");
            _dialogues = table.GetData(cardId, "Dialogue") as List<int>;
            UIModule.Instance.StartCoroutine(CoShowDialogue(0));
            SetVisible(true);
        }

        private IEnumerator CoShowDialogue(int index)
        {
            if (index >= _dialogues.Count)
            {
                SetVisible(false);
                MeowDiceCardGame.Instance.RoundEnd();
                yield break;
            }

            var table = TableModule.Get("CardDialogue");
            var dialogue = table.GetData((uint)_dialogues[index], "Text");
            _text.text = dialogue.ToString();

            while (!_jump)
            {
                yield return null;
            }

            _jump = false;
            yield return CoShowDialogue(index + 1);
        }

        private void OnChatClick()
        {
            _jump = true;
        }
    }
}