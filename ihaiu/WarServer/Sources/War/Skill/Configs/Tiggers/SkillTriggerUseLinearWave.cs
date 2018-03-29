using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 直线冲击波
    /// </summary>
    public class SkillTriggerUseLinearWave : SkillTriggerLocation
    {
        /// <summary>
        /// 直线宽度
        /// </summary>
        public  float           targetLocationWidth     = 0;
        /// <summary>
        /// 最大半径
        /// </summary>
        public  float           targetFanRadius         = 1;
        /// <summary>
        /// 最小半径
        /// </summary>
        public  float           targetMinRadius         = 0;

    }
}