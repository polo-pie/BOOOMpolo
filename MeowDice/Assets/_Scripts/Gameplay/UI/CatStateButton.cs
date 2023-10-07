using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MeowDice.GamePlay
{
    public class CatStateButton: Button
    {
        public Action OnPointerEnterCallback;
        public Action OnPointerExitCallback;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnPointerEnterCallback?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnPointerExitCallback?.Invoke();
        }   
    }
}