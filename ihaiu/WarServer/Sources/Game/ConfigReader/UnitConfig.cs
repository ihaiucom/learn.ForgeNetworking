using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 1:32:54 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /** 单位配置 */
    public class UnitConfig
    {
        /** 单位ID */
        public int                  unitId;
        /** 名称 */
        public string               name;
        /** 单位类型 */
        public UnitType             unitType;
        /** 建筑类型 */
        public UnitBuildType        buildType       = UnitBuildType.None;
        /** 士兵类型 */
        public UnitSoliderType      soliderType     = UnitSoliderType.None;
        /** 职业类型 */
        public UnitProfessionType   professionType = UnitProfessionType.None;
        /** 单位空间类型 */
        public UnitSpaceType        spaceType       = UnitSpaceType.Ground;
        /** [飞行单位有效] 飞行高度 */
        public float                flyHeight       = 0;
        /** 单位半径 */
        public float                radius          = 3;
        /** 单位RVO半径 */
        public float                rvoRadius       = 2;
        /** AvatarId */
        public int                  avatarId;
        /** 是否可以旋转 */
        public bool enableRotation      = true;
        /** 是否手动攻击 */
        public bool isManualAttack      = false;
        /** 技能列表 */
        //public List<int>            skillList = new List<int>();
        public  Dictionary<int,int> skillList       = new Dictionary<int, int>();
        /** 初始武器 */
        public int                  weaponDefaultId = 0;
        /** 死亡特效 */
        public Dictionary<int, string>               deathEffect    = new Dictionary<int, string>();

        public AvatarConfig GetAvatarConfig()
        {
            return Game.config.avatar.GetConfig(avatarId);
        }

        #region 获取技能特效路径
        public List<string> GetSkillEffectPath(Dictionary<int, int> skillList)
        {
            List<string> list = new List<string>();
            foreach (var kvp in skillList)
            {
                int skillId = kvp.Value;
                if (skillId != 0)
                {
                    SkillConfig skillConfig = Game.config.skill.GetConfig(skillId);
                    if (skillConfig != null)
                    {
                        List<string> list2 = GetSkillEffectPath(skillId);
                        if (list2 != null)
                        {
                            list.AddRange(list2);
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }
        public List<string> GetSkillEffectPath(int skillId, int skillLv = 1)
        {
            List<string> list = new List<string>();
            SkillBass skillBass = Game.config.skillEffect.GetSkillBassConfig(skillId);
            for (int i = 0; i < skillBass.actionEvent.actionEventList.Count; i++)
            {
                SkillTriggerEvent STE = skillBass.actionEvent.actionEventList[i];
                if (STE != null /*&& STE.activeLv <= skillLv && STE.blockLv >= skillLv*/)
                {
                    if (STE.effectPath != null && STE.effectPath.Length > 2)
                    {
                        list.Add(STE.effectPath);
                    }
                    if (STE.secondTarget != null && STE.secondTarget.secondEffectPath != null && STE.secondTarget.secondEffectPath.Length > 2)
                    {
                        list.Add(STE.secondTarget.secondEffectPath);
                    }
                    if (STE.projectile != null && STE.projectile.secondTarget != null && STE.projectile.secondTarget.secondEffectPath != null && STE.projectile.secondTarget.secondEffectPath.Length > 2)
                    {
                        list.Add(STE.projectile.secondTarget.secondEffectPath);
                    }
                }
            }
            if (list.Count > 0)
            {
                return list;
            }
            return null;
        }
        #endregion
    }
}
