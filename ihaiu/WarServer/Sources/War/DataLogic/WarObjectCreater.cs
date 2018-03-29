using Assets.Scripts.Common;
using Games.PB;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 7:51:00 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗对象构建器 */
    public class WarObjectCreater : AbstractRoomObject
    {
        public WarObjectCreater(WarRoom room)
        {
            this.room = room;
        }

        /** 创建势力组 */
        public LegionGroupData CreateLegionGroup(int legionGroupId)
        {
            LegionGroupData group = ClassObjPool<LegionGroupData>.Get();
            group.groupId = legionGroupId;
            return group;
        }

        /** 创建势力 */
        public LegionData CreateLegion(WarSyncCreateLegion data)
        {
            LegionData legion = ClassObjPool<LegionData>.Get();
            legion.SetLegionData(room);
            legion.legionId     = data.legionId;
            legion.type         = (LegionType) data.legionType;
            legion.neutralBuildChangeType = (LegionBuildChangType)data.neutralBuildChangeType;
            legion.isRobot      = data.isRobot;
            legion.robotId           = data.robotId;
            legion.roleInfo.roleId      = data.roleId;
            legion.roleInfo.roleName    = data.roleName;
            //legion.roleInfo.clanId      = data.clanId;
            //legion.roleInfo.clanName    = data.clanName;

            return legion;
        }


        /** 创建势力 */
        public LegionData CreateLegion(WarEnterLegionData data)
        {
            LegionData legion = ClassObjPool<LegionData>.Get();
            legion.SetLegionData(room);
            legion.legionId = data.legionId;
            legion.type = (LegionType)data.legionType;
            legion.neutralBuildChangeType = (LegionBuildChangType)data.neutralBuildChangeType;
            legion.isRobot = data.isRobot;
            legion.robotId = data.robotId;
            legion.roleInfo.roleId = data.roleInfo.roleId;
            legion.roleInfo.roleName = data.roleInfo.roleName;
            //legion.roleInfo.clanId      = data.clanId;
            //legion.roleInfo.clanName    = data.clanName;

            return legion;
        }

        /** 创建单位 */
        public UnitData CreateUnit(WarSyncCreateUnit data)
        {
            UnitData unit = ClassObjPool<UnitData>.Get();
            unit.uid        = data.uid;
            unit.unitId     = data.unitId;
            unit.unitLevel  = data.unitLevel;
            unit.legionId   = data.legionId;
            unit.position = data.position.ProtoToVector3();
            unit.rotation = data.rotation.ProtoToVector3();


            UnitConfig unitConfig = Game.config.unit.GetConfig(data.unitId);
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(data.unitId, data.unitLevel);
            unit.prop.AppProps(unitLevelConfig.protoAttachData, true);

            unit.unitConfig             = unitConfig;
            unit.unitLevelConfig        = unitLevelConfig;
            unit.avatarConfig           = Game.config.avatar.GetConfig(unitConfig.avatarId);
            unit.unitRadius         = unitConfig.radius;
            unit.unitFlyHeight      = unitConfig.flyHeight;
            unit.unitType           = unitConfig.unitType;
            unit.buildType          = unitConfig.buildType;
            unit.soliderType        = unitConfig.soliderType;
            unit.professionType     = unitConfig.professionType;
            unit.spaceType          = unitConfig.spaceType;
            unit.name = unitConfig.name  + " Lv"+ unitLevelConfig.level;
            if(unit.unitType == UnitType.Hero)
            {
                unit.name = room.sceneData.GetLegion(unit.legionId).roleName;
            }

            bool hasSkill300205 = false;
            bool hasSkill300206 = false;
            for (int i = 0; i < data.skills.Count; i++)
            {
                WarSyncCreateSkill skillMsg = data.skills[i];
                SkillController skillcontroller = new SkillController(room, unit);
                skillcontroller.skillUid = skillMsg.skillUid;
                skillcontroller.skillId = skillMsg.skillId;
                skillcontroller.skillLevel = skillMsg.skillLevel;
                if (skillcontroller.skillId > 0)
                {
                    skillcontroller.skillConfig = Game.config.skill.GetConfig(skillcontroller.skillId);
                    skillcontroller.skillLevelConfig = Game.config.skillLevel.GetConfigs(skillcontroller.skillId, skillcontroller.skillLevel);
                    // 被动附加属性
                    if (skillcontroller.skillLevelConfig.attributePack > 0)
                    {
                        AttributePackConfig attributePackConfig = Game.config.attributePack.GetConfig(skillcontroller.skillLevelConfig.attributePack);
                        if (attributePackConfig != null)
                        {
                            List<Prop> proplist = new List<Prop>()
                            {
                                Prop.Create(PropId.Hp,attributePackConfig.hp),
                                Prop.Create(PropId.HpMax,attributePackConfig.hpMax),
                                Prop.Create(PropId.Damage,attributePackConfig.damage),
                                Prop.Create(PropId.PhysicalDefence,attributePackConfig.physicalDefence),
                                Prop.Create(PropId.MagicDefence,attributePackConfig.magicDefence),
                                Prop.Create(PropId.PhysicalAttack,attributePackConfig.physicalAttack),
                                Prop.Create(PropId.MagicAttack,attributePackConfig.magicAttack),
                                Prop.Create(PropId.HpRecover,attributePackConfig.hpRecover),
                                Prop.Create(PropId.EnergyRecover,attributePackConfig.energyRecover),
                                Prop.Create(PropId.RadarRadius,attributePackConfig.radarRadius),
                                Prop.Create(PropId.AttackRadius,attributePackConfig.attackRadius),
                                Prop.Create(PropId.AttackSpeed,attributePackConfig.attackSpeed),
                                Prop.Create(PropId.MovementSpeed,attributePackConfig.movementSpeed)
                            };
                            PropAttachData propAttachData =  new PropAttachData(proplist);
                            unit.prop.AppProps(propAttachData, true);
                        }
                    }
                }
                unit.AddSkill(skillcontroller);

                if(skillMsg.skillId == 300205)
                {
                    hasSkill300205 = true;
                }

                if (skillMsg.skillId == 300206)
                {
                    hasSkill300206 = true;
                }
            }


            if (hasSkill300206)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300202);
            }

            if (hasSkill300205)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300201);
            }

            if(unit.unitType == UnitType.Solider)
            {

                float totalWeight = 0;
                for (int i = 0; i < unitLevelConfig.aiSoliders.Count; i++)
                {
                    AISoliderSkill ai = new AISoliderSkill();
                    ai.unit = unit;
                    ai.aiSoliderConfig = Game.config.aISolider.GetConfig(unitLevelConfig.aiSoliders[i]);
                    ai.skillController = unit.GetSkill(ai.aiSoliderConfig.skillId);
                    ai.weightMinVal = totalWeight;
                    totalWeight += ai.aiSoliderConfig.weight;
                    ai.weightMaxVal = totalWeight;
                    unit.aiSoliderSkillList.Add(ai);
                    if (ai.skillController.skillId == unit.SkillA.skillId)
                    {
                        unit.attackAiSoliderSkill = ai;
                    }
                }

                foreach(AISoliderSkill ai in unit.aiSoliderSkillList)
                {
                    ai.weightMin = ai.weightMinVal / totalWeight * 100;
                    ai.weightMax = ai.weightMaxVal / totalWeight * 100;
                }
            }

            return unit;
        }



        /** 创建单位 */
        public UnitData CreateUnit(int uid, int legionId, WarEnterUnitData enterUnitData, Vector3 position, Vector3 rotation)
        {
            UnitData unit = ClassObjPool<UnitData>.Get();
            unit.uid = uid;
            unit.unitId = enterUnitData.unitId;
            unit.unitLevel = enterUnitData.unitLevel;
            unit.legionId = legionId;
            unit.position = position;
            unit.rotation = rotation;


            UnitConfig unitConfig = Game.config.unit.GetConfig(unit.unitId);
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(unit.unitId, unit.unitLevel);
            unit.prop.AppProps(unitLevelConfig.protoAttachData, true);

            unit.unitConfig = unitConfig;
            unit.unitLevelConfig = unitLevelConfig;
            unit.avatarConfig = Game.config.avatar.GetConfig(unitConfig.avatarId);
            unit.unitRadius = unitConfig.radius;
            unit.unitFlyHeight = unitConfig.flyHeight;
            unit.unitType = unitConfig.unitType;
            unit.buildType = unitConfig.buildType;
            unit.soliderType = unitConfig.soliderType;
            unit.professionType = unitConfig.professionType;
            unit.spaceType = unitConfig.spaceType;

            unit.name = unitConfig.name + " Lv" + unitLevelConfig.level;
            if (unit.unitType == UnitType.Hero)
            {
                unit.name = room.sceneData.GetLegion(unit.legionId).roleName;
            }

            bool hasSkill300205 = false;
            bool hasSkill300206 = false;
            for (int i = 0; i < enterUnitData.skillList.Count; i++)
            {
                WarEnterSkillData skillMsg = enterUnitData.skillList[i];
                SkillController skillcontroller = new SkillController(room, unit);
                skillcontroller.skillUid = room.SKILL_UID;
                skillcontroller.skillId = skillMsg.skillId;
                skillcontroller.skillLevel = skillMsg.skillLevel;
                if (skillcontroller.skillId > 0)
                {
                    skillcontroller.skillConfig = Game.config.skill.GetConfig(skillcontroller.skillId);
                    skillcontroller.skillLevelConfig = Game.config.skillLevel.GetConfigs(skillcontroller.skillId, skillcontroller.skillLevel);
                    // 被动附加属性
                    if (skillcontroller.skillLevelConfig.attributePack > 0)
                    {
                        AttributePackConfig attributePackConfig = Game.config.attributePack.GetConfig(skillcontroller.skillLevelConfig.attributePack);
                        if (attributePackConfig != null)
                        {
                            List<Prop> proplist = new List<Prop>()
                            {
                                Prop.Create(PropId.Hp,attributePackConfig.hp),
                                Prop.Create(PropId.HpMax,attributePackConfig.hpMax),
                                Prop.Create(PropId.Damage,attributePackConfig.damage),
                                Prop.Create(PropId.PhysicalDefence,attributePackConfig.physicalDefence),
                                Prop.Create(PropId.MagicDefence,attributePackConfig.magicDefence),
                                Prop.Create(PropId.PhysicalAttack,attributePackConfig.physicalAttack),
                                Prop.Create(PropId.MagicAttack,attributePackConfig.magicAttack),
                                Prop.Create(PropId.HpRecover,attributePackConfig.hpRecover),
                                Prop.Create(PropId.EnergyRecover,attributePackConfig.energyRecover),
                                Prop.Create(PropId.RadarRadius,attributePackConfig.radarRadius),
                                Prop.Create(PropId.AttackRadius,attributePackConfig.attackRadius),
                                Prop.Create(PropId.AttackSpeed,attributePackConfig.attackSpeed),
                                Prop.Create(PropId.MovementSpeed,attributePackConfig.movementSpeed)
                            };
                            PropAttachData propAttachData =  new PropAttachData(proplist);
                            unit.prop.AppProps(propAttachData, true);
                        }
                    }
                }
                unit.AddSkill(skillcontroller);

                if (skillMsg.skillId == 300205)
                {
                    hasSkill300205 = true;
                }

                if (skillMsg.skillId == 300206)
                {
                    hasSkill300206 = true;
                }
            }


            if (hasSkill300206)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300202);
            }

            if (hasSkill300205)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300201);
            }

            if (unit.unitType == UnitType.Solider)
            {

                float totalWeight = 0;
                for (int i = 0; i < unitLevelConfig.aiSoliders.Count; i++)
                {
                    AISoliderSkill ai = new AISoliderSkill();
                    ai.unit = unit;
                    ai.aiSoliderConfig = Game.config.aISolider.GetConfig(unitLevelConfig.aiSoliders[i]);
                    ai.skillController = unit.GetSkill(ai.aiSoliderConfig.skillId);
                    ai.weightMinVal = totalWeight;
                    totalWeight += ai.aiSoliderConfig.weight;
                    ai.weightMaxVal = totalWeight;
                    unit.aiSoliderSkillList.Add(ai);
                    if (ai.skillController.skillId == unit.SkillA.skillId)
                    {
                        unit.attackAiSoliderSkill = ai;
                    }
                }

                foreach (AISoliderSkill ai in unit.aiSoliderSkillList)
                {
                    ai.weightMin = ai.weightMinVal / totalWeight * 100;
                    ai.weightMax = ai.weightMaxVal / totalWeight * 100;
                }
            }

            return unit;
        }

        /** 创建Halo单位 */
        public UnitData CreateHaloUnit(int uid, int legionId, Vector3 position, Vector3 rotation)
        {
            UnitData unit = ClassObjPool<UnitData>.Get();
            unit.uid = uid;
            unit.legionId = legionId;
            unit.position = position;
            unit.rotation = rotation;
            unit.unitType = UnitType.Halo;
            unit.room = room;
            unit.unitData = unit;
            room.sceneData.AddUnit(unit);

            GameObject go = new GameObject("HaloUnit");
            UnitAgent unitAgent = go.AddComponent<UnitAgent>();
            unitAgent.room = room;
            unitAgent.position = position;
            unitAgent.rotation = rotation;
            unitAgent.Init(unit);
            room.clientSceneView.AddUnit(unitAgent);

            return unit;
        }
    }
}
