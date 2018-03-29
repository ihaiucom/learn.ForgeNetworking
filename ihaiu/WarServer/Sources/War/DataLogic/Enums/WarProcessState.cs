using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 2:47:47 PM
*  @Description:    战斗流程状态
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗流程状态 */
    public enum WarProcessState
    {
        /** 未知 */
        None,

        /** 退出 */
        Exit,

        /** 准备--等待连接房间 */
        Ready_WaitConnect,

        /** 准备--加载中 */
        Ready_Loading,

        /** 准备--等待进入游戏 */
        Ready_WaitEnter,

        /** 游戏进行中 */
        Gameing,

        /** 游戏结束 */
        GameOver,
    }
}
