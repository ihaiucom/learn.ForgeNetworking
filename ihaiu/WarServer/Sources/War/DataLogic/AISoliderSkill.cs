using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/23/2017 5:08:44 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 士兵技能AI
    /// </summary>
    public class AISoliderSkill
    {
        public UnitData         unit;
        public AISoliderConfig  aiSoliderConfig;
        public SkillController  skillController;
        public float weightMin = 0;
        public float weightMax = 100;
        public float weightMinVal = 0;
        public float weightMaxVal = 100;

        /// <summary>
        /// 当前使用次数
        /// </summary>
        public int useNum = 0;

        public void OnUse()
        {
            useNum++;
            unit.attackCD = unit.prop.AttackInterval;
            skillController.cd = skillController.skillLevelConfig.cd;
        }

        public bool enableUse
        {
            get
            {
                if(skillController.HasConfig && skillController.cd <= 0)
                {
                    if(aiSoliderConfig.enableUseCount > 0 && useNum >= aiSoliderConfig.enableUseCount)
                    {
                        return false;
                    }

                    if(aiSoliderConfig.precoditionType == AIPreconditionType.HP)
                    {
                        float hpRate = unit.prop.HpRate;
                        if (hpRate >= aiSoliderConfig.minHP && hpRate <= aiSoliderConfig.maxHP)
                        {
                            return true;
                        }
                        
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
