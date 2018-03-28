using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 6:40:51 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /** 关卡--波次配置 */
    [Serializable]
    public class StageWaveConfig
    {
        /** 备注 */
        public string                       describe = "描述备注";
        /** 是否需要检测怪已经被清场 */
        public bool                         needCheckClearance = true;
        /** [needCheckClearance=false有效] 等待时间 */
        public float                        waitTime = 0;
        /** 准备时间 */
        public float                        readyTime = 5;
        /** 单位配置列表 */
        [SerializeField]
        public List<StageWaveUnitConfig>    unitList = new List<StageWaveUnitConfig>();


        public StageWaveConfig Clone()
        {
            StageWaveConfig config = new StageWaveConfig();
            config.describe = describe;
            config.needCheckClearance = needCheckClearance;
            config.waitTime = waitTime;
            config.readyTime = readyTime;

            foreach(StageWaveUnitConfig unit in unitList)
            {
                config.unitList.Add(unit.Clone());
            }
            return config;
        }
    }
}
