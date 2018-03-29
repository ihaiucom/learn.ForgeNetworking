using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 8:13:23 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public interface IScript
    {
        /** 重置 */
        void CReset();

        /** 释放 */
        void CRelease();
    }
}
