using FMODUnity;
using System;
namespace Games.Wars
{
    /// <summary>
    /// 技能事件 - 声音
    /// </summary>
    [Serializable]
    public class SkillActionConfigSound : SkillActionConfig
    {
        /// <summary>
        /// 音效文件路径
        /// </summary>
        [SoundKey]
        public  string                  soundPath                  = "";
    }
}
