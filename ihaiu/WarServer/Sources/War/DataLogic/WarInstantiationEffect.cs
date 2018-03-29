//using FMODUnity;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Games.Wars
//{
//    /// <summary>
//    /// 生成的特效
//    /// </summary>
//    public class WarInstantiationEffect : AbstractRoomObject
//    {
//        private static WarInstantiationEffect _instance;
//        public static WarInstantiationEffect Instance
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    _instance = new WarInstantiationEffect();
//                }
//                return _instance;
//            }
//        }
//        public void Init(WarRoom room)
//        {
//            this.room = room;
//            _instance.Start();
//        }

//        public void Start()
//        {
//            Stop();
//        }

//        public void Stop()
//        {
//            projectileListList.Clear();
//            for (int i = showEffect.Count - 1; i >= 0; i--)
//            {
//                SkillInstantEffect item = showEffect[i];
//                if (item != null)
//                {
//                    item.showHideEffect(false);
//                    showEffect.Remove(item);
//                }
//            }
//            showEffect.Clear();
//            //for (int i = 0; i < poolEffect.Count; i++)
//            //{
//            //    if (poolEffect[i] != null && poolEffect[i].gObject != null)
//            //    {
//            //        GameObject.Destroy(poolEffect[i].gObject);
//            //    }
//            //}
//            //poolEffect.Clear();
//        }

//        #region 参数配置
//        /// <summary>
//        /// 特效对象池
//        /// </summary>
//        private List<SkillInstantEffect>               poolEffect  = new List<SkillInstantEffect>();
//        /// <summary>
//        /// 展示中的特效
//        /// </summary>
//        private List<SkillInstantEffect>               showEffect  = new List<SkillInstantEffect>();
//        /// <summary>
//        /// 待发射的子弹列表
//        /// </summary>
//        private List<ProjectileList>                    projectileListList = new List<ProjectileList>();
//        #endregion

//        // Update is called once per frame
//        public void OnSyncedUpdate()
//        {
//            #region 刷新子弹
//            for (int i = projectileListList.Count - 1; i >= 0; i--)
//            {
//                ProjectileList item = projectileListList[i];
//                if (item.maxCount > 0)
//                {
//                    if (LTime.time > item.startTime)
//                    {
//                        if (item.projectile.projectileMoveMethod == ProjectileMoveMethod.AngledFromEach)
//                        {
//                            // 多重箭
//                            Transform tf = item.skillDate.unitAgentSend.modelTform;
//                            float angleUnit = item.projectile.angleFromEach / (item.maxCount - 1);
//                            float maxAngle = (item.projectile.angleFromEach / 2);
//                            float minAngle = -maxAngle;
//                            float angle = minAngle;
//                            for (int k = 0; k < item.maxCount; k++)
//                            {
//                                Vector3 tl = tf.position + (Quaternion.AngleAxis(+angle, Vector3.up) * tf.forward * item.projectile.fizzleDistance);
//                                InitEffect(item.skillDate, item.skillTriggerEvent, item.projectile, tl);
//                                angle += angleUnit;
//                            }
//                            item.maxCount = 0;
//                        }
//                        else
//                        {
//                            item.startTime += item.projectile.createCD;
//                            item.maxCount--;
//                            InitEffect(item.skillDate, item.skillTriggerEvent, item.projectile);
//                        }
//                    }
//                }
//                else
//                {
//                    projectileListList.Remove(item);
//                    continue;
//                }
//            }
//            #endregion

//            #region 刷新特效
//            for (int i = showEffect.Count - 1; i >= 0; i--)
//            {
//                SkillInstantEffect item = showEffect[i];
//                if (item == null || item.gObject == null)
//                {
//                    showEffect.Remove(item);
//                    continue;
//                }
//                if (!item.bShow)
//                {
//                    // 特效显示
//                    if (LTime.time > item.startTime)
//                    {
//                        item.showHideEffect(true);
//                    }
//                    else
//                    {
//                        continue;
//                    }
//                }
//                else if (item.bBullet)
//                {
//                    // 是否飞行结束
//                    bool flyOver =  item.HaveMove;
//                    // 到终点的距离
                    
//                    if (!flyOver)
//                    {
//                        // 子弹飞行
//                        if (item.projectile.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget)
//                        {
//                            //// 追踪弹
//                            if (item.unitData != null && item.unitData.IsInSceneAndLive)
//                            {
//                                //if (item.skillDate.unitAgentSend.name.Equals("Unit_Build(3)"))
//                                //{
//                                //    LLL.LL("=== " + item.endPos + "  " + item.unitData.isLive);
//                                //}
//                                item.endPos = item.unitData.AnchorAttackbyPos;
//                                float __dis = Vector3.Distance(new Vector3(item.unitData.position.x,0,item.unitData.position.z), new Vector3(item.tForm.position.x,0,item.tForm.position.z));
//                                if (__dis <= item.projectile.collisionRayWidth)
//                                {
//                                    // 攻击目标
//                                    if (item.skillDate != null)
//                                    {
//                                        AttackTartget(item.skillDate.unitAgentSend, item.unitData, item.projectile, item.projectile.secondTarget, item.tForm.position, item.skillDate.skillBass.skillLv, item.skillTriggerEvent.attackRuleList, item.skillDate, ref item.HaveDamageDic);
//                                    }
//                                    if (!item.HaveDamageDic.ContainsKey(item.unitData.uid))
//                                    {
//                                        item.HaveDamageDic.Add(item.unitData.uid, item.unitData.uid);
//                                    }
//                                    flyOver = true;
//                                }
//                            }
//                            item.tForm.LookAt(item.endPos);
//                            if (item.bRotate)
//                            {
//                                // 抛物线
//                                float angle = Mathf.Min(1, Vector3.Distance(item.tForm.position, item.endPos) / item.endDis) * 45;
//                                item.tForm.rotation = item.tForm.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -35, 35), 0, 0);
//                            }
//                        }
//                        else
//                        {
//                            // 直线飞行
//                            item.tForm.LookAt(item.endPos);
//                            if (item.bRotate)
//                            {
//                                // 抛物线
//                                float angle = Mathf.Min(1, Vector3.Distance(item.tForm.position, item.endPos) / item.endDis) * 45;
//                                item.tForm.rotation = item.tForm.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -35, 35), 0, 0);
//                            }
//                            #region 飞行途中遇到地方目标，是否攻击
//                            if (item.hitCount > 0)
//                            {
//                                UnitData unitData = FindTargetList(item.tForm.position,item.projectile.collisionRayWidth,item.skillTriggerEvent.attackRuleList,item.legionId,item.HaveDamageDic);
//                                if (unitData != null)
//                                {
//                                    AttackTartget(item.skillDate.unitAgentSend, unitData, item.projectile, item.projectile.secondTarget, item.tForm.position, item.skillDate.skillBass.skillLv, item.skillTriggerEvent.attackRuleList, item.skillDate, ref item.HaveDamageDic);
//                                    if (!item.HaveDamageDic.ContainsKey(unitData.uid))
//                                    {
//                                        item.HaveDamageDic.Add(unitData.uid, unitData.uid);
//                                    }
//                                    item.hitCount--;
//                                }
//                                if (item.hitCount <= 0)
//                                {
//                                    flyOver = true;
//                                }
//                            }
//                            #endregion
//                        }
//                    }
//                    if (flyOver)
//                    {
//                        #region 飞行终点
//                        UnitData unitData = item.unitData;
//                        if (unitData == null)
//                        {
//                            unitData = FindTargetList(item.tForm.position, item.projectile.collisionRayWidth, item.skillTriggerEvent.attackRuleList, item.legionId, item.HaveDamageDic);
//                        }
//                        else if (item.projectile.projectileMoveMethod != ProjectileMoveMethod.DirectToTarget)
//                        {
//                            if (Vector3.Distance(item.tForm.position, unitData.position) > item.projectile.collisionRayWidth)
//                            {
//                                unitData = null;
//                            }
//                        }
//                        if (item.skillDate != null)
//                        {
//                            AttackTartget(item.skillDate.unitAgentSend, unitData, item.projectile, item.projectile.secondTarget, item.tForm.position, item.skillDate.skillBass.skillLv, item.skillTriggerEvent.attackRuleList, item.skillDate, ref item.HaveDamageDic);
//                            if (unitData != null && !item.HaveDamageDic.ContainsKey(unitData.uid))
//                            {
//                                item.HaveDamageDic.Add(unitData.uid, unitData.uid);
//                            }
//                        }
//                        #endregion
//                        item.showHideEffect(false);
//                        showEffect.Remove(item);
//                        continue;
//                    }
//                    float currentDist = Vector3.Distance(item.tForm.position, item.endPos);
//                    item.tForm.Translate(Vector3.forward * Mathf.Min(item.projectile.moveSpeed * Time.deltaTime, currentDist));
//                }
//                else
//                {
//                    if (LTime.time > item.life)
//                    {
//                        // 特效结束，关闭
//                        item.showHideEffect(false);
//                        showEffect.Remove(item);
//                        continue;
//                    }
//                }
//            }
//            #endregion
//        }

//        #region 生成特效
//        /// <summary>
//        /// 仅仅创建特效
//        /// </summary>
//        /// <param name="skillDate"></param>
//        /// <param name="skillTriggerEvent"></param>
//        public SkillInstantEffect InitEffect(SkillDate skillDate, SkillTriggerEvent skillTriggerEvent, Vector3 pos, bool followEffect = false)
//        {
//            Transform parent = null;
//            if (skillTriggerEvent.followSelf && skillDate != null)
//            {
//                parent = skillDate.unitAgentSend.modelTform;
//                pos = skillTriggerEvent.pathOffset;
//            }
//            return InitEffect(skillDate, skillTriggerEvent.effectPath, skillTriggerEvent.life, pos, parent, -1, followEffect);
//        }
//        public SkillInstantEffect InitEffect(SkillDate skillDate, string path, float life, Vector3 pos, Transform parent = null, int legionId = -1, bool followEffect = false)
//        {
//            SkillInstantEffect item = showEffect.Find(m => m != null && !m.bActive && m.path.Equals(path));
//            if (item == null)
//            {
//                Transform parent2 = null;
//                if (skillDate != null && skillDate.unitAgentSend != null)
//                {
//                    parent2 = skillDate.unitAgentSend.modelTform;
//                }
//                item = InitObject(path, parent, parent2);
//                showEffect.Add(item);
//            }
//            if (parent == null)
//            {
//                if (followEffect && skillDate != null)
//                {
//                    item.tForm.SetParent(skillDate.unitAgentSend.modelTform);
//                    item.tForm.localPosition = Vector3.zero;
//                    item.tForm.localEulerAngles = Vector3.zero;
//                    item.tForm.SetParent(null);
//                }
//                else
//                {
//                    item.tForm.position = pos;
//                    if (skillDate != null)
//                    {
//                        item.tForm.localEulerAngles = skillDate.unitAgentSend.rotation;
//                    }
//                }
//            }
//            else if (skillDate != null && followEffect)
//            {
//                item.tForm.localPosition = pos;
//            }
//            item.HaveDamageDic = new Dictionary<int, int>();
//            item.skillDate = skillDate;
//            item.startTime = LTime.time;
//            item.life = LTime.time + life;
//            if (skillDate != null)
//            {
//                item.legionId = skillDate.unitAgentSend.unitData.LegionId;
//            }
//            else
//            {
//                item.legionId = legionId;
//            }
//            return item;
//        }
//        /// <summary>
//        /// 生成多重箭的其中一支
//        /// </summary>
//        /// <param name="skillDate"></param>
//        /// <param name="skillTriggerEvent"></param>
//        /// <param name="projectile"></param>
//        /// <param name="to"></param>
//        /// <returns></returns>
//        public SkillInstantEffect InitEffect(SkillDate skillDate, SkillTriggerEvent skillTriggerEvent, Projectile projectile, Vector3 to)
//        {
//            SkillInstantEffect item = InitEffect(skillDate, skillTriggerEvent,Vector3.zero);
//            item.bBullet = true;
//            item.projectile = projectile;
//            item.skillTriggerEvent = skillTriggerEvent;
//            item.endDis = projectile.fizzleDistance;
//            item.endPos = to;
//            item.hitCount = projectile.hitCount;
//            item.bRotate = projectile.bRotation;
//            item.skillDate = skillDate;
//            if (skillDate != null)
//            {
//                item.legionId = skillDate.unitAgentSend.unitData.LegionId;
//            }
//            else
//            {
//                item.legionId = -1;
//            }
//            item.tForm.position = skillDate.unitAgentSend.unitData.AnchorAttackShotPos + skillTriggerEvent.pathOffset;
//            if (item.eventEmitter == null)
//            {
//                item.eventEmitter = item.gObject.GetComponent<StudioEventEmitter>();
//                if (item.eventEmitter == null)
//                {
//                    item.eventEmitter = item.gObject.AddComponent<StudioEventEmitter>();
//                }
//            }
//            if (item.eventEmitter != null && !string.IsNullOrEmpty(item.projectile.projectMusic))
//            {
//                item.eventEmitter.Event = item.projectile.projectMusic;
//                item.eventEmitter.StopEvent = EmitterGameEvent.ObjectDisable;
//            }
//            return item;
//        }
//        /// <summary>
//        /// 生成子弹
//        /// </summary>
//        /// <param name="skillDate"></param>
//        /// <param name="skillTriggerEvent"></param>
//        /// <param name="projectile"></param>
//        /// <returns></returns>
//        public SkillInstantEffect InitEffect(SkillDate skillDate, SkillTriggerEvent skillTriggerEvent, Projectile projectile)
//        {
//            SkillInstantEffect item = InitEffect(skillDate, skillTriggerEvent,Vector3.zero);
//            item.bBullet = true;
//            item.projectile = projectile;
//            item.skillTriggerEvent = skillTriggerEvent;
//            if (skillDate.skillBass.aimDirectionList[0].targetLocation == TargetLocation.TargetCircleArea)
//            {
//                if (skillDate.unitDataBy != null)
//                {
//                    item.endDis = Vector3.Distance(skillDate.unitDataBy.AnchorAttackbyPos, skillDate.unitAgentSend.modelTform.position);
//                    item.endPos = skillDate.unitDataBy.AnchorAttackbyPos;
//                }
//                else
//                {
//                    if (skillDate.bAuToSearchTarget)
//                    {
//                        item.endDis = skillDate.skillBass.aimDirectionList[0].targetCircleAreaDis;
//                        item.endPos = skillDate.unitAgentSend.modelTform.position + skillDate.unitAgentSend.modelTform.forward * item.endDis;
//                    }
//                    else
//                    {
//                        item.endDis = Vector3.Distance(skillDate.skillAttackPoint, skillDate.unitAgentSend.modelTform.position);
//                        item.endPos = skillDate.skillAttackPoint;
//                    }
//                }
//            }
//            else
//            {
//                Transform tf = skillDate.unitAgentSend.modelTform;
//                if (skillDate != null && skillDate.unitAgentSend != null && skillDate.unitAgentSend.animatorManager != null && skillDate.unitAgentSend.animatorManager.ShotPos != null)
//                {
//                    tf = skillDate.unitAgentSend.animatorManager.ShotPos;
//                }
//                if (skillDate != null)
//                {
//                    item.endDis = projectile.fizzleDistance;
//                    if (skillDate.unitDataBy != null && projectile.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget)
//                    {
//                        item.endPos = skillDate.skillAttackPoint;
//                        item.unitData = skillDate.unitDataBy;
//                    }
//                    else
//                    {
//                        item.unitData = null;
//                        item.endPos = tf.position + tf.forward * item.endDis;
//                    }
//                }
//            }
//            item.hitCount = projectile.hitCount;
//            item.bRotate = projectile.bRotation;
//            item.skillDate = skillDate;
//            if (skillDate != null)
//            {
//                item.legionId = skillDate.unitAgentSend.unitData.LegionId;
//            }
//            else
//            {
//                item.legionId = -1;
//            }
//            item.tForm.position = skillDate.unitAgentSend.unitData.AnchorAttackShotPos + skillTriggerEvent.pathOffset;
//            if (item.unitData == null)
//            {
//                // 追踪点确定目标
//                if (item.projectile.projectileMoveMethod == ProjectileMoveMethod.DirectToTarget)
//                {
//                    item.unitData = FindTargetList(item.tForm.position, item.projectile.searchDis, item.skillTriggerEvent.attackRuleList, item.legionId, item.HaveDamageDic);
//                }
//            }
//            if (item.eventEmitter == null)
//            {
//                item.eventEmitter = item.gObject.GetComponent<StudioEventEmitter>();
//                if (item.eventEmitter == null)
//                {
//                    item.eventEmitter = item.gObject.AddComponent<StudioEventEmitter>();
//                }
//            }
//            if(item.eventEmitter != null && !string.IsNullOrEmpty(item.projectile.projectMusic))
//            {
//                item.eventEmitter.Event = item.projectile.projectMusic;
//                item.eventEmitter.StopEvent = EmitterGameEvent.ObjectDisable;
//            }

//            return item;
//        }
//        /// <summary>
//        /// 发射的子弹加入发射列表内，等待发射
//        /// </summary>
//        /// <param name="skillDate"></param>
//        /// <param name="skillTriggerEvent"></param>
//        /// <param name="projectile"></param>
//        public void InitEffectList(SkillDate skillDate, SkillTriggerEvent skillTriggerEvent, Projectile projectile, UnitAgent unitagent = null)
//        {
//            ProjectileList item = new ProjectileList();
//            item.skillDate = skillDate;
//            item.unitAgent = unitagent;
//            item.skillTriggerEvent = skillTriggerEvent;
//            item.projectile = projectile;
//            item.startTime = LTime.time;
//            item.maxCount = projectile.maxProjectiles;
//            projectileListList.Add(item);
//        }
//        /// <summary>
//        /// 从对象池中取出特效
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="parent"></param>
//        /// <returns></returns>
//        SkillInstantEffect InitObject(string path, Transform parent = null, Transform parents = null)
//        {
//            if (path == null || path.Length < 2)
//            {
//                path = "PrefabFx/AttackEffect/Bullet_Empty";
//            }
//            SkillInstantEffect item = null;
//            if (parent == null)
//            {
//                item = poolEffect.Find(m => m != null && !m.bActive && m.path.Equals(path) && m.gObject != null);
//            }
//            else
//            {
//                item = poolEffect.Find(m => m != null && !m.bActive && m.path.Equals(path) && m.gObject != null && m.tForm.parent == parent);
//            }
//            if (item == null)
//            {
//                item = new SkillInstantEffect();
//                if (parent == null)
//                {
//                    item.gObject = room.clientRes.GetGameObjectInstall(path, parents);
//                    item.gObject.transform.SetParent(null);
//                }
//                else
//                {
//                    item.gObject = room.clientRes.GetGameObjectInstall(path, parent);
//                }
//                item.tForm = item.gObject.transform;
//                item.path = path;
//                item.gObject.SetActive(false);
//                poolEffect.Add(item);
//            }
//            item.bActive = true;
//            item.bShow = false;
//            return item;
//        }
//        #endregion

//        #region 关闭一个特效，针对英雄的关闭普攻特效
//        public void OnStopEffect(int uid, string path)
//        {
//            List<SkillInstantEffect> Item = showEffect.FindAll(m => m != null && m.bActive && m.path != null && m.path.Equals(path) && m.skillDate != null && m.skillDate.unitAgentSend != null && m.skillDate.unitAgentSend.uid == uid);
//            if (Item != null && Item.Count > 0)
//            {
//                for (int i = Item.Count - 1; i >= 0; i--)
//                {
//                    Item[i].life = 0;
//                }
//            }
//        }
//        #endregion

//        #region 查找攻击目标
//        /// <summary>
//        /// 查找攻击目标
//        /// </summary>
//        /// <param name="pos">查找的圆心</param>
//        /// <param name="collisionRayWidth">撞检测宽度</param>
//        /// <param name="attackRule">攻击规则</param>
//        /// <param name="legionId">势力id</param>
//        /// <param name="dic">已检测过的字典</param>
//        /// <returns></returns>
//        UnitData FindTargetList(Vector3 pos, float collisionRayWidth, AttackRule attackRule, int legionId, Dictionary<int, int> dic)
//        {
//            List<UnitAgent> list = room.clientSceneView.GetUnitList().FindAll(m => attackRule.unitType.UContain(m.unitType) && attackRule.unitSpaceType.UContain(m.unitSpaceType) && attackRule.relationType.RContain(m.unitData.GetRelationType(legionId)));
//            if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
//            {
//                list = list.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
//            }
//            if (dic.Count > 0)
//            {
//                for (int j = list.Count - 1; j >= 0; j--)
//                {
//                    if (dic.ContainsKey(list[j].unitData.uid))
//                    {
//                        list.RemoveAt(j);
//                    }
//                }
//            }
//            if (list.Count > 0)
//            {
//                list.Sort((UnitAgent a, UnitAgent b) =>
//                {
//                    float aa = Vector3.Distance(pos, a.position);
//                    float bb = Vector3.Distance(pos, b.position);
//                    if (aa > bb)
//                    {
//                        return 1;
//                    }
//                    else if (aa < bb)
//                    {
//                        return -1;
//                    }
//                    return 0;
//                });
//                float __dis = Vector3.Distance(new Vector3(list[0].position.x,0,list[0].position.z), new Vector3(pos.x,0,pos.z));
//                if (__dis <= collisionRayWidth)
//                {
//                    // 攻击目标
//                    return list[0].unitData;
//                }
//            }
//            return null;
//        }
//        #endregion

//        #region 攻击目标
//        /// <summary>
//        /// 攻击目标
//        /// </summary>
//        /// <param name="unitAgent">攻击发起者</param>
//        /// <param name="unitData">被攻击者</param>
//        /// <param name="projectile">抛射效果</param>
//        /// <param name="secondTarget"></param>
//        /// <param name="pos">攻击点</param>
//        /// <param name="skilllv">技能等级</param>
//        /// <param name="attackRule">攻击规则</param>
//        /// <param name="dic">已检测过的字典</param>
//        void AttackTartget(UnitAgent unitAgent, UnitData unitData, Projectile projectile, SecondTarget secondTarget, Vector3 pos, int skilllv, AttackRule attackRule, SkillDate skilldate, ref Dictionary<int, int> dic)
//        {
//            if (dic == null)
//            {
//                dic = new Dictionary<int, int>();
//            }
//            if (dic.Count > 0 && unitData != null)
//            {
//                if (dic.ContainsKey(unitData.uid))
//                {
//                    unitData = null;
//                }
//            }
//            if (unitData != null && unitAgent != null && !projectile.onlySecondDamage)
//            {
//                // "播放音效";
//                Game.audio.PlayWarSFX(projectile.damageInfo.DamageMusic, pos);

//                //if (projectile.damageInfo.passiveJudgment)
//                //{
//                //    unitAgent.unitControl.OnPassiveEvent(Triggercondition.OnAttackTarget, unitAgent.unitData, unitData);
//                //}

//                //JudgmentInfo ji = new JudgmentInfo();
//                //ji.judgmentTargger = JudgmentTargger.TargetType;
//                //ji.unitType = unitData.unitType;
//                //unitAgent.unitControl.OnPassiveJudgEvent(ji);
//                //JudgmentInfo ji2 = new JudgmentInfo();
//                //ji2.judgmentTargger = JudgmentTargger.TargetDis;
//                //ji2.disTarget = Vector3.Distance(unitAgent.position, unitData.position);
//                //unitAgent.unitControl.OnPassiveJudgEvent(ji2);

//                DamageData damage = new DamageData();
//                damage.AttackSend = unitAgent.unitData;
//                damage.damageInfBaseCSV = projectile.damageInfo.damageInfBaseCSV;
//                damage.damageInfBaseCSV.skillLv = skilllv;
//                if (damage.damageInfBaseCSV.selectTarget == SelectTarget.Self)
//                {
//                    unitAgent.unitData.GetDamageInfoVal(ref damage.damageInfBaseCSV);
//                }
//                unitData.OnTakeDamage(damage);
//                if (!dic.ContainsKey(unitData.uid))
//                {
//                    dic.Add(unitData.uid, unitData.uid);
//                }
//                if (projectile.damageInfo.buffid > 0)
//                {
//                    // 生成buff
//                    BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(projectile.damageInfo.buffid);
//                    if (projectile.damageInfo.bufflife > 0)
//                    {
//                        buffInfo.mLife = projectile.damageInfo.bufflife;
//                    }
//                    room.haoleBuff.AddOneBuffForUnit(unitData, buffInfo, skilllv, unitAgent.unitData);
//                }
//            }

//            if (projectile.damageInfo.haloid > 0)
//            {
//                // 生成定位光环
//                BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(projectile.damageInfo.haloid);
//                if (projectile.damageInfo.bufflife > 0)
//                {
//                    buffInfo.mLife = projectile.damageInfo.bufflife;
//                }
//                room.haoleBuff.AddHalos(unitAgent.unitData.LegionId, attackRule, buffInfo, pos, skilllv, null, unitAgent.unitData);
//            }
//            if (projectile.damageInfo.bShakeEffect)
//            {
//                Game.camera.CameraMg.OnShakeCamera(projectile.damageInfo.shakeTime, projectile.damageInfo.shakeAmplitude, projectile.damageInfo.shakeRange, pos);
//            }

//            if (unitData == null && secondTarget.hitActiveSecond)
//            {
//                return;
//            }

//            // 是否有爆点，二次伤害效果
//            if (secondTarget.maxSceondTargets > 0 && unitAgent != null)
//            {
//                #region 爆炸特效
//                InitEffect(skilldate, secondTarget.secondEffectPath, secondTarget.life, pos);
//                #endregion

//                // "播放音效";
//                Game.audio.PlayWarSFX(secondTarget.secondaryMusic, pos);

//                if (secondTarget.damageInfo.bShakeEffect)
//                {
//                    Game.camera.CameraMg.OnShakeCamera(secondTarget.damageInfo.shakeTime, secondTarget.damageInfo.shakeAmplitude, secondTarget.damageInfo.shakeRange, pos);
//                }
//                #region 伤害计算
//                if (secondTarget.secondaryRadius > 0)
//                {

//                    int _count = 0;
//                    List<UnitData> TemUnitDataList = room.sceneData.SearchUnitListInRule(unitAgent.unitData, attackRule.unitType, attackRule.relationType, attackRule.unitSpaceType, secondTarget.secondaryRadius,pos,out _count,true,secondTarget.warSkillRule,secondTarget.maxSceondTargets);
//                    if (attackRule.unitType == UnitType.BuildAndPlayer && attackRule.unitBuildType != UnitBuildType.All)
//                    {
//                        TemUnitDataList = TemUnitDataList.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRule.unitBuildType.UContain(m.buildType)));
//                    }
//                    if (dic.Count > 0)
//                    {
//                        for (int j = TemUnitDataList.Count - 1; j >= 0; j--)
//                        {
//                            if (dic.ContainsKey(TemUnitDataList[j].unitData.uid))
//                            {
//                                TemUnitDataList.RemoveAt(j);
//                            }
//                        }
//                    }
//                    _count = TemUnitDataList.Count;
//                    if (_count > 0)
//                    {
//                        BuffInfo buffInfo = unitAgent.unitData.GetBuffInfo(secondTarget.damageInfo.buffid);
//                        if (secondTarget.damageInfo.bufflife > 0)
//                        {
//                            buffInfo.mLife = secondTarget.damageInfo.bufflife;
//                        }
//                        for (int l = 0; l < _count; l++)
//                        {
//                            room.haoleBuff.AddOneBuffForUnit(TemUnitDataList[l], buffInfo, skilllv, unitAgent.unitData);
//                            DamageData damage = new DamageData();
//                            damage.AttackSend = unitAgent.unitData;
//                            damage.damageInfBaseCSV = secondTarget.damageInfo.damageInfBaseCSV;
//                            damage.damageInfBaseCSV.skillLv = skilllv;
//                            if (damage.damageInfBaseCSV.selectTarget == SelectTarget.Self)
//                            {
//                                unitAgent.unitData.GetDamageInfoVal(ref damage.damageInfBaseCSV);
//                            }
//                            TemUnitDataList[l].unitData.OnTakeDamage(damage);
//                            // "播放音效";
//                            Game.audio.PlayWarSFX(secondTarget.damageInfo.DamageMusic, TemUnitDataList[l].position);
//                        }
//                    }
//                }
//                #endregion
//            }
//        }
//        #endregion

//        #region 建造和维修特效
//        public SkillInstantEffect GetBuildFixEffect(string path, Transform parent)
//        {
//            SkillInstantEffect item = InitObject(path, null, parent);
//            item.tForm.SetParent(parent);
//            item.tForm.localEulerAngles = Vector3.zero;
//            item.tForm.localPosition = Vector3.zero;
//            item.tForm.localScale = Vector3.one;
//            item.tForm.SetParent(null);
//            return item;
//        }
//        public void GetBuildFixEffect(string path, Transform parent, float life)
//        {
//            SkillInstantEffect item = InitObject(path, null, parent);
//            item.tForm.SetParent(parent);
//            item.tForm.localEulerAngles = Vector3.zero;
//            item.tForm.localPosition = Vector3.zero;
//            item.tForm.localScale = Vector3.one;
//            item.tForm.SetParent(null);
//            item.life = LTime.time + life;
//            item.bBullet = false;
//            showEffect.Add(item);
//        }
//        #endregion

//    }

//}