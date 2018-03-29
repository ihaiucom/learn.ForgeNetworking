using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 4:46:36 PM
*  @Description:    单位数据--技能
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {
        // 开关技能
        public Dictionary<int, int>         switchSkillDic      = new Dictionary<int, int>();
        public List<int>                    switchSkillList     = new List<int>();
        public Dictionary<int, int>         switchSkillBuffDic  = new Dictionary<int, int>();


        #region 士兵技能
        public List<AISoliderSkill> aiSoliderSkillList = new List<AISoliderSkill>();
        public AISoliderSkill attackAiSoliderSkill;
        #endregion

        #region 技能
        /** 技能列表 */
        private List<SkillController> skillList = new List<SkillController>();
        private Dictionary<int, SkillController> skillDict = new Dictionary<int, SkillController>();



        /** 技能数量 */
        public int SkillCount
        {
            get
            {
                return skillList.Count;
            }
        }

        /** 添加技能 */
        public void AddSkill(SkillController skill)
        {
            skillList.Add(skill);
            skillDict.Add(skill.skillId, skill);
        }

        /// <summary>
        /// 获取技能序号
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public int GetSkillIndex(int skillId)
        {
            return skillList.FindIndex(m => m.skillInfoConfig.skillId == skillId);
        }

        /** 获取技能 */
        public SkillController GetSkill(int skillId)
        {
            if (skillDict.ContainsKey(skillId))
                return skillDict[skillId];
            return null;
        }

        #region buff数据

        private Dictionary<int, HaloBuff> poolHaloBuff = new Dictionary<int, HaloBuff>();

        public HaloBuff GetHaloBuff(int id)
        {
            if (id == 0)
            {
                return null;
            }
            if (poolHaloBuff.ContainsKey(id))
            {
                return poolHaloBuff[id];
            }
            HaloBuff result = Game.config.skillEffect.GetHaloBuffConfig(id);
            poolHaloBuff.Add(id, result);
            return result;
        }
        #endregion

        /** 获取技能，根据index */
        public SkillController GetSkillByIndex(int index)
        {
            if (index >= SkillCount) return null;
            return skillList[index];
        }

        // 替换普攻技能
        public void ChangeNormalSkillInf(int skillid)
        {
            if (skillid > 0)
            {
                SkillController skill0 = GetSkillByIndex(0);
                skill0.ChangeSkillConfig(skillid);
            }
        }

        /** 获取技能0 普攻 */
        public SkillController SkillA
        {
            get
            {
                return GetSkillByIndex(0);
            }
        }

        /** 获取技能1 Q */
        public SkillController SkillQ
        {
            get
            {
                return GetSkillByIndex(1);
            }
        }

        /** 获取技能2 W */
        public SkillController SkillW
        {
            get
            {
                return GetSkillByIndex(2);
            }
        }
        /** 获取技能3 E */
        public SkillController SkillE
        {
            get
            {
                return GetSkillByIndex(3);
            }
        }

        /// <summary>
        /// 使用被动技能--出生时触发
        /// </summary>
        public void UsePassiveSkillBirth()
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                SkillController skillController = skillList[i];
                if (skillController.skillConfig != null
                    && skillController.skillInfoConfig != null
                    && skillController.skillInfoConfig.activation == Activation.Passive)
                {
                    skillController.Use(this);
                }
                //if (skillController.skillConfig != null
                //&& skillController.skillConfig.isPassive
                //&& skillController.skillConfig.passiveType == SkillPassiveType.Birth)
                //if (skillController.skillConfig != null
                //    && skillController.skillBass != null
                //    && skillController.skillBass.skillActivation.activation == Activation.Passive)
                //{
                //    skillController.Use(this);
                //}
            }
        }

        /// <summary>
        /// 攻击CD
        /// AI使用
        /// </summary>
        public float attackCD = 0;



        /// <summary>
        /// 是否有普攻
        /// </summary>
        public bool HasSkillA
        {
            get
            {
                return SkillA != null && SkillA.HasConfig;
            }
        }
        #endregion





        //#region Skill
        ///** 技能启动 */
        //public void SkillStart()
        //{

        //}

        ///** 技能结束 */
        //public void SkillEnd()
        //{

        //}

        ///** 技能停止 */
        //public void SkillKill()
        //{

        //}
        //#endregion

    }
}
