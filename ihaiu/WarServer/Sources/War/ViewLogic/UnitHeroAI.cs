using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// pvpladder英雄AI行为
    /// </summary>
    public partial class UnitHeroAI : AbstractUnitMonoBehaviour, IPunObservable
    {
        public enum HeroAiEnum
        {
            None = 0,
            Default,
            Idle,
            Move,
            MoveFoward,
            MoveBack,
            MoveRight,
            MoveLeft,
            Attack,
            AttackFoward,
            AttackBack,
            AttackRight,
            AttackLeft,
            Skill1,
            Skill2,
            Skill3,
            Skill4,
        }

        public HeroAiEnum state = HeroAiEnum.None;

        public override void Init(UnitData unitData)
        {
            base.Init(unitData);
            if (navMeshAgent == null)
                navMeshAgent = GetComponent<UnitRVOAgent>();
            navMeshAgent.spaceType = unitData.spaceType;
            navMeshAgent.enabled = false;
            int id = unitData.unitLevelConfig.aiHeros[0];
            aIHeroConfig = Game.config.aIHero.AIGetConfig(id);

            skillIdDic = new Dictionary<HeroAiEnum, int>();
            SkillController skill0 = unitData.GetSkillByIndex(0);
            if (skill0 != null)
            {
                skillIdDic.Add(HeroAiEnum.Attack, skill0.skillId);
                skillConfig = skill0.skillConfig;
            }
            if (!unitData.isCloneUnit)
            {
                for (int i = 1; i < 5; i++)
                {
                    SkillController skill = unitData.GetSkillByIndex(i);
                    if (skill != null)
                    {
                        HeroAiEnum HeroAiEnums = HeroAiEnum.Skill1;
                        switch (i)
                        {
                            case 1:
                                HeroAiEnums = HeroAiEnum.Skill1;
                                break;
                            case 2:
                                HeroAiEnums = HeroAiEnum.Skill2;
                                break;
                            case 3:
                                HeroAiEnums = HeroAiEnum.Skill3;
                                break;
                            case 4:
                                HeroAiEnums = HeroAiEnum.Skill4;
                                break;
                        }
                        stateCD.Add(HeroAiEnums, skill.skillLevelConfig.cd);
                        skillIdDic.Add(HeroAiEnums, skill.skillId);
                    }
                }
            }
            moveSpeed = unitData.prop.MovementSpeed;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //if (stream.isWriting)
            //{
            //    stream.SendNext(state);
            //    stream.SendNext(routePointIndex);
            //}
            //else
            //{
            //    state = (BehaviourState)stream.ReceiveNext();
            //    routePointIndex = (int)stream.ReceiveNext();
            //}
        }

        // 攻击目标
        public UnitData targetUnit = null;
        public string targetName;
        // 位置目标
        public Vector3 targetPosition;
        // aihero配置
        public AIHeroConfig aIHeroConfig;
        // ai刷新cd时间间隔
        public float    aiRefrushCD = 0;
        // 当前遇敌距离（事件）
        public int      currentDis = 0;
        // 状态cd
        public Dictionary<HeroAiEnum, float> stateCD = new Dictionary<HeroAiEnum, float>();
        // 状态上次记录时间
        public Dictionary<HeroAiEnum, float> statePreTime = new Dictionary<HeroAiEnum, float>();
        // 当前技能列表
        public Dictionary<HeroAiEnum, int>             skillIdDic = new Dictionary<HeroAiEnum, int>();
        // 开始当前攻击
        public bool    currentAttackStart = false;
        private float   moveSpeed = 12;

        /// <summary>
        /// 遇敌距离
        /// </summary>
        private float enemyDistance
        {
            get
            {
                if (unitData.isCloneUnit)
                {
                    if (targetUnit != null)
                    {
                        return Vector3.Distance(unitAgent.position, targetUnit.position);
                    }
                    else
                    {
                        return 99999;
                    }
                }
                else
                {
                    return Vector3.Distance(room.clientOperationUnit.position, room.clientOperationUnit.positionPVPLadder);
                }
            }
        }

        private float _attackDis = 0;
        /// <summary>
        /// 攻击距离
        /// </summary>
        private float attackDistance
        {
            get
            {
                if (targetUnit == null)
                {
                    if (unitData.isCloneUnit)
                    {
                        TickSearchTargetAttack();
                    }
                    else
                    {
                        if (unitData.LegionId == room.clientOperationUnit.legionId)
                        {
                            targetUnit = room.clientOperationUnit.GetUnitDataPVPLadder();
                        }
                        else
                        {
                            targetUnit = room.clientOperationUnit.GetUnitData();
                        }
                    }
                    if (targetUnit != null)
                    { 
                        float min = unitData.rvoRadius + targetUnit.rvoRadius + 1;

                        if (unitData.attackAiSoliderSkill != null)
                        {
                            float val = unitData.attackAiSoliderSkill.skillController.attackRadius;
                            if (val > 0)
                            {
                                _attackDis = Mathf.Max(min, val);
                            }
                        }
                        _attackDis = prop.AttackRadius + unitData.rvoRadius + targetUnit.rvoRadius;
                    }
                }
                return _attackDis;
            }
        }


        /** 搜索目标CD */
        public float searchTargetCD = 0;
        /** 搜索目标频率间隔时间 */
        public float searchTargetInterval = 0.5f;
        public SkillConfig      skillConfig;
        /** 搜索目标 */
        public void TickSearchTargetAttack()
        {
            if (searchTargetCD <= 0)
            {
                searchTargetCD = searchTargetInterval;

                List<UnitData> enumyList = sceneData.SearchUnitList(unitData, skillConfig.targetRelation, skillConfig.targetUnitType, skillConfig.targetBuildType, skillConfig.targetSoliderType, skillConfig.targetSpaceType, skillConfig.targetProfessionType, unitData.prop.RadarRadius, true);
                int count = enumyList.Count;
                float maxScore = 0;
                UnitData maxEnumy = null;
                UnitData enumy;
                float scoreDistance = 0;
                float scoreHatred = 0;
                float scoreType = 0;
                for (int i = 0; i < enumyList.Count; i++)
                {
                    enumy = enumyList[i];
                    if (room.sceneData.CheckUnitInSafeRegion(enumy) || enumy.isCloneUnit) continue;
                    scoreDistance = unitData.Distance(enumy);
                    scoreDistance = Mathf.Clamp(1 - scoreDistance / unitData.prop.RadarRadius, 0, 2) * skillConfig.aiConfigDistance.GetVal(enumy);
                    scoreType = skillConfig.aiConfigType.GetVal(enumy);
                    scoreHatred = unitData.GetHatred(enumy.uid);

                    scoreHatred *= skillConfig.aiConfigWeight.weightHatred;
                    scoreType *= skillConfig.aiConfigWeight.weightType;
                    scoreDistance *= skillConfig.aiConfigWeight.weightDistance;


                    enumy.aiScore = scoreHatred + scoreType + scoreDistance;
                    if (enumy.aiScore > maxScore)
                    {
                        maxScore = enumy.aiScore;
                        maxEnumy = enumy;
                    }
                }


                if (maxEnumy != null)
                {
                    targetUnit = maxEnumy;
                    targetName = targetUnit.unitAgent.name;
                }
            }
        }

        /// <summary>
        /// 战斗距离
        /// </summary>
        private float fightingDistance
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 后退距离
        /// </summary>
        private float backDistance
        {
            get
            {
                return 0;
            }
        }

        public bool TickIdleState()
        {
            if (unitData.IsUnusual) return false;
            if (aiRefrushCD > 0)
            {
                state = aIHeroConfig.defaultState;
                StopMove();
                //Loger.Log("<color=blue>Enter Default " + unitData.name + " ====  " + state + "</color>");
                currentAttackStart = false;
                return true;
            }
            else
            {
                TickState();
            }
            return false;
        }
        //检测英雄状态
        public bool TickState(bool tick = false)
        {
            if (unitData.IsUnusual) return false;
            if (unitData.isCloneUnit)
            {
                if (targetUnit == null || targetUnit.unitAgent == null)
                {
                    targetUnit = null;
                    TickSearchTargetAttack();
                }
                if (targetUnit == null)
                {
                    StopMove();
                    state = HeroAiEnum.Idle;
                    return false;
                }
            }
            if (aiRefrushCD <= 0 || tick)
            {
                if (aiRefrushCD <= 0)
                {
                    tick = false;
                }
                int dis = 99999;
                int key = 0;
                Dictionary<int, HeroAiEnum> eventDic = new Dictionary<int, HeroAiEnum>();
                // 确定用哪个事件
                foreach (var item in aIHeroConfig.distanceList)
                {
                    if (item.Value > 0 && dis > item.Value && item.Value >= enemyDistance)
                    {
                        if (aIHeroConfig.eventDic.ContainsKey(item.Key))
                        {
                            key = item.Key;
                            eventDic = aIHeroConfig.eventDic[key];
                            if (eventDic != null)
                            {
                                dis = item.Value;
                            }
                        }
                    }
                }
                if (tick)
                {
                    if (currentDis == dis)
                    {
                        currentAttackStart = false;
                        return false;
                    }
                }
                currentDis = dis;

                // 获取随机事件
                if (eventDic != null && eventDic.Count > 0)
                {
                    int maxCount = 0;
                    Dictionary<int, HeroAiEnum> temAiEnum = new Dictionary<int, HeroAiEnum>();
                    foreach (var item in eventDic)
                    {
                        if (maxCount < item.Key) maxCount = item.Key;

                        temAiEnum.Add(item.Key, item.Value);
                    }
                    HeroAiEnum aiEnum = HeroAiEnum.None;
                    while (aiEnum == HeroAiEnum.None)
                    {
                        if (temAiEnum.Count == 0)
                        {
                            // 使用默认状态
                            aiEnum = aIHeroConfig.defaultState;
                            break;
                        }
                        //随机数
                        int randomIndex = Random.Range(0,maxCount);
                        int preIndex = 99999;
                        HeroAiEnum temHeroAiEnum = HeroAiEnum.None;
                        foreach (var item in temAiEnum)
                        {
                            if (randomIndex <= item.Key && preIndex > item.Key)
                            {
                                temHeroAiEnum = item.Value;
                                preIndex = item.Key;
                            }
                        }
                        // 判断是否随机到事件
                        if (!temAiEnum.ContainsKey(preIndex)) continue;
                        // 判断技能是否存在
                        switch (temHeroAiEnum)
                        {
                            case HeroAiEnum.Skill1:
                            case HeroAiEnum.Skill2:
                            case HeroAiEnum.Skill3:
                            case HeroAiEnum.Skill4:
                                {
                                    if (!skillIdDic.ContainsKey(temHeroAiEnum))
                                    {
                                        temAiEnum.Remove(preIndex);
                                        continue;
                                    }
                                }
                                break;
                        }

                        //判断是否cd中的状态
                        if (statePreTime.ContainsKey(temHeroAiEnum) && stateCD.ContainsKey(temHeroAiEnum))
                        {
                            if (LTime.time - statePreTime[temHeroAiEnum] > stateCD[temHeroAiEnum])
                            {
                                aiEnum = temHeroAiEnum;
                                statePreTime[temHeroAiEnum] = LTime.time;
                                break;
                            }
                            else
                            {
                                temAiEnum.Remove(preIndex);
                                continue;
                            }
                        }
                        else
                        {
                            aiEnum = temHeroAiEnum;
                            if (!statePreTime.ContainsKey(temHeroAiEnum))
                            {
                                statePreTime.Add(temHeroAiEnum, LTime.time);
                            }
                            else
                            {
                                statePreTime[temHeroAiEnum] = LTime.time;
                            }
                            break;
                        }
                    }
                    state = aiEnum;
                    aiRefrushCD = aIHeroConfig.timeCD;
                    StopMove();
                    //Loger.Log("<color=red>" + unitData.name + " ====  " + state + "</color>");
                    currentAttackStart = false;
                    return true;
                }
            }
            return false;
        }

        public UnitRVOAgent navMeshAgent;
        //移动到某个点
        public void MoveTo(Vector3 pos)
        {
            if (unitData.disableAttack)
            {
                return;
            }
            targetPosition = unitAgent.GetNearest(pos);
            if (unitData.prop.MovementSpeed > 0)
            {
                aniManager.Do_Run();
                //unitAgent.photonView.RPC("RPC_SoliderMove", PhotonTargets.Others);
            }

            unitAgent.RegisterRVO();
            if (!navMeshAgent.enabled) navMeshAgent.enabled = true;
            navMeshAgent.SetTarget(targetPosition);
        }

        //停止移动
        public void StopMove()
        {
            if (!unitData.disableAttack) aniManager.Do_Idle();
            navMeshAgent.enabled = false;
            //unitAgent.photonView.RPC("RPC_SoliderStopMove", PhotonTargets.Others);
        }

        /// <summary>
        /// 检测使用哪个攻击技能
        /// </summary>
        public void TickSelectSkill()
        {

        }
    }
}