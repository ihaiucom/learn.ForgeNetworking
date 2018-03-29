using FMODUnity;
namespace Games.Wars
{
    /// <summary>
    /// 周期性
    /// </summary>
    public class BuffTriggerConfigPeriodic : BuffTriggerConfig
    {
        public float                life;
        public float                cd;
        /// <summary>
        /// 每次触发时的音效文件路径
        /// </summary>
        [SoundKey]
        public string               pulSoundPath             = "";
    }
}