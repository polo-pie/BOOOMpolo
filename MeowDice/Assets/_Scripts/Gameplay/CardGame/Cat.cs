using MeowDice.GamePlay.Settings;
using Unity.Mathematics;

namespace MeowDice.GamePlay
{
    public class Cat
    {
        /// <summary>
        /// san 值
        /// </summary>
        public int sanValue => _sanValue;

        private int _sanValue;

        /// <summary>
        /// 警戒度
        /// </summary>
        public int altertValue => _alterValue;

        private int _alterValue;

        public readonly int MaxSanValue;
        public readonly int MaxAlterValue;

        public Cat()
        {
            MaxAlterValue = (int)SettingModule.Instance.GlobalConfig.AlterLimit.y;
            MaxSanValue = (int)SettingModule.Instance.GlobalConfig.SanLimit.y;

            _sanValue = SettingModule.Instance.GlobalConfig.SanInitValue;
            _alterValue = SettingModule.Instance.GlobalConfig.AlterInitValue;
        }

        public void SanChange(int value)
        {
            _sanValue += value;
            _sanValue = math.max((int)SettingModule.Instance.GlobalConfig.SanLimit.x,
                math.min(_sanValue, (int)SettingModule.Instance.GlobalConfig.SanLimit.y));
        }

        public void AlterChange(int value)
        {
            _alterValue += value;
            _alterValue = math.max((int)SettingModule.Instance.GlobalConfig.AlterLimit.x,
                math.min(_alterValue, (int)SettingModule.Instance.GlobalConfig.AlterLimit.y));
        }

        public void OnRoundEnd()
        {
            
        }
    }
}