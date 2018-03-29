using UnityEngine;
using System.Collections.Generic;
using Pathfinding.RVO;
using System.IO;
using System;
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
    /// 单位组件
    /// </summary>
    public partial class UnitAgent
    {
        private WarUnitcontrol _warUnitcontrol = null;
        /// <summary>
        /// 玩家操作的战斗单位
        /// </summary>
        public WarUnitcontrol warUnitcontrol
        {
            get
            {
                if(_warUnitcontrol == null)
                {
                    _warUnitcontrol = gameObject.AddComponent<WarUnitcontrol>();
                    _warUnitcontrol.Init(unitData);
                }
                return _warUnitcontrol;
            }

            set
            {
                _warUnitcontrol = value;
            }
        }
        /// <summary>
        /// 特效控制器，单位当前被附加的特效，比如冰冻，眩晕等
        /// </summary>
        public UnitStateEffect unitStateEffect;
        /// <summary>
        /// 单位操控
        /// </summary>
        public UnitControl unitControl;
        /// <summary>
        /// 脚下提示方向的Object
        /// </summary>
        public Transform                forwardObject;
        /// <summary>
        /// 技能提示箭头
        /// </summary>
        public  Player_Arrow            skillTipArrow;
        /// <summary>
        /// 动作
        /// </summary>
        public AnimatorManager          animatorManager;
        public UnitBloodBag             unitBloodBag;

        /// <summary>
        /// 移动
        /// </summary>
        public AbstractUnitMonoBehaviour moveBehaviour;

        /// <summary>
        /// 修理特效
        /// </summary>
        private SkillBullet buildEffect = null;


        /// <summary>
        /// 门
        /// </summary>
        public UnitDoor     doorBehaviour;

        /// <summary>
        /// 寻路组件--RVO（注册，和移除）
        /// </summary>
        public RVOController rvoController;


        /// <summary>
        /// 寻路组件--RVO（注册，和移除）
        /// </summary>
        public UnitRVOAgent unitRVOAgent;

        /// <summary>
        /// 移动
        /// </summary>
        public UnitMoveAstar moveAstar;

        public PhotonView               photonView;
        public PhotonTransformView      photonTransformView;
        public WarPunUnit               punUnit;
        public WarPunHero               punHero;

        /// <summary>
        /// 天梯英雄AI
        /// </summary>
        public UnitHeroAI               heroAI;
        /// <summary>
        /// 士兵行为
        /// </summary>
        public UnitSoliderBehaviour soliderBehaviour;

        /// <summary>
        /// 机关行为
        /// </summary>
        public UnitTowerBehaviour   towerBehaviour;

        /// <summary>
        /// 当前准备释放的技能数据
        /// </summary>
        //public SkillDate    CurSkillDate            = null;


        public SkillBullet          currentRayAttackEffect = null;

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void OnInitComponent(UnitData unitData)
        {
            photonView = GetComponent<PhotonView>();
            photonTransformView = GetComponent<PhotonTransformView>();
            if(photonTransformView != null) photonTransformView.rotationNode = rotationNode;
            punUnit = GetComponent<WarPunUnit>();

            if (animatorManager != null)
            {
                animatorManager.UnitUid = unitData.uid;
            }
            switch (unitData.unitType)
            {
                case UnitType.Solider:
                    unitRVOAgent = GetComponent<UnitRVOAgent>();
                    if (unitRVOAgent == null)
                    {
                        unitRVOAgent = AnchorRootGameObject.AddComponent<UnitRVOAgent>();
                    }

                    //士兵行为
                    soliderBehaviour = GetComponent<UnitSoliderBehaviour>();
                    if (soliderBehaviour == null)
                    {
                        soliderBehaviour = AnchorRootGameObject.AddComponent<UnitSoliderBehaviour>();
                    }

                    break;
                case UnitType.Hero:
                    moveAstar = GetComponent<UnitMoveAstar>();
                    if (moveAstar == null)
                    {
                        moveAstar = AnchorRootGameObject.AddComponent<UnitMoveAstar>();
                    }

                    punHero = GetComponent<WarPunHero>();
                    punHero.room = room;
                    punHero.unitAgent = unitAgent;
                    Transform light = transform.Find("Spotlight");
                    if(light != null)
                    {
                        light.gameObject.SetActive(unitData.clientIsOwn);
                    }
                    if (room.stageType == StageType.PVPLadder || unitData.isCloneUnit)
                    {
                        unitRVOAgent = GetComponent<UnitRVOAgent>();
                        if (unitRVOAgent == null)
                        {
                            unitRVOAgent = AnchorRootGameObject.AddComponent<UnitRVOAgent>();
                        }
                        heroAI = GetComponent<UnitHeroAI>();
                        if (heroAI == null)
                        {
                            heroAI = AnchorRootGameObject.AddComponent<UnitHeroAI>();
                        }
                    }
                    break;
                case UnitType.Build:
                    switch (buildType)
                    {
                        case UnitBuildType.Tower_Attack:
                        case UnitBuildType.Tower_Auxiliary:
                            //攻击
                            towerBehaviour = GetComponent<UnitTowerBehaviour>();
                            if (towerBehaviour == null)
                            {
                                towerBehaviour = AnchorRootGameObject.AddComponent<UnitTowerBehaviour>();
                            }
                            break;

                        case UnitBuildType.Tower_Door:
                            doorBehaviour = GetComponent<UnitDoor>();
                            if (doorBehaviour == null)
                            {
                                doorBehaviour = AnchorRootGameObject.AddComponent<UnitDoor>();
                            }
                            break;
                        case UnitBuildType.Mainbase:
                            towerBehaviour = GetComponent<UnitTowerBehaviour>();
                            if (towerBehaviour != null)
                            {
                                towerBehaviour.enabled = false;
                            }
                            break;
                    }

                    // 游戏运行中，添加创建特效
                    if (room.IsGameing)
                    {
                        //WarInstantiationEffect.Instance.GetBuildFixEffect("PrefabFx/AttackEffect/Effect_Build", unitAgent.modelTform, 3);
                        SkillBullet Effect_Build = room.skillManager.GetEffectFromPool("PrefabFx/AttackEffect/Effect_Build", modelTform, false, Vector3.zero);
                        Effect_Build.OnStart(room, 3);
                        room.skillManager.skillBulletList.Add(Effect_Build);
                    }
                    break;
            }


            //附加移动操作组件
            unitControl = GetComponent<UnitControl>();
            if (unitControl == null)
            {
                unitControl = AnchorRootGameObject.AddComponent<UnitControl>();
            }
            //unitControl.OnPassiveEvent(Triggercondition.OnCreated, unitAgent.unitData);
            UIHandler.OnHandPassiveed(Triggercondition.OnCreated, unitAgent.unitData);

            // 寻路组件--RVO（注册，和移除）
            rvoController = GetComponent<RVOController>();
            if(rvoController != null)
            {
                rvoController.radius = unitData.rvoRadius;
            }
            // 脚下移动方向提示，目前仅英雄存在
            if (forwardObject == null)
            {
                forwardObject = GameLoadResources.FindChildByName(AnchorRoot, "PlayFowardTip");
            }
            
            // 技能提示箭头，目前仅英雄存在
            if (skillTipArrow == null)
            {
                Transform _Transform = GameLoadResources.FindChildByName(AnchorRoot, "Arrow", true);
                if (_Transform != null)
                {
                    skillTipArrow = _Transform.GetComponent<Player_Arrow>();
                    skillTipArrow._Target = AnchorRoot;
                }
            }

            //附加特效组件
            unitStateEffect = GetComponent<UnitStateEffect>();
            if (unitStateEffect == null)
            {
                unitStateEffect = AnchorRootGameObject.AddComponent<UnitStateEffect>();
            }

            AbstractUnitMonoBehaviour[] list = GetComponents<AbstractUnitMonoBehaviour>();
            foreach (AbstractUnitMonoBehaviour item in list)
            {
                if (item == this) continue;
                item.unitAgent = this;
                item.Init(unitData);
            }
        }


        /// <summary>
        /// 帧更新组件
        /// </summary>
        private void OnSyncedUpdateComponet()
        {
            //for(int i = 0, count = syncedUpdateList.Count; i < count; i ++)
            //{
            //    try
            //    {
            //        if (syncedUpdateList[i].enabled)
            //            syncedUpdateList[i].OnSyncedUpdate();
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.LogError(e);
            //    }
            //}
        }


        /// <summary>
        /// 释放组件
        /// </summary>
        private void OnReleaseComponet()
        {
        }

        private void OnGameOverComponet()
        {
            AbstractUnitMonoBehaviour[] list = GetComponents<AbstractUnitMonoBehaviour>();
            foreach (AbstractUnitMonoBehaviour item in list)
            {
                if (item == this) continue;
                item.OnGameOver();
            }

        }




    }
}
