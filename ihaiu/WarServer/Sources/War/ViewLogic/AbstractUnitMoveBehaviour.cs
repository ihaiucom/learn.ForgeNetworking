using Games.Wars;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/28/2017 11:18:16 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 抽象移动
    /// 临时放置视图里，以后放到数据逻辑层
    /// </summary>
    public abstract class AbstractUnitMoveBehaviour : AbstractUnitMonoBehaviour
    {

        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="onArrive">到达目标点回调</param>
        abstract public void MoveTo(Vector3 position, Action onArrive);


        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="position">位置</param>
        abstract public void MoveTo(Vector3 position);

        /// <summary>
        /// 停止移动
        /// </summary>
        abstract public void StopMove();
    }
}
