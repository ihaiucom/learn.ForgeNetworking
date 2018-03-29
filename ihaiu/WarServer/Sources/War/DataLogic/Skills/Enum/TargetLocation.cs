using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 技能执行的方向和位置
    /// </summary>
    public enum TargetLocation
    {
        /// <summary>
        /// 直线冲击波
        /// </summary>
        LinearWave = 0,
        /// <summary>
        /// 扇形冲击波
        /// </summary>
        FanWave = 1,
        /// <summary>
        /// 自身周围圆形冲击波
        /// </summary>
        CircularShockwave = 2,
        /// <summary>
        /// 目标圆形区域
        /// </summary>
        TargetCircleArea = 3,
        /// <summary>
        /// 对自身施法
        /// </summary>
        Self = 4,
    }
}