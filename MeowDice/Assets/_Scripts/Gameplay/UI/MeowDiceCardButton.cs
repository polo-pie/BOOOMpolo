using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MeowDice.GamePlay.UI
{
    public class MeowDiceCardButton : Button
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

