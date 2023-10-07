using System;
using System.Collections.Generic;
using Engine.UI;
using MeowDice.GamePlay.Settings;
using MeowDice.GamePlay.UI;
using Unity.Mathematics;
using UnityEngine;

namespace MeowDice.GamePlay
{
    [Serializable]
    public class CatPosition
    {
        public int index;
        public Transform transform;
    }
    
    public class Cat
    {
        /// <summary>
        /// san 值
        /// </summary>
        public int SanValue => _sanValue;

        private int _sanValue;

        /// <summary>
        /// 警戒度
        /// </summary>
        public int AlterValue => _alterValue;

        private int _alterValue;

        public int ComfortValue => _comfortValue;
        private int _comfortValue;

        public readonly int MaxSanValue;
        public readonly int MaxAlterValue;

        public bool clearComfortInRoundEnd;

        public int memoryStateCount;
        public int angryStateCount;

        public Cat()
        {
            MaxAlterValue = (int)SettingModule.Instance.GlobalConfig.AlterLimit.y;
            MaxSanValue = (int)SettingModule.Instance.GlobalConfig.SanLimit.y;

            _sanValue = SettingModule.Instance.GlobalConfig.SanInitValue;
            _alterValue = SettingModule.Instance.GlobalConfig.AlterInitValue;
            _comfortValue = 0;
            clearComfortInRoundEnd = true;
            memoryStateCount = 0;
            angryStateCount = 0;
        }

        public void SanChange(int value)
        {
            _sanValue += value;
            _sanValue = math.max((int)SettingModule.Instance.GlobalConfig.SanLimit.x,
                math.min(_sanValue, (int)SettingModule.Instance.GlobalConfig.SanLimit.y));
        }

        public void AlterChange(int value)
        {
            if (value > 0)
            {
                if (value > _comfortValue)
                {
                    value = value - _comfortValue;
                    _comfortValue = 0;
                }
                else
                {
                    _comfortValue -= value;
                    value = 0;
                }
            }
            
            _alterValue += value;
            _alterValue = math.max((int)SettingModule.Instance.GlobalConfig.AlterLimit.x,
                math.min(_alterValue, (int)SettingModule.Instance.GlobalConfig.AlterLimit.y));
        }

        public void ComfortChange(int value)
        {
            _comfortValue += value;
            _comfortValue = math.max(0, _comfortValue);
            UIModule.Instance.GetWindow<MeowDiceCatInfoWindow>().RefreshUIElement(new Dictionary<string, object>());
        }

        public void OnRoundEnd()
        {
            if(clearComfortInRoundEnd)
                _comfortValue = 0;
            clearComfortInRoundEnd = true;
        }
    }
}