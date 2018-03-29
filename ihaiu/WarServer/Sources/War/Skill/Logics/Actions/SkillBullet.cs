using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class SkillBullet : AbstractRoomObject
    {
        /// <summary>
        /// 射线攻击脚本
        /// </summary>
        public FxLineSegment                            fxLineSegment               = null;

        /// <summary>
        /// 子弹配置路径
        /// </summary>
        public string                                   path                        = "";
        /// <summary>
        /// 当前物体
        /// </summary>
        public GameObject                               gObject;
        /// <summary>
        /// 当前物体
        /// </summary>
        public Transform                                tForm;
        /// <summary>
        /// 是否使用中
        /// </summary>
        public bool                                     bActive                     = false;
        /// <summary>
        /// 是否子弹
        /// </summary>
        public bool                                     bullet                      = false;
        /// <summary>
        /// 技能等级
        /// </summary>
        public int                                      skillLv;
        /// <summary>
        /// 技能ID
        /// </summary>
        public int                                      currentSkillId;
        /// <summary>
        /// 搜索规则
        /// </summary>
        public AttackRule                               attackRuleList;
        /// <summary>
        /// 技能配置中的子弹信息
        /// </summary>
        public SkillActionConfigProjectile              projectileInf;
        /// <summary>
        /// 攻击者
        /// </summary>
        public UnitData                                 sendUnit;
        /// <summary>
        /// 当前目标
        /// </summary>
        public UnitData                                 attackUnit;
        /// <summary>
        /// 移动了多少距离了
        /// </summary>
        private float                                   haveMoveDistance;
        /// <summary>
        /// 上一帧位置
        /// </summary>
        private Vector3                                 proPosition;
        /// <summary>
        /// 终点位置
        /// </summary>
        public Vector3                                  endPos;
        public float                                  maxDis;
        /// <summary>
        /// 穿透数量
        /// </summary>
        public int                                      CrossCount = 0;
        /// <summary>
        /// 已经产生过伤害的列表
        /// </summary>
        public Dictionary<int,int>                      HaveDamageDic       = new Dictionary<int, int>();

        /// <summary>
        /// 子弹的音效
        /// </summary>
        public StudioEventEmitter                       eventEmitter;

        public float                                    life;

        public List<HaloBuff> haloBuffList = null;

        public void OnStart(WarRoom room, float life)
        {
            bActive = true;
            bullet = false;
            this.room = room;
            this.life = life + room.LTime.time;
            gObject.SetActive(true);
        }

        private SkillActionConfigDamage skillActionConfigDamage;
        private float                   damageCD = 0;
        private float                   curDamageCD = 0;

        public void OnStart(WarRoom room, SkillActionConfigDamage skillActionConfigDamage, UnitData sendUnit, UnitData attackUnit, AttackRule attackRuleList, int skillLv, int currentSkillId, float damageCD, float life, Vector3 attackPos, float maxDis)
        {
            bActive = true;
            bullet = false;
            this.room = room;
            this.life = life + room.LTime.time;
            this.skillActionConfigDamage = skillActionConfigDamage;
            this.sendUnit = sendUnit;
            this.attackUnit = attackUnit;
            this.attackRuleList = attackRuleList;
            this.skillLv = skillLv;
            this.currentSkillId = currentSkillId;
            this.damageCD = damageCD;
            this.curDamageCD = 0;
            this.endPos = attackPos;
            this.maxDis = maxDis;
            gObject.SetActive(true);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void OnStart(WarRoom room, SkillActionConfigProjectile projectile, UnitData sendUnit, UnitData attackUnit, Vector3 endPos, AttackRule attackRuleList, int skillLv, int currentSkillId, List<HaloBuff> haloBuffList = null)
        {
            this.room = room;
            projectileInf = projectile;
            if (eventEmitter == null)
            {
                eventEmitter = gObject.GetComponent<StudioEventEmitter>();
                if (eventEmitter == null)
                {
                    eventEmitter = gObject.AddComponent<StudioEventEmitter>();
                }
            }
            if (eventEmitter != null && !string.IsNullOrEmpty(projectile.projectSound))
            {
                eventEmitter.Event = projectile.projectSound;
                eventEmitter.StopEvent = EmitterGameEvent.ObjectDisable;
            }
            this.sendUnit = sendUnit;
            this.attackUnit = attackUnit;
            this.endPos = endPos;
            if (projectileInf != null)
            {
                if (projectileInf.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget)
                {
                    // 追踪弹
                    if (attackUnit != null)
                    {
                        this.endPos = attackUnit.AnchorAttackbyPos;
                    }
                }
            }

            this.attackRuleList = attackRuleList;
            this.skillLv = skillLv;
            this.currentSkillId = currentSkillId;
            CrossCount = projectile.CrossCount;
            HaveDamageDic = new Dictionary<int, int>();
            tForm.position = sendUnit.AnchorAttackShotPos;

            this.haloBuffList = haloBuffList;

            bActive = true;
            gObject.SetActive(true);
            haveMoveDistance = 0;
            proPosition = tForm.position;
            if (eventEmitter != null)
            {
                eventEmitter.Play();
            }
        }
        /// <summary>
        /// 到底终点
        /// </summary>
        public void OnEnd()
        {
            sendUnit = null;
            attackUnit = null;
            projectileInf = null;
            bActive = false;
            if (gObject != null)
            {
                gObject.SetActive(false);
            }
            if (eventEmitter != null)
            {
                eventEmitter.Stop();
            }
            if (room.skillManager.skillBulletList.Contains(this))
            {
                room.skillManager.skillBulletList.Remove(this);
            }
        }
        /// <summary>
        /// 是否到底终点
        /// </summary>
        bool HaveMoveOver
        {
            get
            {
                if (projectileInf != null)
                {
                    haveMoveDistance += Vector3.Distance(proPosition, tForm.position);
                    proPosition = tForm.position;
                    if (haveMoveDistance >= projectileInf.maxMoveDistance)
                    {
                        return true;
                    }
                    else
                    {
                        if (projectileInf.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget && Vector3.Distance(tForm.position, endPos) <= projectileInf.collisionRayWidth)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        // 子弹移动中
        public void Tick()
        {
            if (bActive && !bullet)
            {
                // 非自动，仅仅特效显示
                if (life <= room.LTime.time)
                {
                    OnEnd();
                }
                else if (damageCD > 0)
                {
                    // 射线伤害
                    curDamageCD -= room.LTime.deltaTime;
                    if (curDamageCD <= 0)
                    {
                        curDamageCD = damageCD;
                        Vector3 endAttackPos = endPos;
                        UnitData currentAttackTarget = null;
                        if (attackUnit != null && attackUnit.IsInSceneAndLive)
                        {
                            if (Vector3.Distance(sendUnit.AnchorAttackShotPos, attackUnit.AnchorAttackbyPos) <= maxDis)
                            {
                                endAttackPos = attackUnit.AnchorAttackbyPos;
                                currentAttackTarget = attackUnit;
                            }
                        }
                        HaveDamageDic = new Dictionary<int, int>();
                        room.skillManager.OnDamage(sendUnit.unitAgent, currentAttackTarget, attackRuleList, skillActionConfigDamage, endAttackPos, skillLv, currentSkillId, ref HaveDamageDic);
                    }
                }
                return;
            }

            if (!bActive || !bullet) return;

            bool flyOver =  HaveMoveOver;

            if (!flyOver)
            {
                if (projectileInf.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget)
                {
                    // 追踪弹
                    if (attackUnit != null && attackUnit.IsInSceneAndLive)
                    {
                        endPos = attackUnit.AnchorAttackbyPos;
                        float dis = Vector3.Distance(tForm.position.SetY(0),endPos.SetY(0));
                        if (dis <= projectileInf.collisionRayWidth)
                        {
                            // 攻击目标
                            if (!room.sceneData.CheckUnitInSafeRegion(attackUnit) && !attackUnit.isCloneUnit)
                            {
                                // 震屏
                                if (projectileInf.bShakeEffect)
                                {
                                    Game.camera.CameraMg.OnShakeCamera(projectileInf.shakeTime, projectileInf.shakeAmplitude, projectileInf.shakeRange, tForm.position);
                                }
                                room.skillManager.OnDamage(sendUnit.unitAgent, attackUnit, attackRuleList, projectileInf.skillActionConfigDamage, endPos, skillLv, currentSkillId, ref HaveDamageDic, haloBuffList);

                                if (!HaveDamageDic.ContainsKey(attackUnit.uid))
                                {
                                    HaveDamageDic.Add(attackUnit.uid, attackUnit.uid);
                                }
                            }
                            flyOver = true;
                        }
                    }
                }
                else
                {
                    // 直线飞行
                    if (CrossCount > 0)
                    {
                        UnitData canAttackUnit = FindTargetList(tForm.position,projectileInf.collisionRayWidth,attackRuleList,sendUnit.LegionId,HaveDamageDic);
                        if (canAttackUnit != null && canAttackUnit.IsInSceneAndLive)
                        {
                            if (!room.sceneData.CheckUnitInSafeRegion(canAttackUnit) && !canAttackUnit.isCloneUnit)
                            {
                                // 震屏
                                if (projectileInf.bShakeEffect)
                                {
                                    Game.camera.CameraMg.OnShakeCamera(projectileInf.shakeTime, projectileInf.shakeAmplitude, projectileInf.shakeRange, tForm.position);
                                }
                                room.skillManager.OnDamage(sendUnit.unitAgent, canAttackUnit, attackRuleList, projectileInf.skillActionConfigDamage, endPos, skillLv, currentSkillId, ref HaveDamageDic, haloBuffList);
                                if (!HaveDamageDic.ContainsKey(canAttackUnit.uid))
                                {
                                    HaveDamageDic.Add(canAttackUnit.uid, canAttackUnit.uid);
                                }
                                CrossCount--;
                            }
                        }
                        if (CrossCount <= 0)
                        {
                            flyOver = true;
                        }
                    }
                }
            }

            if (flyOver)
            {
                // 飞行终点
                UnitData canAttackUnit = attackUnit;
                if (canAttackUnit == null)
                {
                    canAttackUnit = FindTargetList(tForm.position, projectileInf.collisionRayWidth, attackRuleList, sendUnit.LegionId, HaveDamageDic);
                }
                else if (projectileInf.projectileMoveMethod != ProjectileMoveMethod.DirectToTarget)
                {
                    if (Vector3.Distance(tForm.position.SetY(0), canAttackUnit.AnchorAttackbyPos.SetY(0)) > projectileInf.collisionRayWidth)
                    {
                        canAttackUnit = null;
                    }
                }
                if (canAttackUnit == null || room.sceneData.CheckUnitInSafeRegion(canAttackUnit) || canAttackUnit.isCloneUnit)
                {
                    canAttackUnit = null;
                }

                // 震屏
                if (projectileInf.bShakeEffect)
                {
                    Game.camera.CameraMg.OnShakeCamera(projectileInf.shakeTime, projectileInf.shakeAmplitude, projectileInf.shakeRange, tForm.position);
                }
                room.skillManager.OnDamage(sendUnit.unitAgent, canAttackUnit, attackRuleList, projectileInf.skillActionConfigDamage, endPos, skillLv, currentSkillId, ref HaveDamageDic, haloBuffList);
                OnEnd();
                return;
            }
            else
            {
                tForm.LookAt(endPos);
                if (projectileInf.bRotation)
                {
                    float angle  = Mathf.Min(1, Vector3.Distance(tForm.position, endPos) / projectileInf.maxMoveDistance) * projectileInf.rotationAngle;
                    tForm.rotation = tForm.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -45, 45), 0, 0);
                }
            }
            //float currentDist = Vector3.Distance(tForm.position, endPos);
            //if (currentDist <= 0) currentDist = projectileInf.moveSpeed * Time.deltaTime;
            //tForm.Translate(Vector3.forward * Mathf.Min(projectileInf.moveSpeed * Time.deltaTime, currentDist));
            tForm.Translate(Vector3.forward * projectileInf.moveSpeed * Time.deltaTime);
        }

        UnitData FindTargetList(Vector3 pos, float collisionRayWidth, AttackRule attackRule, int legionId, Dictionary<int, int> dic)
        {
            List<UnitAgent> list = room.clientSceneView.GetUnitList().FindAll(m => attackRule.unitType.UContain(m.unitType) && attackRule.unitSpaceType.UContain(m.unitSpaceType) && attackRule.relationType.RContain(m.unitData.GetRelationType(legionId)));
            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
            {
                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
            }
            if (dic.Count > 0)
            {
                for (int j = list.Count - 1; j >= 0; j--)
                {
                    if (dic.ContainsKey(list[j].unitData.uid))
                    {
                        list.RemoveAt(j);
                    }
                }
            }
            if (list.Count > 0)
            {
                list.Sort((UnitAgent a, UnitAgent b) =>
                {
                    float aa = Vector3.Distance(pos, a.position);
                    float bb = Vector3.Distance(pos, b.position);
                    if (aa > bb)
                    {
                        return 1;
                    }
                    else if (aa < bb)
                    {
                        return -1;
                    }
                    return 0;
                });
                float __dis = Vector3.Distance(new Vector3(list[0].position.x,0,list[0].position.z), new Vector3(pos.x,0,pos.z));
                if (__dis <= collisionRayWidth)
                {
                    // 攻击目标
                    return list[0].unitData;
                }
            }
            return null;
        }
    }
}