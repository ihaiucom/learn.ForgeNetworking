using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/12/2017 4:37:27 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{

    /// <summary>
    /// 并行节点--失败策略  
    /// </summary>
    public enum FailurePolicy
    {
        /** 一个子节点失败，就返回失败 */
        FAIL_ON_ONE,
        /** 所有子节点失败，才返回失败 */
        FAIL_ON_ALL
    }
}
