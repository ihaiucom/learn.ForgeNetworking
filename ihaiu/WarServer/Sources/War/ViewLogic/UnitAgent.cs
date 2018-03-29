using Games;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:38:23 PM
*  @Description:    单位视图代理
* ==============================================================================
*/
namespace Games.Wars
{

    /// <summary>
    /// 附加到单位上，为单位附加状态美术特效
    /// commponent 控制器，总管理类
    /// </summary>
    public partial class UnitAgent : AbstractUnitMonoBehaviour
    {
        #region 锚点 Anchor
        private Transform _anchorRotation;
        private Transform _anchorHelp;


        /// <summary>
        /// 朝向节点
        /// </summary>
        public Transform rotationNode
        {
            get
            {
                if (animatorManager != null && animatorManager.rotationNode != null)
                {
                    return animatorManager.rotationNode;
                }
                return AnchorRotation;
            }
        }

        /// <summary>
        /// 有朝向的锚点
        /// </summary>
        public Transform AnchorRotation
        {
            get
            {
                if (_anchorRotation == null)
                {
                    _anchorRotation = transform.Find("AnchorRotation");
                    if (_anchorRotation == null)
                    {
                        _anchorRotation = new GameObject("AnchorRotation").transform;
                        _anchorRotation.SetParent(transform);
                        _anchorRotation.localPosition = Vector3.zero;
                        _anchorRotation.localEulerAngles = Vector3.zero;
                    }
                }
                return _anchorRotation;
            }
        }

        /// <summary>
        /// 辅助锚点
        /// </summary>
        public Transform AnchorHelp
        {
            get
            {
                if (_anchorHelp == null)
                {
                    _anchorHelp = transform.Find("AnchorHelp");
                    if (_anchorHelp == null)
                    {
                        _anchorHelp = new GameObject("AnchorHelp").transform;
                        _anchorHelp.SetParent(transform);
                        _anchorHelp.localPosition = Vector3.zero;
                        _anchorHelp.localEulerAngles = Vector3.zero;
                    }
                }
                return _anchorHelp;
            }
        }


        /// <summary>
        /// 添加到有朝向的锚点
        /// </summary>
        /// <param name="node">节点</param>
        public void AddToAnchorRotation(GameObject node)
        {
            AddToAnchorRotation(node.transform);
        }

        public void AddToAnchorRotation(Transform node)
        {
            node.SetParent(AnchorRotation, false);
            node.localPosition = Vector3.zero;
            node.localEulerAngles = Vector3.zero;
        }


        /// <summary>
        /// 添加到普通的锚点
        /// </summary>
        /// <param name="node">节点</param>
        public void AddToAnchorNormal(GameObject node)
        {
            AddToAnchorNormal(node.transform);
        }

        public void AddToAnchorNormal(Transform node)
        {
            node.SetParent(transform, false);
            node.localPosition = Vector3.zero;
            node.localEulerAngles = Vector3.zero;
        }

        #endregion

        #region 位置 和 方向
        public Vector3 position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }

        public Vector3 rotation
        {
            get
            {
                return rotationNode.eulerAngles;
            }

            set
            {

                //if (unitData!= null && unitType == UnitType.Hero)
                //{
                //Loger.Log(rotationNode.eulerAngles + "rotation " + value);
                //}
                rotationNode.eulerAngles = value;
            }
        }

        public Quaternion rotationQuaternion
        {

            get
            {
                return rotationNode.rotation;
            }

            set
            {

                //if (unitData != null && unitType == UnitType.Hero)
                //{
                //    Loger.Log("rotationQuaternion");
                //}
                rotationNode.rotation = value;
            }
        }

        public float angleY
        {
            get
            {
                return rotationNode.eulerAngles.y;
            }
            set
            {
                rotationNode.eulerAngles = rotationNode.eulerAngles.SetY(value);
            }
        }

        public Vector3 forward
        {
            get
            {
                return rotationNode.forward;
            }
        }

        public void LookAt(Transform node)
        {
            LookAt(node.position);
        }

        public void LookAt(Vector3 position)
        {
            if (position.EqualsXZ(unitData.position)) return;


            unitData.rotation.y = HMath.AngleBetweenVector(unitData.position, position);
            rotation = unitData.rotation;
        }


        /// <summary>
        /// 朝向
        /// </summary>
        /// <param name="target"></param>
        public void LookAt(UnitData target)
        {
            if (!unitData.enableRotation) return;

            //if (aniManager.GetAnimatorState(0) != AnimatorState.Skill1 && aniManager.GetAnimatorState(0) != AnimatorState.Attack)
            {
                unitData.rotation.y = HMath.AngleBetweenVector(unitData.position, target.position);
                rotation = unitData.rotation;
            }
        }
        /// <summary>
        /// 朝向
        /// </summary>
        /// <param name="target"></param>
        public void LookAt(UnitData target, bool pvpLadder)
        {
            if (!unitData.enableRotation) return;

            unitData.rotation.y = HMath.AngleBetweenVector(unitData.position, target.position);
            rotation = unitData.rotation;
            forwardObject.rotation = rotationNode.rotation;
        }

        #endregion


        #region Pathingfinding
        /// <summary>
        /// 朝某个方向移动，需要发送服务器
        /// 不会控制动作
        /// </summary>
        /// <param name="dir">方向</param>
        public void Move(Quaternion dir, float speed = 12, bool isRotation = true, bool isNearest = true)
        {
            AnchorHelp.rotation = dir;
            Vector3 pos = position + AnchorHelp.forward * speed * Time.deltaTime;
            Vector3 nearestDir;
            if (isNearest)
            {
                NNInfo nearest = AstarPath.active.GetNearest(pos, unitData.GetNNConstraint());
                nearestDir = nearest.position - position;
            }
            else
            {
                nearestDir = pos - position;
            }

            if (nearestDir.magnitude > 0)
            {
                if (isRotation) rotationQuaternion = dir;
                AnchorHelp.rotation = Quaternion.LookRotation(nearestDir);
                position = position + AnchorHelp.forward * speed * Time.deltaTime;
            }
        }
        public void Move(UnitHeroAI.HeroAiEnum state, float speed = 12)
        {
            Vector3 pos = position;
            switch (state)
            {
                case UnitHeroAI.HeroAiEnum.AttackFoward:
                case UnitHeroAI.HeroAiEnum.MoveFoward:
                    {
                        pos += AnchorRotation.forward * speed * Time.deltaTime;
                    }
                    break;
                case UnitHeroAI.HeroAiEnum.AttackBack:
                case UnitHeroAI.HeroAiEnum.MoveBack:
                    {
                        pos -= AnchorRotation.forward * speed * Time.deltaTime;
                    }
                    break;
                case UnitHeroAI.HeroAiEnum.AttackRight:
                case UnitHeroAI.HeroAiEnum.MoveRight:
                    {
                        pos += AnchorRotation.right * speed * Time.deltaTime;
                    }
                    break;
                case UnitHeroAI.HeroAiEnum.AttackLeft:
                case UnitHeroAI.HeroAiEnum.MoveLeft:
                    {
                        pos -= AnchorRotation.right * speed * Time.deltaTime;
                    }
                    break;
            }
            Vector3 v = pos - position;

            Quaternion _temQuaternion = Quaternion.LookRotation(v.normalized);
            Move(_temQuaternion, speed);

            //NNInfo nearest = AstarPath.active.GetNearest(pos, unitData.GetNNConstraint());
            //Vector3 nearestDir = nearest.position - position;
            //if (nearestDir.magnitude > 0)
            //{
            //    position = pos;
            //}
        }

        /// <summary>
        /// 移动到某个点，需要发送服务器
        /// </summary>
        /// <param name="pos">坐标</param>
        /// <param name="speed">速度</param>
        /// <param name="arriveDistance">到达距离</param>
        /// <param name="isRotation">是否更新朝向</param>
        /// <param name="onArrive">到达回调</param>
        public void Move(Vector3 pos, float speed = 12, float arriveDistance = 2, bool isRotation = true, Action onArrive = null)
        {
            if (moveAstar != null)
            {
                moveAstar.isRotation = isRotation;
                moveAstar.MoveTo(pos, speed, arriveDistance, onArrive);
            }
        }

        public void StopMove()
        {
            if (moveAstar != null)
            {
                moveAstar.StopMove();
            }

        }

        /// <summary>
        /// 获取单位在寻路数据中最近的点
        /// </summary>
        public Vector3 GetNearest()
        {
            NNInfo nearest = AstarPath.active.GetNearest(position);
            return nearest.position;
        }


        /// <summary>
        /// 获取该点在寻路数据中最近的点
        /// </summary>
        public Vector3 GetNearest(Vector3 pos)
        {
            if (unitData == null) return transform.position;

            NNInfo nearest = AstarPath.active.GetNearest(pos, unitData.GetNNConstraint());
            return nearest.position;
        }


        /** 寻路--注册RVO */
        public void RegisterRVO()
        {
            if (rvoController != null && !rvoController.enabled)
                rvoController.enabled = true;
        }

        /** 寻路--注销RVO */
        public void RemoveRVO()
        {
            if (rvoController != null && rvoController.enabled)
                rvoController.enabled = false;
        }
        #endregion




        /// <summary>
        /// 对象池释放
        /// </summary>
        protected override void OnRelease()
        {
            base.OnRelease();
            OnReleaseComponet();
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="unitData"></param>
        bool isInited = false;
        public override void Init(UnitData unitData)
        {
            base.Init(unitData);
            OnInitComponent(unitData);
            ActionBirth();

            position = GetNearest();
            isInited = true;
        }


        // =====================================
        // ISyncedUpdate
        // -------------------------------------

        private SkillBullet showEffects = null;
        private bool        gameIsGameing = false;

        /** 游戏--同步更新 */
        private void Update()
        {

            if (gameIsGameing != room.gameIsPause)
            {
                gameIsGameing = room.gameIsPause;
                if (animatorManager != null)
                {
                    if (!gameIsGameing)
                    {
                        animatorManager.Play();
                    }
                    else
                    {
                        animatorManager.Pause();
                    }
                }
            }
            if (delayDestroySelf > 0)
            {
                if (unitData.isCloneUnit)
                {
                    if (showEffects == null)
                    {
                        showEffects = room.skillManager.GetEffectFromPool("PrefabFx/AttackEffect/FX_Gunner_xiaoshi", modelTform, false, Vector3.zero);
                        showEffects.OnStart(room, 2);
                        room.skillManager.skillBulletList.Add(showEffects);
                        UnitDestroy();
                    }
                }
                else
                {
                    if (showEffects == null)
                    {
                        if (!unitData.unitConfig.deathEffect[room.enterData.deathGrading].Equals("0"))
                        {
                            showEffects = room.skillManager.GetEffectFromPool(unitData.unitConfig.deathEffect[room.enterData.deathGrading], modelTform, false, Vector3.zero);
                            showEffects.OnStart(room, 2);
                            room.skillManager.skillBulletList.Add(showEffects);
                        }
                    }
                    delayDestroySelf -= LTime.deltaTime;
                    if (delayDestroySelf <= 0)
                    {
                        UnitDestroy();
                    }
                }
                return;
            }
            else if (unitData.isCloneUnit)
            {
                if (room == null || !room.IsGameing)
                    return;
                if (unitData.prop.Hp > 0)
                {
                    if (room.LTime.time - unitData.lifeDelateTime > 1)
                    {
                        unitData.prop.Hp -= unitData.reduceHpPerTime;
                        unitData.lifeDelateTime = room.LTime.time;
                    }
                }
                else
                {
                    punUnit.Death(-1);
                }
            }
            if (room == null || !room.IsGameing)
            {
                if (animatorManager != null)
                {
                    animatorManager.Do_Idle();
                }
                return;
            }
            OnSyncedUpdateSelf();
            OnSyncedUpdateComponet();
        }

        // =====================================
        // IGamePause
        // -------------------------------------
        /** 游戏--暂停 */
        override public void OnGamePause()
        {
            base.OnGamePause();
        }

        /** 游戏--继续 */
        override public void OnGameUnPause()
        {

        }


        /** 游戏--游戏结束 */
        override public void OnGameOver()
        {
            OnGameOverComponet();
        }

        /// <summary>
        /// 修理时间
        /// </summary>
        private float   fixTime = 0;
        private bool preIsFreed = false;
        private void OnSyncedUpdateSelf()
        {
            if (isInited)
            {
                if (unitData.isLive && preIsFreed != unitData.prop.IsFreezed)
                {
                    if (animatorManager != null && !unitData.isCloneUnit)
                    {
                        if (unitData.prop.IsFreezed)
                        {
                            animatorManager.Pause();
                        }
                        else
                        {
                            animatorManager.Play();
                        }
                    }
                    preIsFreed = unitData.prop.IsFreezed;

                }
            }
            if (unitData.isLive && unitData.isFix)
            {
                if (LTime.time - fixTime >= 0.5F)
                {
                    fixTime = LTime.time;
                    float val = room.clientOperationUnit.GetUnitData().unitLevelConfig.GetPropVal(PropId.MagicAttack);
                    unitData.prop.Hp += val * 0.5F;
                    WarUI.Instance.OnBloodDame(unitData.BloodStartPos, (int)val * -1, false, PropId.Hp);
                }
                if (unitData.prop.Hp >= unitData.prop.HpMax)
                {
                    unitData.prop.Hp = unitData.prop.HpMax;
                    room.clientOperationUnit.EndRebuildUnit(true);
                }
            }
            // 受击变色后的还原
            if (byAttackTime >= 0)
            {
                byAttackTime -= LTime.deltaTime;
                if (byAttackTime <= 0)
                {
                    byAttackTime = -1;
                    getNormalShader();
                }
            }

        }

        private float   byAttackTime = -1;
        public void OnHitBy()
        {
            getByAttackShader();
            byAttackTime = 0.05F;
        }


        private GameObject _unitStatus;  //单位状态栏，用3d ui 和李哥的warui 分离开来 减少频繁的mesh batch 
        public GameObject UnitStatus
        {
            get
            {
                if (_unitStatus == null)
                {
                    _unitStatus = room.clientViewAgent.CreateUnitStatus();
                    _unitStatus.transform.SetParent(transform);
                    _unitStatus.transform.localPosition = Vector3.zero + new Vector3(0, 4.5f, 0);
                    _unitStatus.transform.localEulerAngles = new Vector3(Game.camera.CameraMg.CameraAngles.x, Game.camera.CameraMg.CameraAngles.y, 0);
                    _unitStatus.GetComponent<Canvas>().worldCamera = Game.camera.main;
                    _unitStatus.transform.Find("Icon").GetComponent<Button>().onClick.AddListener(RebuildUnit);
                }
                return _unitStatus;
            }
        }

        public void RebuildUnit()
        {
            room.clientOperationUnit.BeginRebuildUnit(unitData.uid);
        }

        public void ShowUnitStatus(bool bShow)
        {
            if (bShow)
            {
                UnitStatus.SetActive(bShow);
            }
            else
            {
                if (_unitStatus != null)
                {
                    UnitStatus.SetActive(bShow);
                }

            }
        }


    }
}
