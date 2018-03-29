using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public struct DamageData
    {
        /// <summary>
        /// 攻击者
        /// </summary>
        public  UnitData                                AttackSend;
        /// <summary>
        /// 被攻击者
        /// </summary>
        public  List<UnitData>                          AttackBy;
        /// <summary>
        /// 攻击力
        /// </summary>
        public  float                                   AttakcVal;
        /// <summary>
        /// 伤害类型
        /// </summary>
        public  DamageType                              damageType;
        /// <summary>
        /// 是否技能伤害
        /// </summary>
        public  bool                                    bSkillDamage;
        /// <summary>
        /// 伤害信息
        /// </summary>
        public  DamageInfBaseCSV                        damageInfBaseCSV;

        /// <summary>
        /// 百分比伤害
        /// </summary>
        //public  float                                     DamagePer;
        /// <summary>
        /// 是否暴击
        /// </summary>
        public  bool                                    bCrit;
        /// <summary>
        /// 攻击类型
        /// 0   普通攻击
        /// 1   技能1攻击
        /// 2   技能2攻击
        /// 3   技能3攻击
        /// 。。。。。
        /// </summary>
        public  int                                     skillid;

        public SkillController skillController
        {
            get
            {
                return AttackSend.GetSkill(skillid);
            }
        }

        public SkillConfig skillConfig
        {
            get
            {
                if(skillController != null)
                    return skillController.skillConfig;
                return null;
            }
        }
    }

}
