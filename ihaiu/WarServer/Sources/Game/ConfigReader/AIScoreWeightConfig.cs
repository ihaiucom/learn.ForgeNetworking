using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/17/2017 10:11:52 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// AI打分 权重
    /// </summary>
    public class AIScoreWeightConfig
    {
        public int id;

        public string name;

        // 怒气值权重
        public float weightHatred;
        // 单位类型权重
        public float weightType;
        // 距离权重
        public float weightDistance;
    }
}
