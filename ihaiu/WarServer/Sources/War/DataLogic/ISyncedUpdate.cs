using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 7:58:11 PM
*  @Description:    接口 -- 同步更新
* ==============================================================================
*/

namespace Games.Wars
{
    public interface ISyncedUpdate
    {
        /** 游戏--同步更新 */
        void OnSyncedUpdate();
    }
}