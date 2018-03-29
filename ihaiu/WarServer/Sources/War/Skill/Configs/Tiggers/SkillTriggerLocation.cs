using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    [Serializable]
    public class SkillTriggerLocation
    {
        /// <summary>
        /// 技能执行的方向和位置
        /// </summary>
        public TargetLocation       targetLocation;

        public  static SkillTriggerLocation CreateConfig(TargetLocation targetLocation)
        {
            SkillTriggerLocation config = null;
            switch (targetLocation)
            {
                case TargetLocation.LinearWave:
                    config = new SkillTriggerUseLinearWave();
                    break;
                case TargetLocation.FanWave:
                    config = new SkillTriggerUseFanWave();
                    break;
                case TargetLocation.CircularShockwave:
                    config = new SkillTriggerUseCircularShockwave();
                    break;
                case TargetLocation.TargetCircleArea:
                    config = new SkillTriggerUseTargetCircleArea();
                    break;
                default:
                    //对自身施法
                    config = new SkillTriggerLocation();
                    break;
            }
            config.targetLocation = targetLocation;
            return config;
        }

    }
}