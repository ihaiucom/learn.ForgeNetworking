using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 投射物的方向和移动方法
    /// </summary>
    public enum ProjectileMoveMethod
    {
        /// <summary>
        /// 笔直地从角色发射出去
        /// </summary>
        ActorFaceDirection = 0,
        /// <summary>
        /// 成角度的发射，不止一个投射物，如多重箭
        /// </summary>
        AngledFromEach = 1,
        /// <summary>
        /// 投射物寻找到目标，然后朝着目标移动
        /// </summary>
        DirectToTarget = 2
    }
}