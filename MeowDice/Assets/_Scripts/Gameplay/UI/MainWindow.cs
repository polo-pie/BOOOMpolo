using System.Collections;
using System.Collections.Generic;
using Engine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MeowDice.GamePlay
{
    public class MainWindow : UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/MenuWindow";

        private Button _startButton;
        private Button _settingButton;
        private Button _quitButton;

        protected override void OnCreate()
        {
            _startButton = Go.transform.Find("Content/ButtonStart").GetComponent<Button>();
            _settingButton = Go.transform.Find("Content/ButtonSetting").GetComponent<Button>();
            _quitButton = Go.transform.Find("Content/ButtonQuit").GetComponent<Button>();
            
            _startButton.onClick.AddListener(StartGame);
            _settingButton.onClick.AddListener(ShowSettingWindow);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene(1);
            Close();
        }

        private void ShowSettingWindow()
        {
            
        }

        private void QuitGame()
        {
            Application.Quit(1);
        }
    }
}

