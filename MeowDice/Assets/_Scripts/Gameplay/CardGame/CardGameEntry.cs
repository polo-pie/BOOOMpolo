using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Engine.Runtime;
using UnityEngine;

namespace MeowDice.GamePlay
{
    public class CardGameEntry : MonoBehaviour
    {
        private void Awake()
        {
            MeowDiceCardGame.Instance.GameInit();
        }
    }
}

