using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 11:01:14 AM
*  @Description:    战前数据--势力组
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战前数据--势力组 */
    public class WarEnterGroupData
    {
        /** 组ID */
        public int                          groupId;
        /** 玩家列表 */
        public List<WarEnterLegionData>     legionList = new List<WarEnterLegionData>();
    }
}
