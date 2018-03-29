using Games.PB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrueSync;
using UnityEngine;
using System;
using com.ihaiu;
using FMOD.Studio;
using FMODUnity;

namespace Games.Wars
{
    public class WarPunHero : Photon.PunBehaviour, IPunObservable
    {
        public WarRoom room;
        public UnitAgent unitAgent;
        public int legionId
        {
            get
            {
                return unitAgent.unitData.legionId;
            }
        }

        public float Energy
        {
            get
            {
                return unitAgent.legionData.Energy;
            }

            set
            {
                if(unitAgent != null)
                    unitAgent.legionData.Energy = value;
            }
        }

        public MoveJoy          syncJoyMove             = new MoveJoy();
        public MoveJoy          syncJoySkill            = new MoveJoy();
        public SkillStateJoy    syncJoySkillState       = new SkillStateJoy();

        public RecoveryUnit syncRecoveryUnit    = new RecoveryUnit();
        public RebuildUnit  syncRebuildUnit     = new RebuildUnit();

        public Ballot syncBallotSpawnWaveSkip   = new Ballot();
        public Ballot syncSurrender             = new Ballot();

        EventInstance eventInstanceRebuild;
        EventInstance eventInstanceAttack;

        private void Start()
        {
            eventInstanceRebuild = Game.audio.CreateSoundInstance(SoundKeys.Builder_Repair);
            if(unitAgent != null && unitAgent.unitData != null && !string.IsNullOrEmpty(unitAgent.unitData.avatarConfig.audioAttackLoop))
            {
                eventInstanceAttack = Game.audio.CreateSoundInstance(unitAgent.unitData.avatarConfig.audioAttackLoop);
            }
        }

        private void OnDestroy()
        {
            eventInstanceRebuild.stop( STOP_MODE.IMMEDIATE );
            eventInstanceRebuild.release();


            eventInstanceAttack.stop(STOP_MODE.IMMEDIATE);
            eventInstanceAttack.release();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                // 移动
                stream.SendNext(room.clientOperationUnit.syncJoyMove.type);
                stream.SendNext(room.clientOperationUnit.syncJoyMove.pos);
                stream.SendNext(room.clientOperationUnit.syncJoyMove.run);
                stream.SendNext(room.clientOperationUnit.syncJoyMove.realPos);
                stream.SendNext(room.clientOperationUnit.syncJoyMove.isSend);
                room.clientOperationUnit.syncJoyMove.isSend = true;


                // 技能
                // skill pos
                stream.SendNext(room.clientOperationUnit.syncJoySkill.type);
                stream.SendNext(room.clientOperationUnit.syncJoySkill.pos);
                stream.SendNext(room.clientOperationUnit.syncJoySkill.run);
                stream.SendNext(room.clientOperationUnit.syncJoySkill.realPos);
                stream.SendNext(room.clientOperationUnit.syncJoySkill.isSend);
                room.clientOperationUnit.syncJoySkill.isSend = true;
                // skill state
                stream.SendNext(room.clientOperationUnit.syncJoySkillState.stateType);
                stream.SendNext(room.clientOperationUnit.syncJoySkillState.openClose);
                stream.SendNext(room.clientOperationUnit.syncJoySkillState.bulletCurrentCount);
                stream.SendNext(room.clientOperationUnit.syncJoySkillState.isSend);
                room.clientOperationUnit.syncJoySkillState.isSend = true;

                // 机关回收
                stream.SendNext(room.clientOperationUnit.syncRecoveryUnit.state);
                stream.SendNext(room.clientOperationUnit.syncRecoveryUnit.towerUid);
                stream.SendNext(room.clientOperationUnit.syncRecoveryUnit.operateHeroUid);
                stream.SendNext(room.clientOperationUnit.syncRecoveryUnit.isSend);
                room.clientOperationUnit.syncRecoveryUnit.isSend = true;

                // 机关维修
                stream.SendNext(room.clientOperationUnit.syncRebuildUnit.state);
                stream.SendNext(room.clientOperationUnit.syncRebuildUnit.towerUid);
                stream.SendNext(room.clientOperationUnit.syncRebuildUnit.operateHeroUid);
                stream.SendNext(room.clientOperationUnit.syncRebuildUnit.isSend);
                room.clientOperationUnit.syncRebuildUnit.isSend = true;



                // 跳过波次
                stream.SendNext(room.clientOperationUnit.syncBallotSpawnWaveSkip.ballot);
                stream.SendNext(room.clientOperationUnit.syncBallotSpawnWaveSkip.isSend);
                room.clientOperationUnit.syncBallotSpawnWaveSkip.isSend = true;


                // 投降投票
                stream.SendNext(room.clientOperationUnit.syncSurrender.ballot);
                stream.SendNext(room.clientOperationUnit.syncSurrender.isSend);
                room.clientOperationUnit.syncSurrender.isSend = true;

                // 能量
                stream.SendNext(Energy);
            }
            else
            {

                // 移动
                syncJoyMove.type = (WarUIAttackType)stream.ReceiveNext();
                syncJoyMove.pos = (Vector3)stream.ReceiveNext();
                syncJoyMove.run = (bool)stream.ReceiveNext();
                syncJoyMove.realPos = (float)stream.ReceiveNext();
                syncJoyMove.isSend = (bool)stream.ReceiveNext();
                if (syncJoyMove.isSend == false)
                    syncJoyMove.isUpdated = false;


                // 技能
                // skill pos
                syncJoySkill.type = (WarUIAttackType)stream.ReceiveNext();
                syncJoySkill.pos = (Vector3)stream.ReceiveNext();
                syncJoySkill.run = (bool)stream.ReceiveNext();
                syncJoySkill.realPos = (float)stream.ReceiveNext();
                syncJoySkill.isSend = (bool)stream.ReceiveNext();
                if (syncJoySkill.isSend == false)
                    syncJoySkill.isUpdated = false;
                // skill state
                syncJoySkillState.stateType = (WarUIAttackType)stream.ReceiveNext();
                syncJoySkillState.openClose = (ButtonState)stream.ReceiveNext();
                syncJoySkillState.bulletCurrentCount = (int)stream.ReceiveNext();
                syncJoySkillState.isSend = (bool)stream.ReceiveNext();
                if(syncJoySkillState.isSend == false)
                    syncJoySkillState.isUpdated = false;



                // 机关维修
                syncRecoveryUnit.state = (AsyncOperateState)stream.ReceiveNext();
                syncRecoveryUnit.towerUid           = (int)stream.ReceiveNext();
                syncRecoveryUnit.operateHeroUid     = (int)stream.ReceiveNext();
                syncRecoveryUnit.isSend             = (bool)stream.ReceiveNext();
                if (syncRecoveryUnit.isSend == false)
                    syncRecoveryUnit.isUpdated = false;



                // 机关回收
                syncRebuildUnit.state = (AsyncOperateState)stream.ReceiveNext();
                syncRebuildUnit.towerUid = (int)stream.ReceiveNext();
                syncRebuildUnit.operateHeroUid = (int)stream.ReceiveNext();
                syncRebuildUnit.isSend = (bool)stream.ReceiveNext();
                if (syncRebuildUnit.isSend == false)
                    syncRebuildUnit.isUpdated = false;


                // 跳过波次
                syncBallotSpawnWaveSkip.ballot = (TSBallotState)stream.ReceiveNext();
                syncBallotSpawnWaveSkip.isSend = (bool)stream.ReceiveNext();
                if (syncBallotSpawnWaveSkip.isSend == false)
                    syncBallotSpawnWaveSkip.isUpdated = false;

                // 投降投票
                syncSurrender.ballot = (TSBallotState)stream.ReceiveNext();
                syncSurrender.isSend = (bool)stream.ReceiveNext();
                if (syncSurrender.isSend == false)
                    syncSurrender.isUpdated = false;


                // 能量
                Energy = (float)stream.ReceiveNext();
            }
        }


        void OnGameOver()
        {
            if (eventInstanceAttack.isValid())
            {
                eventInstanceAttack.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }

        private void Update()
        {
            if (unitAgent == null) return;
            if(room.clientProcessState == WarProcessState.GameOver)
            {
                OnGameOver();
            }

            if(photonView.isMine)
            {
                if(unitAgent.unitData.clientIsOwn)
                {
                    syncJoyMove = room.clientOperationUnit.syncJoyMove;
                    syncJoySkill = room.clientOperationUnit.syncJoySkill;
                    syncJoySkillState = room.clientOperationUnit.syncJoySkillState;

                    syncRecoveryUnit = room.clientOperationUnit.syncRecoveryUnit;
                    syncRebuildUnit = room.clientOperationUnit.syncRebuildUnit;

                    syncBallotSpawnWaveSkip = room.clientOperationUnit.syncBallotSpawnWaveSkip;
                    syncSurrender           = room.clientOperationUnit.syncSurrender;
                }
            }


            eventInstanceRebuild.set3DAttributes(RuntimeUtils.To3DAttributes(transform));

            if (eventInstanceAttack.isValid())
            {
                eventInstanceAttack.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
            }

            // 移动
            if (!syncJoyMove.isUpdated)
            {
                unitAgent.warUnitcontrol.UIHandler_OnJoyPos(syncJoyMove.type, syncJoyMove.pos, syncJoyMove.run, syncJoyMove.realPos);
                syncJoyMove.isUpdated = true;
            }

            // 技能
            // 控制技能方向
            if (!syncJoySkill.isUpdated)
            {
                unitAgent.warUnitcontrol.UIHandler_OnJoyPos(syncJoySkill.type, syncJoySkill.pos, syncJoySkill.run, syncJoySkill.realPos);
               
                syncJoySkill.isUpdated = true;
            }
            // 控制技能按钮状态
            if (!syncJoySkillState.isUpdated)
            {
                if ( eventInstanceAttack.isValid())
                {
                    if (syncJoySkillState.stateType == WarUIAttackType.Normal)
                    {
                        if (syncJoySkillState.openClose == ButtonState.ButtonDown && Game.audio.setting.enableSFX)
                        {
                            eventInstanceAttack.setParameterValue("Shoot_Trigge", 0);
                            eventInstanceAttack.setVolume(Game.audio.setting.volumeSFX);
                            eventInstanceAttack.start();
                        }
                        else
                        {
                            eventInstanceAttack.setParameterValue("Shoot_Trigge", 1);
                            eventInstanceAttack.stop( STOP_MODE.ALLOWFADEOUT );
                        }
                    }
                }

                unitAgent.warUnitcontrol.OnUseSkill(syncJoySkillState.stateType, syncJoySkillState.openClose, syncJoySkillState.bulletCurrentCount);
                syncJoySkillState.isUpdated = true;
            }




            // 机关回收

            if (!syncRecoveryUnit.isUpdated)
            {
                UnitAgent unit = room.clientSceneView.GetUnit(syncRecoveryUnit.towerUid);
                UnitAgent operateUnit = room.clientSceneView.GetUnit(syncRecoveryUnit.operateHeroUid);

                switch (syncRecoveryUnit.state)
                {
                    case AsyncOperateState.Begin:
                        if (unit != null && operateUnit != null)
                        {
                            operateUnit.unitControl.OnMoveUnitRecovery(unit);
                        }
                        break;
                    case AsyncOperateState.Complete:
                    case AsyncOperateState.Cancel:
                        operateUnit.animatorManager.Do_Idle();
                        room.clientNetS.EndRecoveryUnit(syncRecoveryUnit.towerUid, syncRecoveryUnit.operateHeroUid);

                        Game.audio.PlaySoundWarSFX(SoundKeys.Builder_Revoke, unit.position);
                        break;
                }

                syncRecoveryUnit.isUpdated = true;
            }

            // 机关维修

            if (!syncRebuildUnit.isUpdated)
            {
                UnitAgent unit = room.clientSceneView.GetUnit(syncRebuildUnit.towerUid);
                UnitAgent operateUnit = room.clientSceneView.GetUnit(syncRebuildUnit.operateHeroUid);

                switch (syncRebuildUnit.state)
                {
                    case AsyncOperateState.Begin:
                        if (unit != null && operateUnit != null)
                        {
                            operateUnit.unitControl.OnMoveToRebuildUnit(unit);
                        }
                        break;
                    case AsyncOperateState.Doing:
                        if (unit != null && operateUnit != null)
                        {
                            if(Game.audio.setting.enableSFX)
                            {
                                eventInstanceRebuild.setVolume(Game.audio.setting.volumeSFX);
                                eventInstanceRebuild.start();
                            }
                            operateUnit.unitControl.DoingRebuildUnit(unit);
                        }
                        break;
                    case AsyncOperateState.Complete:
                    case AsyncOperateState.Cancel:
                        eventInstanceRebuild.stop(STOP_MODE.IMMEDIATE);
                        operateUnit.unitControl.EndRebuildUnit(unit);
                        break;
                }

                syncRebuildUnit.isUpdated = true;
            }

            // 跳过波次
            if (!syncBallotSpawnWaveSkip.isUpdated)
            {
                if(syncBallotSpawnWaveSkip.ballot != TSBallotState.None)
                {
                    room.spawnSolider.OnSetSkip(legionId);
                }
                
                syncBallotSpawnWaveSkip.isUpdated = true;
            }

            // 投降投票
            if (!syncSurrender.isUpdated)
            {
                if (syncSurrender.ballot != TSBallotState.None)
                {
                    room.surrender.OnBattlot(legionId, syncSurrender.ballot);
                }

                syncSurrender.isUpdated = true;
            }

        }




    }
}