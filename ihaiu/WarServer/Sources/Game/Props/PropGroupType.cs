using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 11:37:15 AM
*  @Description:    属性分组类型
* ==============================================================================
*/
namespace Games
{
    public enum PropGroupType
    {
        /** 不可逆, HP、Energy */
        Nonrevert = 0,

        /** 可回滚 */
        Revert,

        /** 状态 */
        State,
    }
}
