using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--触发器基类配置
    /// </summary>
    [Serializable]
    public class StoryTriggerConfig
    {
        /// <summary>
        /// 触发类型
        /// </summary>
        public TirggerType              tirggerType;
        /// <summary>
        /// 触发次数，-1表示每次都触发，0表示不触发
        /// </summary>
        public int                      tirggerNumbers;
        /// <summary>
        /// 剧情时长，-1表示整场战斗
        /// </summary>
        public float                    tirggerTime;



        public static StoryTriggerConfig CreateTirggerConfig(TirggerType tirggerType)
        {
            StoryTriggerConfig config = null;
            switch (tirggerType)
            {
                case TirggerType.WaveStart:
                    config = new StoryTriggerWaveConfig();
                    break;
                case TirggerType.WaveTipEnd:
                    config = new StoryTriggerWaveConfig();
                    break;
                case TirggerType.UnitDeath:
                    config = new StoryTriggerUnitDeathConfig();
                    break;
                case TirggerType.StageWin:
                case TirggerType.StageFail:
                case TirggerType.StageStart:
                    config = new StoryTriggerConfig();
                    break;
                default:
                    config = new StoryTriggerConfig();
                    break;

            }

            config.tirggerType = tirggerType;
            return config;


        }
    }
}