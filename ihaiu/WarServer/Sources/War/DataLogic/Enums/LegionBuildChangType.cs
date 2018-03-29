using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/17/2017 7:05:54 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 势力建筑改变方式
    /// </summary>
    public enum LegionBuildChangType
    {
        [Help("无")]
        None,

        [Help("可占领")]
        EnableOccupy,

        [Help("可雇佣")]
        EnableEmploy,
    }
}
