using System.Collections;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCatInfoWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/MeowDiceCatInfoWindow";

        private Cat _cat;
        private Text _sanText;
        private Text _alterText;

        private Slider _sanSlider;
        private Slider _alterSlider;

        protected override void OnCreate()
        {
            _sanSlider = Go.transform.Find("CatSan").GetComponent<Slider>();
            _alterSlider = Go.transform.Find("CatAlter").GetComponent<Slider>();
            _sanText = Go.transform.Find("CatSan/Text").GetComponent<Text>();
            _alterText = Go.transform.Find("CatAlter/Text").GetComponent<Text>();
            GameEvent.AddEventListener<uint, int, int>(EventKey.OnStartAct, OnStartAct);
            _cat = MeowDiceCardGame.Instance.Cat;

        }

        protected override void BindProperty()
        {
            _sanSlider.value = (float)_cat.SanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.AlterValue / _cat.MaxAlterValue;
            _sanText.text = $"{_cat.SanValue}/{_cat.MaxSanValue}";
            _alterText.text = $"{_cat.AlterValue}/{_cat.MaxAlterValue}";
        }

        protected override void OnRefresh()
        {
            _sanSlider.value = (float)_cat.SanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.AlterValue / _cat.MaxAlterValue;
            _sanText.text = $"{_cat.SanValue}/{_cat.MaxSanValue}";
            _alterText.text = $"{_cat.AlterValue}/{_cat.MaxAlterValue}";
        }

        private void OnStartAct(uint cardId, int alterChange, int sanChange)
        {

            UIModule.Instance.StartCoroutine(DoScanAndAlterChange());
        }

        IEnumerator DoScanAndAlterChange()
        {
            yield return new WaitForSeconds(1);
            var currentSanValue = _sanSlider.value;
            var currentAlterValue = _alterSlider.value;

            var targetSanValue = (float)_cat.SanValue / _cat.MaxSanValue;
            var targetAlterValue = (float)_cat.AlterValue / _cat.MaxAlterValue;

            float time = 0;
            float showTime = 1;
            while (time < showTime)
            {

                _sanSlider.value = math.lerp(currentSanValue, targetSanValue, time / showTime);
                _alterSlider.value = math.lerp(currentAlterValue, targetAlterValue, time / showTime);
                _sanText.text = $"{(int)(_cat.MaxSanValue * _sanSlider.value)}/{_cat.MaxSanValue}";
                _alterText.text = $"{(int)(_cat.MaxAlterValue * _alterSlider.value)}/{_cat.MaxAlterValue}";
                time += Time.deltaTime;
                yield return null;
            }

            _sanSlider.value = targetSanValue;
            _alterSlider.value = targetAlterValue;
            _sanText.text = $"{_cat.SanValue}/{_cat.MaxSanValue}";
            _alterText.text = $"{_cat.AlterValue}/{_cat.MaxAlterValue}";

            MeowDiceCardGame.Instance.RoundEnd();
        }
    }
}