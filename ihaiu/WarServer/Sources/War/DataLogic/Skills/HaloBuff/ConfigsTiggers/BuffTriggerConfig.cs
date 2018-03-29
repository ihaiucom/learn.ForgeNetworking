using FMODUnity;
using System;
using UnityEngine;
namespace Games.Wars
{
    [Serializable]
    public class BuffTriggerConfig
    {
        /// <summary>
        /// 触发类型
        /// </summary>
        public BuffTriggerType              buffTriggerType;
        /// <summary>
        /// 能否叠加
        /// </summary>
        public bool                         superposition           = true;
        /// <summary>
        /// 特效路径
        /// </summary>
        public string                       effectPath;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3                      effectOffset;
        /// <summary>
        /// 区域，针对halo
        /// </summary>
        public float                        radius;
        /// <summary>
        /// 触发时的音效文件路径
        /// </summary>
        [SoundKey]
        public string                       soundPath             = "";

        public  static BuffTriggerConfig CreateConfig(BuffTriggerType buffTriggerType)
        {
            BuffTriggerConfig config = null;
            switch (buffTriggerType)
            {
                case BuffTriggerType.Disposable:
                    config = new BuffTriggerConfigDisposable();
                    break;
                case BuffTriggerType.Periodic:
                    config = new BuffTriggerConfigPeriodic();
                    break;
                case BuffTriggerType.Frequency:
                    config = new BuffTriggerConfigFrequency();
                    break;
            }
            config.buffTriggerType = buffTriggerType;
            return config;
        }

    }
}