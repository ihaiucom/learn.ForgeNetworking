using FMOD;
using FMOD.Studio;
using FMODUnity;
using Games.PB;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /** 战斗操作单位
     * 英雄控制----移动  释放技能
     *  */
    public partial class WarUnitcontrol : AbstractUnitMonoBehaviour
    {

        StudioEventEmitter studioEventEmitter;
        public override void Init(UnitData unitData)
        {
            base.Init(unitData);
            studioEventEmitter = gameObject.AddComponent<StudioEventEmitter>();
            studioEventEmitter.Event = Game.audio.SoundKey2EventPath( unitData.avatarConfig.audioMove );
            if (attackTimeList == null && aniManager.aniLengthDic.ContainsKey("Attack1"))
            {
                attackTimeList = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    int t = i + 1;
                    attackTimeList[i] = aniManager.aniLengthDic["Attack" + t];
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            studioEventEmitter.Stop();
        }

        #region joy操作
        /// <summary>
        /// 技能朝向
        /// </summary>
        private Vector3     skillPos;
        /// <summary>
        /// 技能落点距离
        /// </summary>
        private float       skillRealPos;
        /// <summary>
        /// 操作单位状态
        /// </summary>
        private WarUIAttackType        moveUnit            = 0;
        /// <summary>
        /// 摇杆位置
        /// </summary>
        private Vector3     joyPos;
        /// <summary>
        /// 移动单位
        /// </summary>
        /// <param name="btid">btid==-1为移动旋钮，否则为移动技能旋钮</param>
        /// <param name="_Pos">技能落点</param>
        /// <param name="_Run">移动或停止</param>
        /// <param name="_RealPos">范围伤害距离单位的距离</param>
        public void UIHandler_OnJoyPos(WarUIAttackType btid, Vector3 _Pos, bool _Run, float _RealPos)
        {
            normalAttackToIdleTime = 0;
            if (unitAgent.unitControl != null)
            {
                if (btid == WarUIAttackType.MovePlay)
                {
                    // 移动单位
                    if (_Run)
                    {
                        unitAgent.StopMove();
                        moveUnit = WarUIAttackType.PlayRun;
                        joyPos = _Pos;
                    }
                    else
                    {
                        moveUnit = WarUIAttackType.PlayStop;
                    }
                }
                else
                {
                    // 操作技能方向
                    skillPos = _Pos;
                    skillRealPos = _RealPos;
                }
            }
        }
        /// <summary>
        /// 技能落点
        /// </summary>
        public Vector3 mSkillShowPos = Vector3.zero;


        #endregion

        #region 使用技能
        /// <summary>
        /// 操控类型
        /// 0表示普攻
        /// 1表示第一个技能
        /// 2表示第二个技能
        /// 目前UI界面只有3个攻击按钮
        /// </summary>
        private WarUIAttackType                     AttackType = WarUIAttackType.None;
        /// <summary>
        /// 按钮状态
        /// 0按下，1抬起，2想移动方向攻击，3自动寻怪，10取消 11过程中
        /// </summary>
        private ButtonState                         buttonState;
        /// <summary>
        /// 弹夹数量
        /// </summary>
        private int                                 bulletCount = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Type">同AttackType的注释</param>
        /// <param name="_OpenClose">同buttonState的注释</param>
        public void OnUseSkill(WarUIAttackType _Type, ButtonState _OpenClose,int bulletCount)
        {
            normalAttackToIdleTime = 0;
            if (AttackType == WarUIAttackType.Cancel && _OpenClose != ButtonState.ButtonDown)
            {
                return;
            }
            this.bulletCount = bulletCount;
            if (unitAgent != null && unitAgent.unitControl != null && unitAgent.skillTipArrow != null)
            {
                if (_Type == WarUIAttackType.Skill1 || _Type == WarUIAttackType.Skill2 || _Type == WarUIAttackType.Normal)
                {
                    AttackType = _Type;
                    buttonState = _OpenClose;
                }
            }
            else
            {
                AttackType = WarUIAttackType.None;
            }
        }
        #endregion

        private int skillId;
        private float[]                 attackTimeList = null;
        public float                    doAttackTime = -1;
        public float                    simpleClickTime = -1;
        public bool                     bDoAttack = false;
        public AnimatorState            animatorState;
        private int                     normalSkillId = 0;

        private float                   normalAttackToIdleTime = 0;

        private void Update()
        {
            if (room == null)
            {
                return;
            }

            if (!room.IsGameing)
            {
                if (unitData.isLive && unitAgent != null && unitAgent.aniManager != null)
                {
                    unitAgent.aniManager.Do_Idle();
                }
                return;
            }

            if (room.displayModel == WarDisplayModel.Story) return;
            
            if (normalAttackToIdleTime > 0)
            {
                normalAttackToIdleTime += LTime.deltaTime;
                if (normalAttackToIdleTime > 2)
                {
                    normalAttackToIdleTime = 0;
                    aniManager.Do_Idle();
                }
            }

            #region 做普攻动作1，2，3，4
            if (bDoAttack && attackTimeList != null)
            {
                switch (animatorState)
                {
                    case AnimatorState.Attack1:
                        if (LTime.time - doAttackTime >= attackTimeList[0])
                        {
                            if (AttackType == WarUIAttackType.None && LTime.time - simpleClickTime > 0.2F && room.skillManager.OnStopSkill(unitAgent, normalSkillId))
                            {
                                // 取消普攻
                                bDoAttack = false;
                                animatorState = AnimatorState.Attack;
                                aniManager.Do_Idle();
                            }
                            else
                            {
                                doAttackTime = LTime.time;
                                aniManager.Do_Attack2();
                                animatorState = AnimatorState.Attack2;
                            }
                        }
                        break;
                    case AnimatorState.Attack2:
                        if (LTime.time - doAttackTime >= attackTimeList[1])
                        {
                            if (AttackType == WarUIAttackType.None && LTime.time - simpleClickTime > 0.2F && room.skillManager.OnStopSkill(unitAgent, normalSkillId))
                            {
                                // 取消普攻
                                bDoAttack = false;
                                animatorState = AnimatorState.Attack;
                                aniManager.Do_Idle();
                            }
                            else
                            {
                                doAttackTime = LTime.time;
                                aniManager.Do_Attack3();
                                animatorState = AnimatorState.Attack3;
                            }
                        }
                        break;
                    case AnimatorState.Attack3:
                        if (LTime.time - doAttackTime >= attackTimeList[2])
                        {
                            if (AttackType == WarUIAttackType.None && LTime.time - simpleClickTime > 0.2F && room.skillManager.OnStopSkill(unitAgent, normalSkillId))
                            {
                                // 取消普攻
                                bDoAttack = false;
                                animatorState = AnimatorState.Attack;
                                aniManager.Do_Idle();
                            }
                            else
                            {
                                doAttackTime = LTime.time;
                                aniManager.Do_Attack4();
                                animatorState = AnimatorState.Attack4;
                                aniManager.Do_RunT();
                            }
                        }
                        break;
                    case AnimatorState.Attack4:
                        if (LTime.time - doAttackTime >= attackTimeList[3])
                        {
                            if (AttackType == WarUIAttackType.None && LTime.time - simpleClickTime > 0.2F && room.skillManager.OnStopSkill(unitAgent, normalSkillId))
                            {
                                // 取消普攻
                                bDoAttack = false;
                                animatorState = AnimatorState.Attack;
                                aniManager.Do_Idle();
                            }
                            else
                            {
                                doAttackTime = LTime.time;
                                aniManager.Do_Attack1();
                                animatorState = AnimatorState.Attack1;
                            }
                        }
                        break;
                    case AnimatorState.Attack:
                        {
                            doAttackTime = LTime.time;
                            aniManager.Do_Attack1();
                            animatorState = AnimatorState.Attack1;
                        }
                        break;
                }
            }
            #endregion
            if (WarUI.Instance.isPVPLadderAuto && room.stageType == StageType.PVPLadder) return;

            //if (unitData.uid != room.clientOperationUnit.unitUid)
            //{
            //    return;
            //}

            if (unitData.IsUnusual)
            {
                unitAgent.skillTipArrow.OnSetHide(unitAgent.rotationQuaternion);
                AttackType = WarUIAttackType.Cancel;
                moveUnit = WarUIAttackType.PlayIdle;
            }
            

            #region 技能操作
            switch (AttackType)
            {
                case WarUIAttackType.Normal:
                    {
                        if (buttonState == ButtonState.ButtonDown)
                        {
                            // 普攻按钮按下
                            buttonState = ButtonState.Aimed;
                            simpleClickTime = LTime.time;
                            if (normalSkillId == 0)
                            {
                                SkillController skillController = unitData.GetSkillByIndex(0);
                                if (skillController != null)
                                {
                                    normalSkillId = skillController.skillId;
                                }
                            }
                        }
                        else if (buttonState == ButtonState.ButtonUp)
                        {
                            // 普攻按钮抬起，停止普攻特效
                            AttackType = WarUIAttackType.None;
                            //room.skillManager.OnStopSkill(unitAgent, normalSkillId);
                            normalAttackToIdleTime = 1;
                        }
                        else
                        {
                            // 普攻无取消，取消即抬起，所以此次表示长按普攻按钮中
                            room.skillManager.Init(unitAgent, normalSkillId, Vector3.zero, null, unitAgent.rotationQuaternion, true, true);

                        }
                    }
                    break;
                case WarUIAttackType.Skill1:
                case WarUIAttackType.Skill2:
                    {
                        bDoAttack = false;
                        if (buttonState == ButtonState.ButtonDown)
                        {
                            // 技能按下
                            #region 设置英雄脚下技能指示标识
                            mSkillShowPos = unitAgent.position;
                            int t = AttackType == WarUIAttackType.Skill1 ? 1 : 2;
                            SkillController skillController = unitAgent.unitData.GetSkillByIndex(t);
                            int selectShow = 0;
                            float radius = 0.3F;
                            float range = 1;
                            if (skillController != null)
                            {
                                skillId = skillController.skillId;
                                SkillInfoConfig skillInfoConfig = room.skillManager.GetSkillInfoConfig(skillId);
                                SkillTriggerUse skillTriggerUse = (SkillTriggerUse)skillInfoConfig.skillTriggerConfig;
                                switch (skillTriggerUse.targetLocation)
                                {
                                    case TargetLocation.TargetCircleArea:
                                        {
                                            selectShow = 1;
                                            SkillTriggerUseTargetCircleArea stutca = (SkillTriggerUseTargetCircleArea)skillTriggerUse.skillTriggerLocation;
                                            radius = stutca.targetCircleAreaDis * 0.1F;
                                            range = stutca.targetFanRadius / 3;
                                            rotePos = stutca.targetCircleAreaDis;
                                        }
                                        break;
                                    case TargetLocation.FanWave:
                                        {
                                            selectShow = 2;
                                            SkillTriggerUseFanWave stufw = (SkillTriggerUseFanWave)skillTriggerUse.skillTriggerLocation;
                                            radius = stufw.targetFanRadius * 0.1F;
                                            rotePos = stufw.targetFanRadius;
                                        }
                                        break;
                                    case TargetLocation.CircularShockwave:
                                        {
                                            selectShow = 3;
                                            SkillTriggerUseCircularShockwave sucs = (SkillTriggerUseCircularShockwave)skillTriggerUse.skillTriggerLocation;
                                            radius = sucs.targetFanRadius * 0.1F;
                                            range = sucs.targetFanRadius / 3;
                                            rotePos = sucs.targetFanRadius;

                                        }
                                        break;
                                    case TargetLocation.Self:
                                        {
                                            selectShow = 3;
                                        }
                                        break;
                                    case TargetLocation.LinearWave:
                                        {
                                            SkillTriggerUseLinearWave stulw = (SkillTriggerUseLinearWave)skillTriggerUse.skillTriggerLocation;
                                            radius = stulw.targetFanRadius * 0.1F;
                                            rotePos = stulw.targetFanRadius;
                                        }
                                        break;
                                }
                            }
                            unitAgent.skillTipArrow.OnSetShow(selectShow, radius, range);
                            #endregion
                            buttonState = ButtonState.Aimed;
                        }
                        else if (buttonState == ButtonState.Cancel)
                        {
                            // 取消释放
                            unitAgent.skillTipArrow.OnSetHide(unitAgent.rotationQuaternion);
                            AttackType = WarUIAttackType.None;
                        }
                        else if (buttonState == ButtonState.LookAt || buttonState == ButtonState.AutoSearch)
                        {
                            // 释放技能
                            Quaternion rotation = unitAgent.skillTipArrow._Tf.rotation;
                            room.skillManager.Init(unitAgent, skillId, mSkillShowPos, null, rotation, false, buttonState == ButtonState.AutoSearch, bulletCount);
                            unitAgent.skillTipArrow.OnSetHide(unitAgent.rotationQuaternion);
                            AttackType = WarUIAttackType.None;
                        }
                        else
                        {
                            // 移动摇杆，控制技能方向及位置
                            if (skillPos.magnitude > 0.001F)
                            {
                                unitAgent.skillTipArrow._Tf.rotation = Quaternion.LookRotation(skillPos);
                            }
                            if (skillRealPos > 1)
                            {
                                skillRealPos = 1;
                            }
                            unitAgent.skillTipArrow._Range.localPosition = new Vector3(0, 0, skillRealPos * rotePos * 0.93F);
                            mSkillShowPos = unitAgent.skillTipArrow._Range.position;

                        }

                    }
                    break;
            }
            #endregion

            #region 摇杆移动操作
            if (moveUnit == WarUIAttackType.PlayRun || moveUnit == WarUIAttackType.PlayStop)
            {
                if (moveUnit == WarUIAttackType.PlayRun)
                {
                    // 移动

                    if (Game.camera.CameraMg.mCameraStatus == CameraStatus.FirstCamera)
                    {
                        // 第一人称操控机关 此次需重新设置操作的单位
                        unitAgent.AnchorRotation.RotateAround(unitAgent.AnchorRotation.position, Vector3.up * joyPos.x, 40 * Time.deltaTime);
                    }
                    else
                    {
                        if (!unitData.IsUnusual)
                        {
                            Quaternion _temQuaternion = Quaternion.LookRotation(joyPos);
                            if (room.skillManager.normalSkill)
                            {
                                float eulerAngles = unitAgent.angleY - _temQuaternion.eulerAngles.y;
                                MoveTo(unitAgent, true, eulerAngles, _temQuaternion, unitData.prop.MovementSpeed * 0.5F, false);
                            }
                            else
                            {
                                MoveTo(unitAgent, false, 0, _temQuaternion, unitData.prop.MovementSpeed, true);
                            }
                        }
                    }
                }
                else
                {
                    // 停止移动
                    if (room.skillManager.normalSkill)
                    {
                        if (animatorState == AnimatorState.Attack4)
                        {
                            aniManager.Do_RunT();
                        }
                        else
                        {
                            unitAgent.aniManager.Do_RunIdle();
                        }
                    }
                    else
                    {
                        unitAgent.aniManager.Do_Idle();
                    }
                    moveUnit = WarUIAttackType.PlayIdle;
                }
            }
            #endregion
        }

        #region 基本设置
        private float   rotePos = 1;

        /// <summary>
        /// 获取技能id
        /// </summary>
        /// <param name="skillIndex"></param>
        /// <returns></returns>
        public int GetSkillId(int skillIndex)
        {
            if (unitData.GetSkillByIndex(skillIndex) == null)
            {
                //LLL.LL("未知的技能 " + skillIndex, "red");
                return -1;
            }
            return unitData.GetSkillByIndex(skillIndex).skillId;
        }

        private float audioMoveTime = 0;
        private float audioMoveTimeConfig = 0.5f;

        /// <summary>
        /// 单位移动
        /// </summary>
        /// <param name="unitAgent">需移动的单位</param>
        /// <param name="heroAtkRun">是否移动攻击</param>
        /// <param name="eulerAngles">移动攻击的角度unitAgent.angleY - _temQuaternion.eulerAngles.y，单位角度-朝向角度</param>
        /// <param name="dir">目前朝向</param>
        /// <param name="speed">速度</param>
        /// <param name="isRotation"></param>
        /// <param name="isNearest"></param>
        public void MoveTo(UnitAgent unitAgent, bool heroAtkRun, float eulerAngles, Quaternion dir, float speed = 12, bool isRotation = true, bool isNearest = true)
        {
            if (unitAgent.forwardObject != null)
            {
                unitAgent.forwardObject.rotation = dir;
            }
            if (heroAtkRun)
            {
                if (animatorState == AnimatorState.Attack4)
                {
                    unitAgent.aniManager.Do_RunT();
                    return;
                }
                else
                {
                    if (eulerAngles < 0)
                    {
                        eulerAngles = 360 + eulerAngles;
                    }
                    if ((eulerAngles >= 0 && eulerAngles < 45) || (eulerAngles > 315 && eulerAngles < 360))
                    {
                        //up
                        unitAgent.aniManager.Do_RunU();
                    }
                    else if (eulerAngles >= 45 && eulerAngles < 135)
                    {
                        //left
                        unitAgent.aniManager.Do_RunL();
                    }
                    else if (eulerAngles >= 135 && eulerAngles < 225)
                    {
                        //Down
                        unitAgent.aniManager.Do_RunD();
                    }
                    else if (eulerAngles >= 225 && eulerAngles < 315)
                    {
                        //Right
                        unitAgent.aniManager.Do_RunR();
                    }
                    else
                    {
                        unitAgent.aniManager.Do_RunU();
                    }
                    audioMoveTimeConfig = 0.8f;
                }
            }
            else
            {
                unitAgent.aniManager.Do_Run();
                audioMoveTimeConfig = 0.4f;
            }

            if (Game.audio.setting.enableSFX)
            {
                audioMoveTime -= LTime.deltaTime;
                if (audioMoveTime <= 0)
                {
                    audioMoveTime = audioMoveTimeConfig;
                    studioEventEmitter.SetVolume(Game.audio.setting.volumeSFX);
                    studioEventEmitter.Play();
                }
            }

            if (unitAgent.photonView.isMine)
            {
                unitAgent.Move(dir, speed, isRotation);
            }
        }





        /** 获取操作的单位 */
        public UnitAgent GetUnitAgent()
        {
            return unitAgent;
        }

        /** 获取操作的单位 */
        public int unitUid
        {
            get
            {
                return unitData.uid;
            }
        }

        /** 获取操作的势力ID */
        public int legionId
        {
            get
            {
                return unitData.legionId;
            }
        }

        /** 获取单位位置 */
        public Vector3 position
        {
            get
            {
                if (unitAgent != null)
                {
                    return unitAgent.position;
                }
                return Vector3.zero;
            }
        }
        #endregion
    }
}
