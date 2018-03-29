using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 2:18:38 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 产兵流程状态 */
    public enum WarSpawnWaveState
    {
        /** 未知 */
        None,

        /** 准备时间 */
        ReadyTime,

        /** 产兵中 */
        Spawn,

        /** 等待中 */
        Waiting,

        /** 完成 */
        Final
    }
}
