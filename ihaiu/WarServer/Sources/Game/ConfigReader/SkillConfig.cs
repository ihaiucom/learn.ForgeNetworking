using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 1:33:35 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 技能配置 */
    public class SkillConfig
    {

        /** 技能ID */
        public int skillId;
        /** 名称 */
        public string name;
        /** 描述文字 */
        public string tip;
        /** 图标 */
        public string icon;

        /** 是否是被动技能 1主动2被动 冷却时间 */
        public bool isPassive = false;
        /** 被动触发方式：0无效1攻击触发2施法触发3出生触发 */
        public SkillPassiveType passiveType = SkillPassiveType.None;

        /** 操作--主动施法方式: 主动施法方式: 0无效1直线 2扇形3身边圆形区域4目标圆形区域5对自己6方向瞄准 */
        public SkillCastType castType = SkillCastType.None;
        /** 操作--可攻击范围半径 */
        public float castRadius = 0;
        /** 操作--直线宽 */
        public float castLineWidth = 0;
        /** 操作--扇形夹角 */
        public float castFanAngle = 0;
        /** 操作--圆形半径 */
        public float castCircleRadius = 0;
        /** AIScoreUnitType单位类型打分表 */
        public int aiScoreUnitType = 0;
        /** AIScoreDistance单位距离打分表 */
        public int aiScoreDistance = 0;
        /** AIScoreAttackHatred单位攻击怒气值给分表 */
        public int aiScoreAttackHatred = 0;
        /** AIScoreWeight权重打分表 */
        public int aiScoreWeight = 0;

        /** 目标--关系类型 */
        public RelationType targetRelation = RelationType.All;
        /** 目标--单位类型 */
        public UnitType targetUnitType = UnitType.All;
        /** 目标--单位建筑类型 */
        public UnitBuildType targetBuildType = UnitBuildType.All;
        /** 目标--单位士兵类型 */
        public UnitSoliderType targetSoliderType = UnitSoliderType.All;
        /** 目标--单位空间类型 */
        public UnitSpaceType targetSpaceType = UnitSpaceType.All;
        /** 目标--单位职业类型 */
        public UnitProfessionType targetProfessionType = UnitProfessionType.All;

        /** 技能类别，默认0非辅助技能 */
        public int isLinkSkill = 0;
        /** 辅助技能序列列表 */
        public Dictionary<int,int> skillLinkList = new Dictionary<int,int>();

        /// <summary>
        /// AI打分配置--仇恨值
        /// </summary>
        public AIScoreConfig aiConfigHatred
        {
            get
            {
                return Game.config.aIScoreAttackHatred.GetConfig(aiScoreAttackHatred);
            }
        }


        /// <summary>
        /// AI打分配置--单位类型
        /// </summary>
        public AIScoreConfig aiConfigType
        {
            get
            {
                return Game.config.aIScoreUnitTypeCofnigRenader.GetConfig(aiScoreUnitType);
            }
        }


        /// <summary>
        /// AI打分配置--距离
        /// </summary>
        public AIScoreConfig aiConfigDistance
        {
            get
            {
                return Game.config.aIScoreDistance.GetConfig(aiScoreDistance);
            }
        }


        /// <summary>
        /// AI打分配置--权重
        /// </summary>
        public AIScoreWeightConfig aiConfigWeight
        {
            get
            {
                return Game.config.aIScoreWeight.GetConfig(aiScoreWeight);
            }
        }

    }
}
