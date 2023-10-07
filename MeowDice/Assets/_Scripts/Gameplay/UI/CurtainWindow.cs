using System.Collections;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using MeowDice.GamePlay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class CurtainWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/CurtainWindow";

        // 骰子阶段
        private GameObject _dicePanel;
        private Animation _diceAnimation;
        private Image _diceImage;

        private Text _curtainText;
        private RectTransform _curtain;
        
        protected override void OnCreate()
        {
            _curtainText = Go.transform.Find("Curtain/Text").GetComponent<Text>();
            _curtain = Go.transform.Find("Curtain").GetComponent<RectTransform>();

            _dicePanel = Go.transform.Find("Dice").gameObject;
            _diceAnimation = _dicePanel.transform.Find("DiceImage").GetComponent<Animation>();
            _diceImage = _dicePanel.transform.Find("ResultImage").GetComponent<Image>();
            GameEvent.AddEventListener<bool>(EventKey.OnGameEnd, OnGameEnd);
        }

        public void PlayDiceAnimation()
        {
            _curtain.gameObject.SetActive(false);
            _dicePanel.SetActive(true);
            _diceAnimation.gameObject.SetActive(false);
            _diceImage.gameObject.SetActive(false);
            UIModule.Instance.StartCoroutine(CoPlayDiceAnimation());
        }

        IEnumerator CoPlayDiceAnimation()
        {
            var panelRectTransform = _dicePanel.GetComponent<RectTransform>();
            var position = new Vector3(0, _curtain.rect.height, 0);
            var targetPosition = Vector3.zero;
            panelRectTransform.localPosition = position;
            _curtainText.text = "";

            float time = 0;
            float playTime = 1;
            while (time < playTime)
            {
                time += Time.deltaTime;
                panelRectTransform.localPosition = Vector3.Lerp(position, targetPosition, time / playTime);
                yield return null;
            }

            _diceAnimation.gameObject.SetActive(true);
            // _diceAnimation.Play();
            _curtain.localPosition = targetPosition;
            UIModule.Instance.GetWindow<MeowDiceCardGameWindow>().SetVisible(false);
            UIModule.Instance.GetWindow<MeowDiceCatInfoWindow>().SetVisible(true);

            // _curtainText.text = $"骰子点数{MeowDiceCardGame.Instance.CardManager.RandomSelectCardIndex}";

            yield return new WaitForSeconds(1f);
            
            _diceAnimation.gameObject.SetActive(false);
            var texture2D = Resources.Load<Texture2D>(
                $"Art/ui/gaming/shades/result/dice{MeowDiceCardGame.Instance.CardManager.RandomSelectCardIndex + 1}");
            _diceImage.gameObject.SetActive(true);
            _diceImage.sprite = Sprite.Create(texture2D, Rect.MinMaxRect(0, 0, texture2D.width, texture2D.height), Vector2.one / 2);
            
            yield return new WaitForSeconds(1f);
            
            time = 0;
            while (time < playTime)
            {
                time += Time.deltaTime;
                panelRectTransform.localPosition = Vector3.Lerp(targetPosition, position, time / playTime);
                yield return null;
            }

            panelRectTransform.localPosition = position;
            MeowDiceCardGame.Instance.EnterActStage();
            _curtain.gameObject.SetActive(true);
            _dicePanel.SetActive(false);
            SetVisible(false);
        }

        public void OnEnterNextRound()
        {
            SetVisible(true);
            PlayEnterNextRound();
        }

        public void PlayEnterNextRound()
        {
            UIModule.Instance.GetWindow<MeowDiceCatInfoWindow>().SetVisible(false);
            _curtain.localPosition = Vector3.zero;
            _curtainText.text = "接下来想让猫咪做什么";
            // MeowDiceCardGame.Instance.RoundEnd();

            UIModule.Instance.StartCoroutine(CoEnterNextRound());
        }

        IEnumerator CoEnterNextRound()
        {
            yield return new WaitForSeconds(1);
            SetVisible(false);
            var window = UIModule.Instance.GetWindow<MeowDiceCardGameWindow>();
            window.SetVisible(true);
            window.EnterCardUseStage();
        }

        private void OnGameEnd(bool isWin)
        {
            SetVisible(true);
            _curtain.localPosition = Vector3.zero;
            _curtainText.text = isWin ? "胜利" : "失败";
        }
    }
}