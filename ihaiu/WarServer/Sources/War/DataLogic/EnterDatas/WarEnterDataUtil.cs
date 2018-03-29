using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/25/2017 6:41:51 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarEnterDataUtil
    {
        /// <summary>
        /// 副本（单人，多人）
        /// </summary>
        /// <param name="pveData"></param>
        /// <returns></returns>
        public static WarEnterData GeneratePVE(WarEnterPVEData pveData)
        {
            WarEnterGroupData       group;
            WarEnterLegionData      legion;
            WarEnterMainbaseData    mainbase;

            pveData.legionList.Sort((WarEnterPVELegionData a, WarEnterPVELegionData b) =>
            {
                return a.roleInfo.roleId - b.roleInfo.roleId;
            });


            StageConfig stageConfig = Game.config.stage.GetConfig(pveData.stageId);

            WarEnterData enterData = new Wars.WarEnterData();
            enterData.stageId = pveData.stageId;



            // Group
            Dictionary<int, WarEnterGroupData> groupDict = new Dictionary<int, WarEnterGroupData>();
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                if (groupDict.ContainsKey(legionConfig.groupId)) continue;

                group = new WarEnterGroupData();
                group.groupId = legionConfig.groupId;
                enterData.groupList.Add(group);
                groupDict.Add(group.groupId, group);

            }

            // Legion
            Dictionary<int, WarEnterLegionData> legionDict = new Dictionary<int, WarEnterLegionData>();
            int playerIndex = 0;
            int playerCount = pveData.legionList.Count;
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                group = groupDict[legionConfig.groupId];

                legion = new WarEnterLegionData();
                legion.roleInfo.roleId = -1;
                legion.roleInfo.roleName = legionConfig.describe;
                legion.legionId = legionConfig.legionId;
                legion.legionType = legionConfig.legionType;
                legion.neutralBuildChangeType = legionConfig.neutralBuildChangeType;
                legion.robotId = legionConfig.ai;
                legion.isRobot = true;
                legion.regionId = legionConfig.regionId;

                if (legion.legionType == LegionType.Player)
                {
                    if(playerIndex < playerCount)
                    {
                        WarEnterPVELegionData pveLegion = pveData.legionList[playerIndex];
                        legion.hero = pveLegion.hero;
                        legion.towerList = pveLegion.towerList;
                        legion.roleInfo = pveLegion.roleInfo;
                        legion.isRobot = false;

                        if(pveData.ownRoleId == legion.roleInfo.roleId)
                        {
                            enterData.ownLegionId = legion.legionId;
                        }

                        if(playerIndex == 0)
                        {
                            enterData.hostLegionId = legion.legionId;
                        }

                        playerIndex++;
                    }
                    else
                    {
                        continue;
                    }
                }

                // 技能列表内添加辅助技能信息，辅助技能等级和主技能等级保持一致
                List<WarEnterSkillData> skillLinkList = new List<WarEnterSkillData>();
                for (int i = 0; i < legion.hero.skillList.Count; i++)
                {
                    int id = legion.hero.skillList[i].skillId;
                    int lv = legion.hero.skillList[i].skillLevel;
                    SkillConfig skillConfig = Game.config.skill.GetConfig(id);
                    if (skillConfig.isLinkSkill == 0)
                    {
                        if (skillConfig.skillLinkList.Count > 0)
                        {
                            for (int k = 0; k < skillConfig.skillLinkList.Count; k++)
                            {
                                if (skillConfig.skillLinkList[k] > 0)
                                {
                                    WarEnterSkillData _WarEnterSkillData = new WarEnterSkillData();
                                    _WarEnterSkillData.skillId = skillConfig.skillLinkList[k];
                                    _WarEnterSkillData.skillLevel = lv;
                                    skillLinkList.Add(_WarEnterSkillData);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < skillLinkList.Count; i++)
                {
                    legion.hero.skillList.Add(skillLinkList[i]);
                }

                group.legionList.Add(legion);
                legionDict.Add(legion.legionId, legion);
            }
            


            // Mainbase
            foreach (StageMainbaseConfig mainbaseConfig in stageConfig.mainbaseList)
            {
                mainbase = new WarEnterMainbaseData();
                mainbase.legionId = mainbaseConfig.legionId;
                mainbase.position = mainbaseConfig.position;
                mainbase.rotation = mainbaseConfig.rotation;
                mainbase.unit.unitId = mainbaseConfig.unit.unitId;
                mainbase.unit.unitLevel = mainbaseConfig.unit.unitLevel;
                mainbase.unit.name = mainbaseConfig.unit.name;

                legion = legionDict[mainbase.legionId];

                if (stageConfig.stageType == StageType.Dungeon &&  legion.legionType == LegionType.Player)
                {
                    WarEnterPVELegionData pveLegion = pveData.legionList[0];
                    mainbase.unit = pveLegion.mianbaseUnit;

                }
                // 更新主基地模型
                mainbase.unit.avatarId = mainbaseConfig.unit.avatarId;

                UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(mainbase.unit.unitId, mainbase.unit.unitLevel);
                if (unitLevelConfig == null)
                {
                    Loger.LogErrorFormat("没有找到单位等级配置UnitLevelConfig unitId={0}, unitLevel={1}", mainbase.unit.unitId, mainbase.unit.unitLevel);
                }
                else
                {
                    mainbase.unit.skillList.Clear();
                    foreach (WarEnterSkillData skillData in unitLevelConfig.skillList)
                    {
                        WarEnterSkillData skill = skillData.Clone();
                        skill.skillLevel = 1;
                        mainbase.unit.skillList.Add(skill);
                    }
                }

                enterData.mainbaseList.Add(mainbase);
            }

            enterData.monsterLiveTime = pveData.monsterLifeTime;
            enterData.monsterAllCount = pveData.monsterAllCount;
            enterData.activityType = pveData.activityType;
            return enterData;
        }


        /// <summary>
        /// 单人副本
        /// </summary>
        /// <param name="stageId"></param>
        /// <param name="roleInfo"></param>
        /// <param name="hero"></param>
        /// <param name="mianbaseUnit"></param>
        /// <param name="towerList"></param>
        /// <returns></returns>
        public static WarEnterData GenerateDungeon(int stageId, RoleInfo roleInfo, WarEnterUnitData hero, WarEnterUnitData mianbaseUnit , List<WarEnterUnitData> towerList)
        {
            WarEnterGroupData group;
            WarEnterLegionData legion;
            WarEnterMainbaseData mainbase;

            StageConfig stageConfig = Game.config.stage.GetConfig(stageId);
            WarEnterData enterData = new Wars.WarEnterData();
            enterData.stageId = stageId;



            // Group
            Dictionary<int, WarEnterGroupData> groupDict = new Dictionary<int, WarEnterGroupData>();
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                if (groupDict.ContainsKey(legionConfig.groupId)) continue;

                group = new WarEnterGroupData();
                group.groupId = legionConfig.groupId;
                enterData.groupList.Add(group);
                groupDict.Add(group.groupId, group);

            }

            // Legion
            Dictionary<int, WarEnterLegionData> legionDict = new Dictionary<int, WarEnterLegionData>();
            WarEnterLegionData player = null;
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                group = groupDict[legionConfig.groupId];

                legion = new WarEnterLegionData();
                legion.roleInfo.roleId      = -1;
                legion.roleInfo.roleName    = legionConfig.describe;
                legion.legionId             = legionConfig.legionId;
                legion.legionType           = legionConfig.legionType;
                legion.robotId              = legionConfig.ai;
                legion.isRobot              = true;
                legion.regionId             = legionConfig.regionId;
                group.legionList.Add(legion);
                legionDict.Add(legion.legionId, legion);

                if(legion.legionType == LegionType.Player)
                {
                    player = legion;
                }
            }

            player.roleInfo     = roleInfo;
            player.hero         = hero;
            player.towerList    = towerList;



            // 技能列表内添加辅助技能信息，辅助技能等级和主技能等级保持一致
            List<WarEnterSkillData> skillLinkList = new List<WarEnterSkillData>();
            for (int i = 0; i < player.hero.skillList.Count; i++)
            {
                int id = player.hero.skillList[i].skillId;
                int lv = player.hero.skillList[i].skillLevel;
                SkillConfig skillConfig = Game.config.skill.GetConfig(id);
                if (skillConfig.isLinkSkill == 0)
                {
                    if (skillConfig.skillLinkList.Count > 0)
                    {
                        for (int k = 0; k < skillConfig.skillLinkList.Count; k++)
                        {
                            if (skillConfig.skillLinkList[k] > 0)
                            {
                                WarEnterSkillData _WarEnterSkillData = new WarEnterSkillData();
                                _WarEnterSkillData.skillId = skillConfig.skillLinkList[k];
                                _WarEnterSkillData.skillLevel = lv;
                                skillLinkList.Add(_WarEnterSkillData);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < skillLinkList.Count; i++)
            {
                player.hero.skillList.Add(skillLinkList[i]);
            }

            WarEnterMainbaseData playerMainbase = null;

            // Mainbase
            foreach (StageMainbaseConfig mainbaseConfig in stageConfig.mainbaseList)
            {
                mainbase = new WarEnterMainbaseData();
                mainbase.legionId = mainbaseConfig.legionId;
                mainbase.position = mainbaseConfig.position;
                mainbase.rotation = mainbaseConfig.rotation;
                mainbase.unit.unitId = mainbaseConfig.unit.unitId;
                mainbase.unit.avatarId = mainbaseConfig.unit.avatarId;
                mainbase.unit.unitLevel = mainbaseConfig.unit.unitLevel;
                mainbase.unit.name = mainbaseConfig.unit.name;


                if (mainbase.legionId == player.legionId)
                {
                    playerMainbase = mainbase;
                }
                else
                {
                    UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(mainbase.unit.unitId, mainbase.unit.unitLevel);
                    if (unitLevelConfig == null)
                    {
                        Loger.LogErrorFormat("没有找到单位等级配置UnitLevelConfig unitId={0}, unitLevel={1}", mainbase.unit.unitId, mainbase.unit.unitLevel);
                    }
                    else
                    {
                        foreach (WarEnterSkillData skillData in unitLevelConfig.skillList)
                        {
                            mainbase.unit.skillList.Add(skillData.Clone());
                        }
                    }
                }

                enterData.mainbaseList.Add(mainbase);
            }

            playerMainbase.unit = mianbaseUnit;


            enterData.ownLegionId = player.legionId;
            enterData.hostLegionId = player.legionId;
            return enterData;
        }

        public static WarEnterData GeneratePVPLadder(WarEnterPVPLadderData ladderData)
        {
            WarEnterGroupData       group;
            WarEnterLegionData      legion;

            StageConfig stageConfig = Game.config.stage.GetConfig(ladderData.stageId);
            WarEnterData enterData = new Wars.WarEnterData();
            enterData.stageId = ladderData.stageId;

            // Group
            Dictionary<int, WarEnterGroupData> groupDict = new Dictionary<int, WarEnterGroupData>();
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                if (groupDict.ContainsKey(legionConfig.groupId)) continue;

                group = new WarEnterGroupData();
                group.groupId = legionConfig.groupId;
                enterData.groupList.Add(group);
                groupDict.Add(group.groupId, group);
            }

            // Legion
            Dictionary<int, WarEnterLegionData> legionDict = new Dictionary<int, WarEnterLegionData>();
            WarEnterLegionData player = null;
            int legionId = 100000;
            foreach (StageLegionConfig legionConfig in stageConfig.legionList)
            {
                group = groupDict[legionConfig.groupId];
                legion = new WarEnterLegionData();
                legion.roleInfo.roleId = -1;
                legion.roleInfo.roleName = legionConfig.describe;
                legion.legionId = legionConfig.legionId;
                legion.legionType = legionConfig.legionType;
                legion.robotId = legionConfig.ai;
                legion.isRobot = true;
                legion.regionId = legionConfig.regionId;
                group.legionList.Add(legion);
                legionDict.Add(legion.legionId, legion);
                // 取小的势力id作为玩家自己 的势力
                if (legionId > legionConfig.legionId)
                {
                    if (player != null)
                    {
                        enterData.otherLegionId = player.legionId;
                    }
                    player = legion;
                    legionId = legionConfig.legionId;
                }
                else
                {
                    enterData.otherLegionId = legion.legionId;
                }
            }
            legionDict[player.legionId].isRobot = false;

            enterData.ownLegionId = player.legionId;
            enterData.hostLegionId = player.legionId;

            // WarEnterUnitData
            foreach (WarEnterPVELegionData pveLegionData in ladderData.legionList)
            {
                foreach (WarEnterUnitData warEnterUnitData in pveLegionData.towerList)
                {
                    enterData.initUnitList.Add(warEnterUnitData);
                }
                player.roleInfo = pveLegionData.roleInfo;
                player.hero = pveLegionData.hero;
                player.towerList = new List<WarEnterUnitData>();



                // 技能列表内添加辅助技能信息，辅助技能等级和主技能等级保持一致
                List<WarEnterSkillData> skillLinkList = new List<WarEnterSkillData>();
                for (int i = 0; i < player.hero.skillList.Count; i++)
                {
                    int id = player.hero.skillList[i].skillId;
                    int lv = player.hero.skillList[i].skillLevel;
                    SkillConfig skillConfig = Game.config.skill.GetConfig(id);
                    if (skillConfig.isLinkSkill == 0)
                    {
                        if (skillConfig.skillLinkList.Count > 0)
                        {
                            for (int k = 0; k < skillConfig.skillLinkList.Count; k++)
                            {
                                if (skillConfig.skillLinkList[k] > 0)
                                {
                                    WarEnterSkillData _WarEnterSkillData = new WarEnterSkillData();
                                    _WarEnterSkillData.skillId = skillConfig.skillLinkList[k];
                                    _WarEnterSkillData.skillLevel = lv;
                                    skillLinkList.Add(_WarEnterSkillData);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < skillLinkList.Count; i++)
                {
                    player.hero.skillList.Add(skillLinkList[i]);
                }

            }
            foreach (WarEnterPVELegionData pveLegionData in ladderData.ohterLegionList)
            {
                foreach (WarEnterUnitData warEnterUnitData in pveLegionData.towerList)
                {
                    enterData.initOhterUnitList.Add(warEnterUnitData);
                }
                foreach (var item in legionDict)
                {
                    if (item.Key != enterData.ownLegionId)
                    {
                        item.Value.roleInfo = pveLegionData.roleInfo;
                        item.Value.hero = pveLegionData.hero;
                        item.Value.towerList = pveLegionData.towerList;



                        // 技能列表内添加辅助技能信息，辅助技能等级和主技能等级保持一致
                        List<WarEnterSkillData> skillLinkList = new List<WarEnterSkillData>();
                        for (int i = 0; i < item.Value.hero.skillList.Count; i++)
                        {
                            int id = item.Value.hero.skillList[i].skillId;
                            int lv = item.Value.hero.skillList[i].skillLevel;
                            SkillConfig skillConfig = Game.config.skill.GetConfig(id);
                            if (skillConfig.isLinkSkill == 0)
                            {
                                if (skillConfig.skillLinkList.Count > 0)
                                {
                                    for (int k = 0; k < skillConfig.skillLinkList.Count; k++)
                                    {
                                        if (skillConfig.skillLinkList[k] > 0)
                                        {
                                            WarEnterSkillData _WarEnterSkillData = new WarEnterSkillData();
                                            _WarEnterSkillData.skillId = skillConfig.skillLinkList[k];
                                            _WarEnterSkillData.skillLevel = lv;
                                            skillLinkList.Add(_WarEnterSkillData);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < skillLinkList.Count; i++)
                        {
                            item.Value.hero.skillList.Add(skillLinkList[i]);
                        }
                    }
                }
            }

            return enterData;
        }

    }
}
