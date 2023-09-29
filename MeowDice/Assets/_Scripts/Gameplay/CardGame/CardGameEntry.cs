using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Engine.Runtime;
using Engine.UI;
using MeowDice.GamePlay.UI;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class CardGameEntry : MonoBehaviour
    {
        private void Awake()
        {
            MeowDiceCardGame.Instance.GameInit();
            UIModule.Instance.ShowUI<MeowDiceCardGameWindow>();
        }
    }
}

