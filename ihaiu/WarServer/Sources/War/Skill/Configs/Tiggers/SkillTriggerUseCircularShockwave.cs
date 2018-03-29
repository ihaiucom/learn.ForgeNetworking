using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 自身周围圆形冲击波
    /// </summary>
    public class SkillTriggerUseCircularShockwave : SkillTriggerLocation
    {
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