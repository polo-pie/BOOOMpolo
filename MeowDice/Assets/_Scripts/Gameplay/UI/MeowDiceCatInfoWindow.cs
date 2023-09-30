using System.Collections;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Gameplay.UI
{
    public class MeowDiceCatInfoWindow: UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/MeowDiceCatInfoWindow";

        private Cat _cat;

        private Slider _sanSlider;
        private Slider _alterSlider;

        protected override void OnCreate()
        {
            _sanSlider = Go.transform.Find("CatSan").GetComponent<Slider>();
            _alterSlider = Go.transform.Find("CatAlter").GetComponent<Slider>();
            GameEvent.AddEventListener<uint, int, int>(EventKey.OnStartAct, OnStartAct);
            _cat = MeowDiceCardGame.Instance.Cat;

        }

        protected override void BindProperty()
        {
            _sanSlider.value = (float)_cat.sanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.altertValue / _cat.MaxAlterValue;
        }

        protected override void OnRefresh()
        {
            _sanSlider.value = (float)_cat.sanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.altertValue / _cat.MaxAlterValue;
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

            var targetSanValue = (float)_cat.sanValue / _cat.MaxSanValue;
            var targetAlterValue = (float)_cat.altertValue / _cat.MaxAlterValue;

            float time = 0;
            float showTime = 1;
            while (time < showTime)
            {

                _sanSlider.value = math.lerp(currentSanValue, targetSanValue, time / showTime);
                _alterSlider.value = math.lerp(currentAlterValue, targetAlterValue, time / showTime);
                time += Time.deltaTime;
                yield return null;
            }

            _sanSlider.value = targetSanValue;
            _alterSlider.value = targetAlterValue;

            MeowDiceCardGame.Instance.RoundEnd();
        }
    }
}