using System.Collections;
using System.Collections.Generic;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        SoundModule.Instance.PlayBGM(2);
        UIModule.Instance.ShowUI<MainWindow>(new Dictionary<string, object>());
    }
}
