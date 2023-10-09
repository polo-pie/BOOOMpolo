using System.Collections;
using System.Collections.Generic;
using Engine.UI;
using MeowDice.GamePlay;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Main : MonoBehaviour
{
    void Start()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(UIModule.Instance.UICamera);
        UIModule.Instance.ShowUI<MainWindow>(new Dictionary<string, object>());
    }
}
