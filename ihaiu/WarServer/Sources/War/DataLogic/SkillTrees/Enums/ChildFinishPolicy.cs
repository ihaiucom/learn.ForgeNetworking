using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/12/2017 4:43:17 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 并行节点--子节点结束还是继续策略  
    /// </summary>
    public enum ChildFinishPolicy
    {
        /** 子节点只执行一次流程 */
        CHILDFINISH_ONCE,
        /** 只要有一个节点是SNStatues.RUNNING， 那么其他子节点可以继续Tick */
        CHILDFINISH_LOOP
    }
}
