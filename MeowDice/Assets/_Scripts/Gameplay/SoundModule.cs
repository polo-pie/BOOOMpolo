using System.Collections;
using Engine.Runtime;
using Engine.SettingModule;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MeowDice.GamePlay
{
    public class SoundModule: UnitySingleton<SoundModule>
    {
        public AudioSource bgmSource;
        public AudioSource meowSource;
        public AudioSource otherSource;

        public void PlayBGM(int resourceId)
        {
            var table = TableModule.Get("SoundsResource");
            var path = table.GetData((uint)resourceId, "SoundsResource");

            var audio = Resources.Load<AudioClip>(path.ToString());
            bgmSource.clip = audio;
            bgmSource.Play();
        }

        public void PlayMeow()
        {
            var resourceId = Random.Range(6, 11);
            
            var table = TableModule.Get("SoundsResource");
            var path = table.GetData((uint)resourceId, "SoundsResource");

            var audio = Resources.Load<AudioClip>(path.ToString());
            meowSource.clip = audio;
            meowSource.Play();
        }

        public void PlayAudio(int resourceId)
        {
            var table = TableModule.Get("SoundsResource");
            var path = table.GetData((uint)resourceId, "SoundsResource");

            var audio = Resources.Load<AudioClip>(path.ToString());
            otherSource.clip = audio;
            otherSource.Play();
        }
    }
}