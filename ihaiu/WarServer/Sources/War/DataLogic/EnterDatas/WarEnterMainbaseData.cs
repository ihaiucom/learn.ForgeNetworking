using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 11:24:13 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战前数据--主基地数据 */
    public class WarEnterMainbaseData
    {
        /** 势力ID */
        public int                  legionId;
        /** 单位配置 */
        public WarEnterUnitData     unit = new WarEnterUnitData();
        /** 位置 */
        public Vector3                position;
        /** 方向 */
        public Vector3                rotation = Vector3.zero;
    }
}
