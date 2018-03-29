using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 召唤分身
    /// </summary>
    public class SkillActionCreateMirror : SkillAction
    {
        private SkillActionConfigMirror actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigMirror)config.config;
            endType = StoryActionEndType.Call;
        }

        /// <summary>
        /// 位移距离
        /// </summary>
        //private float       moveEndDis     = 0;
        /// <summary>
        /// 位移速度
        /// </summary>
        //private float       moveSpeed      = 0.5F;
        /// <summary>
        /// 启动位置
        /// </summary>
        //private Vector3     StartPos;
        /// <summary>
        /// 总移动时间
        /// </summary>
        //private float       moveAllTime    = 0;
        /// <summary>
        /// 移动朝向
        /// </summary>
        //private Quaternion  quaternion;
        /// <summary>
        /// 召唤物
        /// </summary>
        //private UnitAgent  mirrorUnitAgent;

        protected override void OnStart()
        {
            UnitConfig unitConfig = Game.config.unit.GetConfig(actionConfig.unitId);
            //生成模型
            //mObject = room.clientRes.GetGameObjectInstall(avatarConfig.model);
            GameObject mObject = WarPhotonRoom.CloneUnit(warSkill.actionUnitAgent.unitData.LegionId, warSkill.actionUnitAgent.uid, unitConfig, warSkill.actionUnitAgent.position + warSkill.actionUnitAgent.forward * actionConfig.disFromSelf, warSkill.actionUnitAgent.rotation, actionConfig.life, UnitProduceType.Clone, actionConfig.weaponId);

            UnitAgent mirrorUnitAgent = mObject.GetComponent<UnitAgent>();
            if (mirrorUnitAgent != null)
            {
                mirrorUnitAgent.unitData.isDeathWithMain = actionConfig.isDeathWithMain;
                mirrorUnitAgent.unitData.attackPer = actionConfig.attackPer;
                mirrorUnitAgent.unitData.hitPer = actionConfig.hitPer;
                if (mirrorUnitAgent.animatorManager != null)
                {
                    // 更换材质球
                    GameObject model = mirrorUnitAgent.animatorManager.gameObject;
                    if (model != null)
                    {
                        SkinnedMeshRenderer[] smrlist = model.GetComponentsInChildren<SkinnedMeshRenderer>();
                        for (int i = 0; i < smrlist.Length; i++)
                        {
                            if (smrlist[i] != null)
                            {
                                string str = smrlist[i].sharedMaterial.name.Replace(" (Instance)","");
                                smrlist[i].sharedMaterial = Game.asset.GetMaterial("Materials/Hero/" + str + "_Clone");
                            }
                        }
                    }
                }
            }
            End();
            // 记录初始位置
            //StartPos = warSkill.actionUnitAgent.position;
            //moveEndDis = actionConfig.disFromSelf;
            //moveSpeed = actionConfig.moveSpeed * 0.1F;
            //moveAllTime = moveEndDis / moveSpeed + room.LTime.time;
            //quaternion = Quaternion.LookRotation(warSkill.actionUnitAgent.forward);
        }

        //protected override void OnTick()
        //{
        //    // 移动中
        //    if (mirrorUnitAgent != null)
        //    {
        //        Quaternion _temQuaternion = Quaternion.LookRotation(mirrorUnitAgent.forward);
        //        mirrorUnitAgent.Move(_temQuaternion, mirrorUnitAgent.unitData.prop.MovementSpeed * moveSpeed, false);
        //        if (room.LTime.time > moveAllTime || Vector3.Distance(mirrorUnitAgent.position, StartPos) >= moveEndDis)
        //        {
        //            End();
        //        }
        //    }
        //    else
        //    {
        //        End();
        //    }
        //}

    }
}