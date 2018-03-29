using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 子弹
    /// </summary>
    public partial class SkillActionShotBullet : SkillAction
    {
        private SkillActionConfigProjectile actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigProjectile)config.config;
            //继承震屏参数
            actionConfig.bShakeEffect = config.bShakeEffect;
            actionConfig.shakeTime = config.shakeTime;
            actionConfig.shakeAmplitude = config.shakeAmplitude;
            actionConfig.shakeRange = config.shakeRange;

            if (actionConfig.projectileMoveMethod == ProjectileMoveMethod.AngledFromEach || actionConfig.maxProjectiles == 1)
            {
                endType = StoryActionEndType.Call;
            }
            else
            {
                endType = StoryActionEndType.DurationTime;
                durationTime = (actionConfig.maxProjectiles - 1) * actionConfig.createCD;
            }
        }
        float createTime;
        int createCount = 0;
        // 附加buff子弹弹夹数量
        protected int bulletBuffCount = 0;
        protected List<HaloBuff> haloBuffList = new List<HaloBuff>();

        protected override void OnStart()
        {

            // 获取攻击子弹弹夹buff
            int bbCount = 0;
            haloBuffList = new List<HaloBuff>();
            if (warSkill.actionUnitAgent.unitData.switchSkillList.Count > 0)
            {
                SkillActionConfigProjectile _SkillActionConfigProjectile = (SkillActionConfigProjectile)config.config;
                for (int ll = 0; ll < warSkill.actionUnitAgent.unitData.switchSkillList.Count; ll++)
                {
                    int skillid = warSkill.actionUnitAgent.unitData.switchSkillList[ll];
                    if (warSkill.actionUnitAgent.unitData.switchSkillBuffDic.ContainsKey(skillid))
                    {
                        if (warSkill.actionUnitAgent.unitData.switchSkillBuffDic[skillid] > 0)
                        {
                            if (warSkill.actionUnitAgent.unitData.switchSkillDic.ContainsKey(skillid))
                            {
                                int buffid = warSkill.actionUnitAgent.unitData.switchSkillDic[skillid];
                                HaloBuff haloBuff = warSkill.actionUnitAgent.unitData.GetHaloBuff(buffid);
                                haloBuffList.Add(haloBuff);
                                bbCount = room.clientOperationUnit.BulletUpdate(_SkillActionConfigProjectile.maxProjectiles);
                            }
                        }
                    }
                }
            }
            else
            {
                haloBuffList.Clear();
            }
            bulletBuffCount = bbCount;


            // 生成子弹，子弹即和技能无关，自行移动

            if (actionConfig.projectileMoveMethod == ProjectileMoveMethod.AngledFromEach)
            {
                Transform tf = warSkill.actionUnitAgent.shotTform;
                float angleUnit = 0;
                if (actionConfig.maxProjectiles > 1)
                {
                    angleUnit = actionConfig.angleFromEach / (actionConfig.maxProjectiles - 1);
                }
                float angle = -1 * (actionConfig.angleFromEach / 2);
                for (int k = 0; k < actionConfig.maxProjectiles; k++)
                {
                    Vector3 tl = tf.position + (Quaternion.AngleAxis(+angle, Vector3.up) * tf.forward * actionConfig.maxMoveDistance);
                    OnInitBullet(tl);
                    angle += angleUnit;
                }
                End();
            }
            else
            {
                if (warSkill.auto)
                {
                    if (warSkill.attackUnit != null)
                    {
                        warSkill.attackPos = warSkill.attackUnit.AnchorAttackbyPos;
                    }
                    else
                    {
                        if (actionConfig.attackRuleList.relationType == RelationType.Own || actionConfig.attackRuleList.relationType == RelationType.OwnAndFriendly)
                        {
                            warSkill.attackPos = warSkill.actionUnitAgent.unitData.AnchorAttackbyPos;
                        }
                        else
                        {
                            float endDis = actionConfig.maxMoveDistance;
                            warSkill.attackPos = warSkill.actionUnitAgent.shotTform.position + warSkill.actionUnitAgent.shotTform.forward * endDis;
                        }
                    }
                }
                else
                {
                    if (warSkill.attackUnit != null)
                    {
                        warSkill.attackPos = warSkill.attackUnit.AnchorAttackbyPos;
                    }
                }
                createCount = actionConfig.maxProjectiles;
                OnInitBullet(warSkill.attackPos);
                createTime = room.LTime.time;
            }
        }

        void OnInitBullet(Vector3 to)
        {
            if (warSkill.actionUnitAgent == null)
            {
                return;
            }
            string str = actionConfig.skillActionConfigDamage.attackEffectPath;
            if (bulletBuffCount > 0 && warSkill.normalSkill)
            {
                str = actionConfig.skillActionConfigDamage.attackEffectPath2;
            }
            SkillBullet item = manager.GetEffectFromPool(str, warSkill.actionUnitAgent.shotTform, false, actionConfig.skillActionConfigDamage.pathOffset);
            item.bullet = true;
            int skillLv = 1;
            SkillController skillController = warSkill.actionUnitAgent.unitData.GetSkill(warSkill.skillInfoConfig.skillId);
            if (skillController != null) skillLv = skillController.skillLevel;
            if (bulletBuffCount > 0 && warSkill.normalSkill)
            {
                bulletBuffCount--;
            }
            else
            {
                haloBuffList = null;
            }
            item.OnStart(room, actionConfig, warSkill.actionUnitAgent.unitData, warSkill.attackUnit, to, actionConfig.attackRuleList, skillLv, warSkill.skillInfoConfig.skillId, haloBuffList);
            manager.skillBulletList.Add(item);
            createCount--;
        }

        protected override void OnTick()
        {
            if (warSkill.actionUnitAgent == null)
            {
                End();
                return;
            }
            // cd后生成子弹
            if (createCount > 0 && room.LTime.time - createTime >= actionConfig.createCD)
            {
                createTime = room.LTime.time;
                if (warSkill.auto)
                {
                    float endDis = actionConfig.maxMoveDistance;
                    warSkill.attackPos = warSkill.actionUnitAgent.shotTform.position + warSkill.actionUnitAgent.shotTform.forward * endDis;
                }
                OnInitBullet(warSkill.attackPos);
            }
            else if (createCount <= 0)
            {
                End();
            }
        }

    }
}