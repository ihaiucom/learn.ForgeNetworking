using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/12/2017 10:27:30 AM
*  @Description:    节点流程
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public enum SNProcess
    {
        // 未知
        NONE,

        // 检测前提条件
        PRECONDITION,

        // 前摇
        ENTER,

        // 结算
        EXECUTE,

        // 后摇
        END
    }
}
