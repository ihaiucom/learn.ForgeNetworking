using UnityEngine;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/13/2017 3:01:18 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位动作
    /// </summary>
    public partial class UnitAgent
    {

        /// <summary>
        /// 动作--出生
        /// </summary>
        public void ActionBirth()
        {
            // 寻路--注册 RVO
            RegisterRVO();

        }
        /// <summary>
        /// 动作--复活
        /// </summary>
        public void ActionRestLive()
        {
            unitData.isLive = true;

            if (animatorManager != null)
            {
                animatorManager.Play();
                animatorManager.ReLive();
            }

            // 寻路--注册 RVO
            RegisterRVO();
        }


        /// <summary>
        /// 开始维修机关
        /// </summary>
        public void ActionDoingRebuild()
        {
            // 开始维修机关
            if (buildType != UnitBuildType.Mainbase && unitType == UnitType.Build)
            {
                //skillInstantEffect = WarInstantiationEffect.Instance.GetBuildFixEffect("PrefabFx/AttackEffect/Effect_Fix", unitAgent.modelTform);
                //skillInstantEffect.showHideEffect(true);
                buildEffect = room.skillManager.GetEffectFromPool("PrefabFx/AttackEffect/Effect_Fix", modelTform, false, Vector3.zero);
                buildEffect.OnStart(room, 9999);
                room.skillManager.skillBulletList.Add(buildEffect);
                fixTime = LTime.time;
                unitData.isFix = true;
                unitData.isLive = true;
            }


            // 寻路--注册 RVO
            RegisterRVO();
        }

        public void ActionEndRebuild()
        {
            // 停止维修特效
            if (buildEffect != null)
            {
                buildEffect.OnEnd();
            }

            if (unitData.isFix)
            {
                unitData.isFix = false;
            }
        }



        /// <summary>
        /// 动作--待机
        /// </summary>
        public void ActionIdea()
        {

        }

        /// <summary>
        /// 动作--奔跑移动
        /// </summary>
        public void ActionRunMove()
        {

        }







        /// <summary>
        /// 动作--普通攻击
        /// 临时放在视图
        /// </summary>
        /// <param name="attack"></param>
        public void ActionAttack(UnitData target, int skillId)
        {
            if (!unitData.isLive || unitData.isFix)
            {
                return;
            }
            
            if (!unitData.IsUnusual)
            {
                if (room.sceneData.CheckUnitInSafeRegion(target) || target.isCloneUnit) target = null;

                if (target != null && target != unitData && !unitData.IsEmploying)
                {
                    LookAt(target);
                }
                
                room.skillManager.Init(unitAgent, skillId, Vector3.zero, target, unitAgent.rotationQuaternion);
                if (unitBloodBag != null)
                {
                    unitBloodBag.OnShowHide(false);
                }
                if (unitData.IsEmploying)
                {
                    unitData.UseEmployAttackNum();
                }
            }

        }

        public void ActionShowModel()
        {
            if (unitBloodBag != null)
            {
                unitBloodBag.OnShowHide(true);
            }
        }

        /// <summary>
        /// 动作--技能攻击
        /// </summary>
        public void ActionSkill()
        {

        }


        /// <summary>
        /// 动作--死亡
        /// NotAni默认存在死亡动作，非正常死亡之间销毁
        /// </summary>
        public void ActionDeath(bool NotAni = false)
        {
            if (unitData.isFix)
            {
                room.clientOperationUnit.EndRebuildUnit(false);
                ActionEndRebuild();
            }

            if (unitData.prop.IsFreezed)
            {
                unitData.prop.IsFreezed = false;
            }
            if (!NotAni)
            {
                if (unitData.soliderType == UnitSoliderType.Boss)
                {
                    if (room.enterData.deathGrading == 2)
                    {
                        GameObject modelGO  = room.clientRes.GetGameObjectInstall(unitData.avatarConfig.model + "_Death");
                        AddToAnchorRotation(modelGO);
                        AnimatorManager temAni = modelGO.GetComponent<AnimatorManager>();
                        if (temAni == null)
                        {
                            temAni = modelGO.GetComponentInChildren<AnimatorManager>();
                        }
                        if (temAni != null)
                        {
                            animatorManager.gameObject.SetActive(false);
                            temAni.Do_Death();
                        }
                    }
                    else
                    {
                        if (animatorManager != null)
                        {
                            animatorManager.Play();
                            animatorManager.Do_Death();
                        }
                    }
                }
                else
                {
                    if (unitData.unitConfig.deathEffect[room.enterData.deathGrading].Equals("0"))
                    {
                        if (animatorManager != null)
                        {
                            animatorManager.Play();
                            animatorManager.Do_Death();
                        }
                    }
                }
            }

            if (soliderBehaviour != null)
            {
                soliderBehaviour.Death();
            }

            //除了主城之外的塔死亡添加修理图标（现在死亡状态下不可被修理）
            //if(unitData.unitType == UnitType.Build && unitData.buildType != UnitBuildType.Mainbase)
            //{
            //	ShowUnitStatus(true);
            //}

            // 寻路--移除RVO
            RemoveRVO();
            //0.1f后直接移除
            if (NotAni)
            {
                delayDestroySelf = 0.1F;
            }
            else
            {
                if (!unitData.unitConfig.deathEffect[room.enterData.deathGrading].Equals("0") && unitData.soliderType != UnitSoliderType.Boss)
                {
                    delayDestroySelf = 0.1F;
                }
                else
                {
                    delayDestroySelf = 3;
                }
                WarUI.Instance.activityKillNum++;
            }
        }
        private float delayDestroySelf = -1;

        /// <summary>
        /// 开门
        /// </summary>
        public void ActionOpenDoor()
        {
            if (doorBehaviour != null)
            {
                doorBehaviour.OpenDoor();
            }
            aniManager.Do_Push();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void ActionCloseDoor()
        {
            if (doorBehaviour != null)
            {
                doorBehaviour.CloseDoor();
            }
            aniManager.Do_Out();
        }




        /// <summary>
        /// 销毁
        /// </summary>
        public void UnitDestroy()
        {
            OnRelease();

            Destroy(gameObject);
            //if (photonView.isMine)
            //{
            //    PhotonNetwork.Destroy(gameObject);
            //}
        }
    }
}
