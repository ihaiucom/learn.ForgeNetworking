using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class WarSkill : AbstractRoomObject
    {
        public WarRoomSkillManager manager;

        public WarSkill(UnitAgent unit, SkillInfoConfig config, WarRoomSkillManager manager, int bulletCount = 0)
        {
            this.manager = manager;
            this.room = manager.room;
            actionUnitAgent = unit;
            actionUnitId = unit.uid;
            skillInfoConfig = config;
            actionList.Clear();
            actionListPasSave.Clear();
            int skilllv = 1;
            SkillController skillController = unit.unitData.GetSkill(config.skillId);
            if (skillController != null)
            {
                skilllv = skillController.skillLevel;
            }
            for (int i = 0; i < skillInfoConfig.SkillActionConfigList.Count; i++)
            {
                if (skillInfoConfig.SkillActionConfigList[i].activeLv <= skilllv && skillInfoConfig.SkillActionConfigList[i].blockLv >= skilllv)
                {
                    SkillAction item = SkillAction.CreateAction(skillInfoConfig.SkillActionConfigList[i],this);

                    // buff附加消耗子弹弹夹 子弹弹夹数量
                    if (bulletCount > 0 && skillInfoConfig.SkillActionConfigList[i].sKillActionType == SKillActionType.TriggerDamage)
                    {
                        SkillActionDamage _SkillActionDamage = (SkillActionDamage)item;
                        if (_SkillActionDamage.actionConfig.costBullet && unit.unitData.switchSkillList.Contains(config.skillId))
                        {
                            if (!unit.unitData.switchSkillDic.ContainsKey(config.skillId))
                            {
                                unit.unitData.switchSkillDic.Add(config.skillId, _SkillActionDamage.actionConfig.buffid);
                            }
                        }
                    }

                    actionList.Add(item);
                    actionListPasSave.Add(item);
                }
            }
            tigger = SkillTrigger.CreateTirgger(config.skillTriggerConfig, this);
        }
        /// <summary>
        /// 技能配置
        /// </summary>
        public SkillInfoConfig      skillInfoConfig;
        /// <summary>
        /// 技能朝向,没有攻击目标时适用
        /// </summary>
        public Quaternion           quaternion;
        /// <summary>
        /// 是英雄的普攻
        /// </summary>
        public bool                 normalSkill             = false;
        /// <summary>
        /// 自动选择目标
        /// </summary>
        public bool                 auto                    = false;
        /// <summary>
        /// 事件列表
        /// </summary>
        public List<SkillAction>    actionList              = new List<SkillAction>();
        /// <summary>
        /// 事件列表
        /// </summary>
        public List<SkillAction>    actionListPasSave       = new List<SkillAction>();
        /// <summary>
        /// 计时器
        /// </summary>
        public float                tickTime                = 0;
        /// <summary>
        /// 调用技能的单位
        /// </summary>
        public UnitAgent            actionUnitAgent         = null;
        /// <summary>
        /// 当前锁定的攻击目标
        /// </summary>
        public UnitData             attackUnit              = null;
        /// <summary>
        /// 当前攻击点
        /// </summary>
        public Vector3              attackPos               = Vector3.zero;
        /// <summary>
        /// 单位ID
        /// </summary>
        public int                  actionUnitId            = 0;

        public StoryStageStatus     status                  = StoryStageStatus.None;
        public SkillTrigger         tigger;


        // 启动触发器检查
        public void StartTigger()
        {
            status = StoryStageStatus.Tiggering;
            tigger.Start();
        }
        // 触发技能
        internal void OnTigger()
        {
            status = StoryStageStatus.Actioning;
            tickTime = 0;
            tigger.End();
            if (actionListPasSave != null && actionList.Count != actionListPasSave.Count)
            {
                for (int i = 0; i < actionListPasSave.Count; i++)
                {
                    actionList.Add(actionListPasSave[i]);
                    actionList[i].status = StoryActionStatus.None;
                }
            }
            manager.OnTiggerSkill(this);
        }
        /// <summary>
        /// 行为
        /// </summary>
        public void TickAction()
        {
            tickTime += Time.deltaTime;
            
            for (int i = actionList.Count - 1; i >= 0; i--)
            {
                actionList[i].Tick();
            }
        }
        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="action"></param>
        public void OnActionFinish(SkillAction action)
        {
            if (actionList.Contains(action))
            {
                actionList.Remove(action);
            }

            if (actionList.Count <= 0)
            {
                if (actionUnitAgent != null)
                {
                    actionUnitAgent.unitData.invincible = false;
                    actionUnitAgent.unitData.isSkillAttack = false;
                    room.clientOperationUnit.OnSkillEnterCD();
                }
                manager.OnFinish(this);
            }
        }
    }
}