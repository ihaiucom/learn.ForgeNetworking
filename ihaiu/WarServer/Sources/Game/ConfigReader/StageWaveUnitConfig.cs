using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 10:49:21 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡--波次--单位配置 */
    public class StageWaveUnitConfig
    {
        /** 航线ID */
        public int              routeId         = 0;
        /** 势力ID */
        public int              legionId        = 0;
        /** 单位 */
        public StageUnitConfig  unit            = new StageUnitConfig();
        /** 数量 */
        public int              num             = 1;
        /** 延时 */
        public float            delay           = 0;
        /** 产兵间隔 */
        public float            interval        = 3;


        public UnitConfig GetUnitConfig()
        {
            return Game.config.unit.GetConfig(unit.unitId);
        }

        public int GetUnitId()
        {
            return GetUnitConfig().unitId;
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string GetIcon()
        {
            return GetUnitConfig().GetAvatarConfig().icon;
        }


        /// <summary>
        /// 士兵类型
        /// </summary>
        public UnitSoliderType GetSoliderType()
        {
            return GetUnitConfig().soliderType;
        }


        public StageWaveUnitConfig Clone()
        {
            StageWaveUnitConfig config = new StageWaveUnitConfig();
            config.routeId = routeId;
            config.legionId = legionId;
            config.unit = unit.Clone();
            config.num = num;
            config.delay = delay;
            config.interval = interval;
            return config;
        }

    }
}
