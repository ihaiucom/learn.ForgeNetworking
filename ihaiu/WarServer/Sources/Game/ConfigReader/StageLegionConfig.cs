using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 11:35:36 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡--势力配置 */
    public class StageLegionConfig
    {
        /** 势力类型 */
        public LegionType   legionType;
        /** 中立势力建筑关系类型 */
        public LegionBuildChangType neutralBuildChangeType;
        /** 势力颜色索引类型 */
        public int          colorIndex;
        /** 势力ID */
        public int          legionId;
        /** 备注描述 */
        public string       describe;
        /** ai配置(非真人玩家有效) */
        public int          ai;
        /** 出生区域 */
        public int          regionId;
        /** 组ID */
        public int          groupId;

    }
}
