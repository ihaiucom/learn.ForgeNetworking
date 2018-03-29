using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:48:58 PM
*  @Description:    单位类型
* ==============================================================================
*/
namespace Games.Wars
{
    /** 单位类型 */
    [System.Flags]
    public enum UnitProduceType
    {
        /** 正常 */
        Normal = 0,

        /** 克隆 */
        Clone = 1,

        /** 召唤 */
        Summoner = 2,
    }

}
