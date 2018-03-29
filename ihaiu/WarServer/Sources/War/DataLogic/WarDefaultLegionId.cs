using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 1:53:23 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 默认势力ID设定
    /// 0-9        为中立
    /// 10-19      为怪物 
    /// 20以上     为玩家
    /// </summary>
    public class WarDefaultLegionId
    {
        /** 中立 起始ID */
        public static int NeutralBegin  = 0;
        /** 怪物 起始ID */
        public static int MonsterBegin  = 10;
        /** 玩家 起始ID */
        public static int PlayerBegin   = 20;


        /** 获取势力ID */
        public static int GetLegionId(LegionType legionType, int index)
        {
            switch(legionType)
            {
                case LegionType.Neutral:
                    return NeutralBegin + index;
                case LegionType.Monster:
                    return MonsterBegin + index;
                case LegionType.Player:
                default:
                    return PlayerBegin + index;
            }
        }

        /** 获取势力Index */
        public static int GetLegionIndex(LegionType legionType, int legionId)
        {
            switch (legionType)
            {
                case LegionType.Neutral:
                    return legionId - NeutralBegin;
                case LegionType.Monster:
                    return legionId - MonsterBegin;
                case LegionType.Player:
                default:
                    return legionId - PlayerBegin;
            }
        }


        /** 获取势力描述 */
        public static string GetLegionDescribe(LegionType legionType, int legionId)
        {
            int index = GetLegionIndex(legionType, legionId);
            switch (legionType)
            {
                case LegionType.Neutral:
                    return string.Format("中立势力{0}", index) ;
                case LegionType.Monster:
                    return string.Format("怪物势力{0}", index);
                case LegionType.Player:
                default:
                    return string.Format("玩家势力{0}", index);
            }
        }

        /** 获取势力描述 */
        public static string GetLegionIDDescribe(LegionType legionType)
        {
            switch (legionType)
            {
                case LegionType.Neutral:
                    return string.Format("中立势力ID从{0}开始编", NeutralBegin);
                case LegionType.Monster:
                    return string.Format("怪物势力ID从{0}开始编", MonsterBegin);
                case LegionType.Player:
                default:
                    return string.Format("玩家势力ID从{0}开始编", PlayerBegin);
            }
        }





    }
}
