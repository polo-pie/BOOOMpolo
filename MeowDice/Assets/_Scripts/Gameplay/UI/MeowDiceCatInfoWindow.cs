using System.Collections;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay;
using TMPro;
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

        private CatStateButton _angryStateBtn;
        private CatStateButton _memoryStateBtn;
        private TextMeshProUGUI _stateText;
        private GameObject _description;

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

            _angryStateBtn = Go.transform.Find("States/AngryState").GetComponent<CatStateButton>();
            _memoryStateBtn = Go.transform.Find("States/MemoryState").GetComponent<CatStateButton>();
            _stateText = Go.transform.Find("Des/Text").GetComponent<TextMeshProUGUI>();
            _description = Go.transform.Find("Des").gameObject;

            _angryStateBtn.OnPointerEnterCallback = ShowAngryStateDetail;
            _angryStateBtn.OnPointerExitCallback = HideStateDetail;
            _memoryStateBtn.OnPointerEnterCallback = ShowMemoryStateDetail;
            _memoryStateBtn.OnPointerExitCallback = HideStateDetail;
        }

        protected override void BindProperty()
        {
            _sanSlider.value = (float)_cat.SanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.AlterValue / _cat.MaxAlterValue;
            _sanText.text = $"{_cat.SanValue}/{_cat.MaxSanValue}";
            _alterText.text = $"{_cat.AlterValue}/{_cat.MaxAlterValue}";
            _angryStateBtn.gameObject.SetActive(_cat.angryStateCount > 0);
            _memoryStateBtn.gameObject.SetActive(_cat.memoryStateCount > 0);
            _description.SetActive(false);
        }

        protected override void OnRefresh()
        {
            _sanSlider.value = (float)_cat.SanValue / _cat.MaxSanValue;
            _alterSlider.value = (float)_cat.AlterValue / _cat.MaxAlterValue;
            _sanText.text = $"{_cat.SanValue}/{_cat.MaxSanValue}";
            _alterText.text = $"{_cat.AlterValue}/{_cat.MaxAlterValue}";
            _angryStateBtn.gameObject.SetActive(_cat.angryStateCount > 0);
            _memoryStateBtn.gameObject.SetActive(_cat.memoryStateCount > 0);
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

        private void ShowAngryStateDetail()
        {
            _description.SetActive(true);
            _stateText.text = $"愤怒/n持续{_cat.angryStateCount}回合";
        }

        private void ShowMemoryStateDetail()
        {
            _description.SetActive(true);
            _stateText.text = $"入梦/n持续{_cat.angryStateCount}回合";
        }

        private void HideStateDetail()
        {
            _description.SetActive(false);
        }
    }
}