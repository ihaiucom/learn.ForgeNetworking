using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class StoryTriggerWaveConfig : StoryTriggerConfig
    {
        public StoryTriggerWaveConfig()
        {

        }
        /// <summary>
        /// 波次id
        /// </summary>
        public  int                 waveIndex;
        /// <summary>
        /// 波次开始 false
        /// 波次结束 true
        /// </summary>
        public  bool                waveStartOrEnd          = false;
    }
}
