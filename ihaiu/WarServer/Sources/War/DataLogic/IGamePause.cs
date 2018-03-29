using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 7:58:11 PM
*  @Description:    接口 -- 游戏暂停
* ==============================================================================
*/
namespace Games.Wars
{
    public interface IGamePause
    {

        /** 游戏--暂停 */
        void OnGamePause();

        /** 游戏--继续 */
        void OnGameUnPause();
    }
}
