using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class WarRoomSkillManager : AbstractRoomObject
    {

        public WarRoomSkillManager(WarRoom room)
        {
            this.room = room;
        }
        /// <summary>
        /// 执行中的技能列表
        /// </summary>
        public List<WarSkill>           warSkillList        = new List<WarSkill>();
        /// <summary>
        /// 是否普攻中
        /// </summary>
        public bool                     normalSkill         = false;
        /// <summary>
        /// 特效对象池
        /// </summary>
        public List<SkillBullet>        poolSkillEffect     = new List<SkillBullet>();
        /// <summary>
        /// 子弹列表
        /// </summary>
        public List<SkillBullet>        skillBulletList     = new List<SkillBullet>();

        /// <summary>
        /// 主动技能调用，同时同一单位只能触发同一个技能
        /// </summary>
        /// <param name="unit">使用技能的单位</param>
        /// <param name="skillId">技能id</param>
        /// <param name="attackPos">攻击点</param>
        /// <param name="attackTarget">攻击的目标</param>
        /// <param name="normalSkill">是否英雄的普攻，默认不是</param>
        /// <param name="auto">是否自动</param>
        /// <param name="bulletCount">子弹弹夹数量</param>
        public void Init(UnitAgent unit, int skillId, Vector3 attackPos, UnitData attackTarget, Quaternion quaternion, bool normalSkill = false, bool auto = false, int bulletCount = 0)
        {
            if (unit.unitType == UnitType.Hero && !normalSkill)
            {
                if (unit != null && unit.currentRayAttackEffect != null)
                {
                    unit.currentRayAttackEffect.OnEnd();
                }
                WarSkill normalWarSkill = warSkillList.Find(m => m.actionUnitId == unit.uid && m.normalSkill);
                if (normalWarSkill != null)
                {
                    if (normalSkill)
                    {
                        if (normalWarSkill.actionUnitAgent != null && normalWarSkill.actionUnitAgent.currentRayAttackEffect != null)
                        {
                            normalWarSkill.actionUnitAgent.currentRayAttackEffect.OnEnd();
                        }
                    }
                    OnFinish(normalWarSkill);
                    // 存在英雄普攻，停止
                    // 需要停止普攻的特效 ---- 如果普攻特效已经产生
                    // 然后停止普攻
                }
            }
            WarSkill warSkill = warSkillList.Find(m => m.actionUnitId == unit.uid && m.skillInfoConfig.skillId == skillId);
            if (warSkill == null)
            {
                SkillInfoConfig skillInfoConfig = GetSkillInfoConfig(skillId);
                warSkill = new WarSkill(unit, skillInfoConfig, this, bulletCount);
                warSkill.attackUnit = attackTarget;
                warSkill.attackPos = attackPos;
                warSkill.normalSkill = normalSkill;
                warSkill.quaternion = quaternion;
                warSkill.auto = auto;
                if (normalSkill)
                {
                    this.normalSkill = normalSkill;
                }
                warSkill.StartTigger();
            }
        }
        /// <summary>
        /// 停止技能
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="skillId"></param>
        public bool OnStopSkill(UnitAgent unit, int skillId)
        {
            //if (unit != null && unit.currentRayAttackEffect != null)
            //{
            //    unit.currentRayAttackEffect.OnEnd();
            //}
            List<WarSkill> list = warSkillList.FindAll(m => m.actionUnitId == unit.uid && m.skillInfoConfig.skillId == skillId);
            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    list[i].actionUnitAgent.unitData.invincible = false;
                    warSkillList.Remove(list[i]);
                }
                normalSkill = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 调用被动技能
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="skillId"></param>
        public void Init(UnitAgent unit, int skillId)
        {
            WarSkill warSkill = warSkillList.Find(m => m.actionUnitId == unit.uid && m.skillInfoConfig.skillId == skillId);
            if (warSkill == null)
            {
                SkillInfoConfig skillInfoConfig = GetSkillInfoConfig(skillId);
                warSkill = new WarSkill(unit, skillInfoConfig, this);
                warSkill.StartTigger();
            }
        }
        public void Stop()
        {
            // 关闭所有特效  poolSkillEffect
            if (skillBulletList.Count > 0)
            {
                for (int i = skillBulletList.Count - 1; i >= 0; i--)
                {
                    skillBulletList.RemoveAt(i);
                }
            }
            if (poolSkillEffect.Count > 0)
            {
                for (int i = poolSkillEffect.Count - 1; i >= 0; i--)
                {
                    poolSkillEffect[i].OnEnd();
                }
            }
        }
        /// <summary>
        /// 触发技能
        /// </summary>
        /// <param name="warSkill"></param>
        public void OnTiggerSkill(WarSkill warSkill)
        {
            warSkillList.Add(warSkill);
        }
        /// <summary>
        /// 技能结束
        /// </summary>
        /// <param name="warSkill"></param>
        public void OnFinish(WarSkill warSkill)
        {
            int index = warSkillList.FindIndex(m => m.actionUnitId == warSkill.actionUnitId && m.skillInfoConfig.skillId == warSkill.skillInfoConfig.skillId);
            if (index >= 0)
            {
                if (warSkill.normalSkill) normalSkill = false;
                warSkillList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Tick()
        {
            // 技能
            if (warSkillList.Count > 0)
            {
                for (int i = warSkillList.Count - 1; i >= 0; i--)
                {
                    warSkillList[i].TickAction();
                }
            }
            // 飞行中的子弹列表
            if (skillBulletList.Count > 0)
            {
                for (int i = skillBulletList.Count - 1; i >= 0; i--)
                {
                    skillBulletList[i].Tick();
                }
            }
        }

        #region 搜索目标
        /// <summary>
        /// 获取攻击目标
        /// </summary>
        /// <param name="maxTargetCount"></param>
        /// <returns></returns>
        public List<UnitData> SelectAttackTarget(int maxTargetCount, SkillInfoConfig skillInfoConfig, UnitAgent actionUnitAgent, UnitData attackUnit, Vector3 attackPos, AttackRule attackRuleList, SkillActionConfigDamage skillActionConfigDamage = null)
        {
            List<UnitData> list = new List<UnitData>();
            int count = 0;
            switch (skillInfoConfig.activation)
            {
                case Activation.User:
                    SkillTriggerUse skillTriggerUse = (SkillTriggerUse)skillInfoConfig.skillTriggerConfig;
                    switch (skillTriggerUse.targetLocation)
                    {
                        case TargetLocation.CircularShockwave:
                            {
                                // 对自身周围圆形区域造成伤害
                                SkillTriggerUseCircularShockwave skillTriggerUseCircularShockwave = (SkillTriggerUseCircularShockwave)skillTriggerUse.skillTriggerLocation;
                                list = room.sceneData.SearchMinDistanceUnit(actionUnitAgent.unitData, attackRuleList, skillTriggerUseCircularShockwave.targetMinRadius, skillTriggerUseCircularShockwave.targetFanRadius, maxTargetCount, out count);
                            }
                            break;
                        case TargetLocation.FanWave:
                            {
                                // 对扇形区域造成伤害
                                SkillTriggerUseFanWave skillTriggerUseFanWave = (SkillTriggerUseFanWave)skillTriggerUse.skillTriggerLocation;
                                // 小扇形内目标
                                List<UnitData> listMin = room.sceneData.SearchFanUnit(actionUnitAgent.rotationQuaternion, actionUnitAgent.position, skillTriggerUseFanWave.targetFanAngle, actionUnitAgent.unitData.LegionId, actionUnitAgent.position, attackRuleList, skillTriggerUseFanWave.targetMinRadius, maxTargetCount, out count);
                                // 大扇形内目标
                                list = room.sceneData.SearchFanUnit(actionUnitAgent.rotationQuaternion, actionUnitAgent.position, skillTriggerUseFanWave.targetFanAngle, actionUnitAgent.unitData.LegionId, actionUnitAgent.position, attackRuleList, skillTriggerUseFanWave.targetFanRadius, maxTargetCount, out count);
                                // 去除大扇形内包含的小扇形目标
                                for (int i = list.Count - 1; i >= 0; i--)
                                {
                                    if (listMin.Find(m => m.uid == list[i].uid) != null)
                                    {
                                        list.RemoveAt(i);
                                    }
                                }
                            }
                            break;
                        case TargetLocation.LinearWave:
                            {
                                // 对矩形区域造成伤害
                                SkillTriggerUseLinearWave skillTriggerUseLinearWave = (SkillTriggerUseLinearWave)skillTriggerUse.skillTriggerLocation;
                                //list = room.sceneData.SearchFanUnit(actionUnitAgent.rotationNode.TransformDirection(Vector3.forward), actionUnitAgent.rotationQuaternion, actionUnitAgent.position, skillTriggerUseLinearWave.targetLocationWidth, skillTriggerUseLinearWave.targetMinRadius, skillTriggerUseLinearWave.targetFanRadius, actionUnitAgent.unitData.LegionId, actionUnitAgent.position, attackRuleList, maxTargetCount, out count);
                                list = room.sceneData.SearchFanUnit2(actionUnitAgent.position, skillTriggerUseLinearWave.targetFanRadius, skillTriggerUseLinearWave.targetLocationWidth, actionUnitAgent.unitData.LegionId, attackRuleList, maxTargetCount, out count);
                            }
                            break;
                        case TargetLocation.TargetCircleArea:
                            {
                                // 对目标圆形区域造成伤害
                                SkillTriggerUseTargetCircleArea skillTriggerUseTargetCircleArea = (SkillTriggerUseTargetCircleArea)skillTriggerUse.skillTriggerLocation;
                                if (attackUnit != null)
                                {
                                    attackPos = attackUnit.AnchorAttackbyPos;
                                }
                                list = room.sceneData.SearchMinDistanceUnit(actionUnitAgent.unitData.LegionId, attackPos, attackRuleList, skillTriggerUseTargetCircleArea.targetFanRadius, maxTargetCount, out count);
                            }
                            break;
                        case TargetLocation.Self:
                            {
                                // 对自身造成伤害
                                list.Add(actionUnitAgent.unitData);
                            }
                            break;
                    }
                    break;
                case Activation.Passive:
                    {
                        list = room.sceneData.SearchMinDistanceUnit(actionUnitAgent.unitData, attackRuleList, skillActionConfigDamage.passiveTargetSelect.minRadius, skillActionConfigDamage.passiveTargetSelect.maxRadius, maxTargetCount, out count);
                    }
                    break;
            }
            return list;
        }
        #endregion

        #region 攻击目标
        /// <summary>
        /// 攻击目标
        /// </summary>
        /// <param name="attackByUnit"></param>
        public void OnDamage(UnitAgent actionUnitAgent, UnitData attackByUnit, AttackRule attackRuleList, SkillActionConfigDamage damage, Vector3 pos, int skillLv, int currentSkillId, ref Dictionary<int, int> dic, List<HaloBuff> haloBuffList = null)
        {
            //主体已移除
            if (actionUnitAgent == null) return;


            if (dic.Count > 0 && attackByUnit != null)
            {
                if (dic.ContainsKey(attackByUnit.uid))
                {
                    attackByUnit = null;
                }
            }
            if (attackByUnit != null && !attackByUnit.IsInSceneAndLive)
            {
                attackByUnit = null;
            }
            //List<HaloBuff> haloBuffAdd = null;
            Dictionary<HaloBuff, float> haloBuffAdd = null;
            List<SkillActionConfigBuffCreate> addBuffId = actionUnitAgent.unitData.GetAddBuffId(currentSkillId);
            if (addBuffId.Count > 0)
            {
                haloBuffAdd = new Dictionary<HaloBuff, float>();
                for (int i = 0; i < addBuffId.Count; i++)
                {
                    HaloBuff haloBuff = actionUnitAgent.unitData.GetHaloBuff(addBuffId[i].buffId);
                    haloBuffAdd.Add(actionUnitAgent.unitData.GetHaloBuff(addBuffId[i].buffId), addBuffId[i].buffLife);
                }
            }

            if (!damage.onlySecondDamage)
            {
                if (attackByUnit != null)
                {
                    // 触发被动
                    if (damage.passiveJudgment)
                    {
                        UIHandler.OnHandPassiveed(Triggercondition.OnAttackTarget, actionUnitAgent.unitData, attackByUnit);
                    }
                    DamageData damageData = new DamageData();
                    damageData.AttackSend = actionUnitAgent.unitData;
                    damageData.damageInfBaseCSV = damage.damageInfBaseCSV;
                    damageData.damageInfBaseCSV.skillLv = skillLv;
                    if (damageData.damageInfBaseCSV.selectTarget == SelectTarget.Self)
                    {
                        actionUnitAgent.unitData.GetDamageInfoVal(ref damageData.damageInfBaseCSV);
                    }

                    attackByUnit.OnTakeDamage(damageData, haloBuffList);
                    // 生成buff
                    if (damage.buffid > 0)
                    {
                        HaloBuff haloBuff = actionUnitAgent.unitData.GetHaloBuff(damage.buffid);
                        room.haoleBuff.AddBuffForUnit(attackByUnit, haloBuff, skillLv, actionUnitAgent.unitData, damage.bufflife);
                    }
                    if (haloBuffAdd != null)
                    {
                        foreach (var item in haloBuffAdd)
                        {
                            room.haoleBuff.AddBuffForUnit(attackByUnit, item.Key, skillLv, actionUnitAgent.unitData, item.Value);
                        }
                    }
                    if (!dic.ContainsKey(attackByUnit.uid))
                    {
                        dic.Add(attackByUnit.uid, attackByUnit.uid);
                    }
                }
                // 生成定位光环
                if (damage.haloid > 0)
                {
                    HaloBuff haloBuff = actionUnitAgent.unitData.GetHaloBuff(damage.haloid);
                    room.haoleBuff.AddHalos(actionUnitAgent.unitData.LegionId, attackRuleList, haloBuff, pos, skillLv, actionUnitAgent.unitData, actionUnitAgent.unitData, damage.bufflife);
                }
            }

            if (!damage.haveDamageSecond) return;
            // 此处显示二次效果特效
            if (!string.IsNullOrEmpty(damage.damageSecond.secondEffectPath))
            {
                if (!damage.damageSecond.hitActiveSecond || (damage.damageSecond.hitActiveSecond && attackByUnit != null))
                {
                    SkillBullet showEffects = GetEffectFromPool(damage.damageSecond.secondEffectPath, pos);
                    showEffects.OnStart(room, damage.damageSecond.life);
                    skillBulletList.Add(showEffects);
                }
            }
            if (attackByUnit == null && damage.haveDamageSecond && damage.damageSecond.hitActiveSecond) return;

            if (damage.damageSecond.maxSceondTargets > 0)
            {
                // "播放音效";
                Game.audio.PlaySoundWarSFX(damage.damageSecond.attackSoundPath, pos);

                if (damage.damageSecond.secondaryRadius > 0)
                {
                    int _count = 0;
                    List<UnitData> TemUnitDataList = room.sceneData.SearchUnitListInRule(actionUnitAgent.unitData, attackRuleList.unitType, attackRuleList.relationType, attackRuleList.unitSpaceType, damage.damageSecond.secondaryRadius,pos,out _count,true,damage.damageSecond.warSkillRule,damage.damageSecond.maxSceondTargets);
                    if (attackRuleList.unitType == UnitType.BuildAndPlayer && attackRuleList.unitBuildType != UnitBuildType.All)
                    {
                        TemUnitDataList = TemUnitDataList.FindAll(m => m.unitType == UnitType.Hero || (m.unitType == UnitType.Build && attackRuleList.unitBuildType.UContain(m.buildType)));
                    }
                    if (dic != null && dic.Count > 0)
                    {
                        for (int j = TemUnitDataList.Count - 1; j >= 0; j--)
                        {
                            if (dic.ContainsKey(TemUnitDataList[j].unitData.uid))
                            {
                                TemUnitDataList.RemoveAt(j);
                            }
                        }
                    }
                    _count = TemUnitDataList.Count;
                    if (_count > 0)
                    {
                        DamageData damage2 = new DamageData();
                        damage2.AttackSend = actionUnitAgent.unitData;
                        damage2.damageInfBaseCSV = damage.damageSecond.damageInfBaseCSV;
                        damage2.damageInfBaseCSV.skillLv = skillLv;
                        if (damage2.damageInfBaseCSV.selectTarget == SelectTarget.Self)
                        {
                            actionUnitAgent.unitData.GetDamageInfoVal(ref damage2.damageInfBaseCSV);
                        }
                        HaloBuff haloBuff = null;
                        if (damage.damageSecond.buffid > 0)
                        {
                            haloBuff = actionUnitAgent.unitData.GetHaloBuff(damage.damageSecond.buffid);
                        }
                        for (int l = 0; l < _count; l++)
                        {
                            if (haloBuff != null)
                            {
                                room.haoleBuff.AddBuffForUnit(attackByUnit, haloBuff, skillLv, actionUnitAgent.unitData, damage.damageSecond.bufflife);
                            }
                            if (haloBuffAdd != null)
                            {
                                foreach (var item in haloBuffAdd)
                                {
                                    room.haoleBuff.AddBuffForUnit(attackByUnit, item.Key, skillLv, actionUnitAgent.unitData, item.Value);
                                    room.haoleBuff.AddBuffForUnit(attackByUnit, item.Key, skillLv, actionUnitAgent.unitData, item.Value);
                                }
                            }
                            TemUnitDataList[l].unitData.OnTakeDamage(damage2);
                        }
                    }
                }
            }

        }
        #endregion

        #region 从对象池获取特效
        public SkillBullet GetEffectFromPool(string path, Transform parent, bool followParent, Vector3 offset)
        {
            SkillBullet item = poolSkillEffect.Find(m => m != null && !m.bActive && m.gObject != null && m.path.Equals(path));
            if (item == null)
            {
                item = new SkillBullet();
                item.gObject = room.clientRes.GetGameObjectInstall(path, parent);
                item.tForm = item.gObject.transform;
                item.path = path;
                poolSkillEffect.Add(item);
            }
            if (followParent)
            {
                item.tForm.SetParent(parent);
                item.tForm.localPosition = offset;
            }
            else
            {
                item.tForm.SetParent(parent);
                item.tForm.localPosition = offset;
                item.tForm.localEulerAngles = Vector3.zero;
                item.tForm.SetParent(null);
                //item.tForm.position = parent.position;
                //item.tForm.localPosition += offset;
            }
            item.bActive = true;
            item.tForm.localScale = Vector3.one;
            return item;
        }
        public SkillBullet GetEffectFromPool(string path, Vector3 pos)
        {
            SkillBullet item = poolSkillEffect.Find(m => m != null && !m.bActive && m.gObject != null && m.path.Equals(path));
            if (item == null)
            {
                item = new SkillBullet();
                item.gObject = room.clientRes.GetGameObjectInstall(path);
                item.tForm = item.gObject.transform;
                item.path = path;
                poolSkillEffect.Add(item);
            }
            item.tForm.SetParent(null);
            item.tForm.position = pos;
            return item;
        }
        #endregion

        #region 配置文件读取
        public Dictionary<int, SkillInfoConfig>         configs         = new Dictionary<int, SkillInfoConfig>();
        public SkillInfoConfig GetSkillInfoConfig(int id)
        {
            if (configs.ContainsKey(id))
            {
                return configs[id];
            }

            string josn = Game.asset.LoadConfig("Config/NewSkill/skill_" + id);
            SkillInfoConfig config = HJsonUtility.FromJsonType<SkillInfoConfig>(josn);
            configs.Add(id, config);
            return config;
        }
        #endregion
    }
}