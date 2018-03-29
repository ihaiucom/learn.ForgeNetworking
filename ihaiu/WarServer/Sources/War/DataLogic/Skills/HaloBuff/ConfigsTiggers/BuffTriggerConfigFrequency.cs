using FMODUnity;
namespace Games.Wars
{
    /// <summary>
    /// 一定次数，依据条件减少次数
    /// </summary>
    public class BuffTriggerConfigFrequency : BuffTriggerConfig
    {
        public int              frequencyCount;
        /// <summary>
        /// 每次触发时的音效文件路径
        /// </summary>
        [SoundKey]
        public string           pulSoundPath             = "";

    }
}