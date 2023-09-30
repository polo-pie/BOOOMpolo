using System.Collections;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using MeowDice.GamePlay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.UI
{
    public class CurtainWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/CurtainWindow";

        private Text _curtainText;
        private RectTransform _curtain;
        
        protected override void OnCreate()
        {
            _curtainText = Go.transform.Find("Curtain/Text").GetComponent<Text>();
            _curtain = Go.transform.Find("Curtain").GetComponent<RectTransform>();
            GameEvent.AddEventListener(EventKey.OnEnterNextRound, OnEnterNextRound);
            GameEvent.AddEventListener<bool>(EventKey.OnGameEnd, OnGameEnd);
        }

        public void PlayDiceAnimation()
        {
            UIModule.Instance.StartCoroutine(CoPlayDiceAnimation());
        }

        IEnumerator CoPlayDiceAnimation()
        {
            var position = new Vector3(0, _curtain.rect.height, 0);
            var targetPosition = Vector3.zero;
            _curtain.localPosition = position;
            _curtainText.text = "";

            float time = 0;
            float playTime = 1;
            while (time < playTime)
            {
                time += Time.deltaTime;
                _curtain.localPosition = Vector3.Lerp(position, targetPosition, time / playTime);
                yield return null;
            }

            _curtain.localPosition = targetPosition;
            UIModule.Instance.GetWindow<MeowDiceCardGameWindow>().SetVisible(false);
            UIModule.Instance.GetWindow<MeowDiceCatInfoWindow>().SetVisible(true);

            _curtainText.text = $"骰子点数{MeowDiceCardGame.Instance.CardManager.RandomSelectCardIndex}";

            yield return new WaitForSeconds(1f);

            time = 0;
            while (time < playTime)
            {
                time += Time.deltaTime;
                _curtain.localPosition = Vector3.Lerp(targetPosition, position, time / playTime);
                yield return null;
            }

            _curtain.localPosition = position;
            MeowDiceCardGame.Instance.EnterActStage();
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

        public void OnGameEnd(bool isWin)
        {
            SetVisible(true);
            _curtain.localPosition = Vector3.zero;
            _curtainText.text = isWin ? "胜利" : "失败";
        }
    }
}