using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/23/2017 4:52:45 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// AI 触发前提条件类型
    /// </summary>
    public enum AIPreconditionType
    {
        [Help("CD完就可以使用")]
        None,

        [Help("血量范围")]
        HP,
    }
}
