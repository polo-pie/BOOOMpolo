using System.Collections;
using System.Collections.Generic;
using Engine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MeowDice.GamePlay
{
    public class MainWindow : UIWindow
    {
        public override WindowType WindowType => WindowType.Normal;
        public override string PrefabPath => "UI/MenuWindow";

        private Button _startButton;
        private Button _settingButton;
        private Button _quitButton;

        private GameObject _content;
        private GameObject _video;

        protected override void OnCreate()
        {
            _content = Go.transform.Find("Content").gameObject;
            _video = Go.transform.Find("Video").gameObject;
            
            _startButton = Go.transform.Find("Content/ButtonStart").GetComponent<Button>();
            _settingButton = Go.transform.Find("Content/ButtonSetting").GetComponent<Button>();
            _quitButton = Go.transform.Find("Content/ButtonQuit").GetComponent<Button>();
            
            _startButton.onClick.AddListener(StartGame);
            _settingButton.onClick.AddListener(ShowSettingWindow);
            _quitButton.onClick.AddListener(QuitGame);


            UIModule.Instance.StartCoroutine(StartPlayVideo());
        }

        IEnumerator StartPlayVideo()
        {
            var firstOpen = PlayerPrefs.GetInt("FirstOpen", 0);
            if (firstOpen == 0)
            {
                _content.SetActive(false);
                _video.SetActive(true);

                var video = _video.GetComponent<VideoPlayer>();
                video.targetCamera = UIModule.Instance.UICamera;
                video.Play();

                var time = 0f;
                while (time < video.clip.length + 1)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
            
                _content.SetActive(true);
                _video.SetActive(false);

                PlayerPrefs.SetInt("FirstOpen", 1);
                PlayerPrefs.Save();
            }
            
            SoundModule.Instance.PlayBGM(2);
        }

        private void StartGame()
        {
            SoundModule.Instance.PlayAudio(11);
            SceneManager.LoadScene(2);
            Close();
        }

        private void ShowSettingWindow()
        {
            SoundModule.Instance.PlayAudio(11);
        }

        private void QuitGame()
        {
            SoundModule.Instance.PlayAudio(11);
            Application.Quit(1);
        }
    }
}

