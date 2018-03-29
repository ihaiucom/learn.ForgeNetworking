using Games.PB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrueSync;
using UnityEngine;

namespace Games.Wars
{
    public class WarPhotonPlayer : TrueSyncBehaviour
    {
        public WarRoom room;
        public LegionData legionData;
        public int LegionId
        {
            get
            {
                return legionData.legionId;
            }
        }

        public override void OnSyncedStart()
        {
            room = War.currentRoom;
            legionData = room.sceneData.GetLegion(owner.Id);
            TSLoger.LogFormat("OnSyncedStart owner.Name={0}, owner.Id={1}", owner.Name, owner.Id);
        }

        public override void OnSyncedStartLocalPlayer()
        {
            Loger.LogFormat("OnSyncedStartLocalPlayer owner.Name={0}, owner.Id={1}", owner.Name, owner.Id);

            //room.LTime.deltaTime = (float) TrueSyncManager.DeltaTime;

            // 生成英雄
            GenerateHero();
            // 生成主基地
            GenerateMainbase();
            // 初始化建筑格子机关
            GenerateBuildCellInitUnit();

            room.clientViewAgent.CreateUI();
            WarUI.Instance.Init();
            room.clientOperationUnit.SetEasyTouch();

            room.clientOperationUnit.Start();
            room.clientRes.CloseLoadPanel();

            room.clientProcessState = WarProcessState.Gameing;
            WarPhotonRoom.SetRoomState(TSRoomState.Gameing);

        }

        #region 初始化
        /** 生成英雄 */
        private void GenerateHero()
        {
            foreach (WarEnterGroupData group in room.enterData.groupList)
            {
                foreach (WarEnterLegionData legion in group.legionList)
                {
                    if (legion.hero == null || legion.hero.unitId <= 0) continue;
                    Vector3 position = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerPosition();
                    Vector3 rotation = room.sceneData.GetRegion(legion.regionId).GenerateSpawnPlayerRotation();
                    WarEnterUnitData item = legion.hero;
                    List<WarSyncCreateSkill> skills = WarProtoUtil.GenerateSyncSkillList(room, item);
                    CreateHeroUnit(room.UNIT_UID, item.unitId, item.unitLevel, legion.legionId, position, rotation, skills);
                }
            }
        }


        /** 生成主基地 */
        private void GenerateMainbase()
        {
            foreach (WarEnterMainbaseData mainbase in room.enterData.mainbaseList)
            {
                WarEnterUnitData item = mainbase.unit;
                List<WarSyncCreateSkill> skills = WarProtoUtil.GenerateSyncSkillList(room, item);
                CreateMainbaseUnit(room.UNIT_UID, item.unitId, item.avatarId, item.unitLevel, mainbase.legionId, mainbase.position, mainbase.rotation, skills);
            }
        }


        /** 初始化建筑格子机关 */
        private void GenerateBuildCellInitUnit()
        {
            int count = room.stageConfig.buildCellList.Count;

            for (int i = 0; i < count; i++)
            {
                StageBuildCellConfig cell = room.stageConfig.buildCellList[i];
                StageBuildCellUnitConfig item = cell.initUnit;
                if (item.HasSetting)
                {
                    List<WarSyncCreateSkill> skills = WarProtoUtil.GenerateSyncSkillList(room, item.unitId, item.unitLevel);
                    CreateTowerUnit(cell.uid, room.UNIT_UID, item.unitId, item.avatarId, item.unitLevel, item.legionId, cell.position, cell.rotation, skills);
                }
            }
        }


        /** 创建英雄单位 */
        public void CreateHeroUnit(int uid, int unitId, int unitLevel, int legionId, Vector3 position, Vector3 rotation, List<WarSyncCreateSkill> skills)
        {
            WarSyncCreateUnit msg = WarProtoUtil.GenerateUnit(uid, unitId, 0, unitLevel, legionId, position, rotation, skills);
            room.clientNetS.CreateUnit(msg);
        }


        /** 创建主基地单位 */
        public void CreateMainbaseUnit(int uid, int unitId, int avatarId, int unitLevel, int legionId, Vector3 position, Vector3 rotation, List<WarSyncCreateSkill> skills)
        {
            WarSyncCreateUnit msg = WarProtoUtil.GenerateUnit(uid, unitId, avatarId, unitLevel, legionId, position, rotation, skills);
            room.clientNetS.CreateUnit(msg);
        }


        /** 创建机关单位 */
        public void CreateTowerUnit(int cellUid, int uid, int unitId, int avatarId, int unitLevel, int legionId, Vector3 position, Vector3 rotation, List<WarSyncCreateSkill> skills)
        {
            WarSyncCreateTowerUnit msg = WarProtoUtil.CreateTowerUnit(cellUid, uid, unitId, avatarId, unitLevel, legionId, position, rotation, skills);
            room.clientNetS.CreateTowerUnit(msg);
        }
        #endregion




        public override void OnSyncedInput()
        {
            // 同步技能操作
            //TrueSyncInput.SetBool(WarTrueSyncInputKey.PLAYER_JOY_ISCHANGE, room.clientOperationUnit.syncJoySkill.isChange);

            //if (room.clientOperationUnit.syncJoySkill.isChange)
            //{
            //    WarSyncUnit msg = new WarSyncUnit();
            //    msg.unitUid = room.clientOperationUnit.unitUid;
            //    msg.unitActionType = WarUnitActionType.Joy;
            //    msg.joy = room.clientOperationUnit.syncJoySkill;

            //    TrueSyncInput.SetByteArray(WarTrueSyncInputKey.PLAYER_JOY, WarProtoUtil.SerializerSyncMsg<WarSyncUnit>(msg));


            //    room.clientOperationUnit.syncJoySkill = new WarSyncUnitJoy();

            //}

            // 同步移动操作
            //TrueSyncInput.SetBool(WarTrueSyncInputKey.PLAYER_JOYMOVE_ISCHANGE, room.clientOperationUnit.syncJoyMove.isChange);

            //if (room.clientOperationUnit.syncJoyMove.isChange)
            //{
            //    WarSyncUnit msg = new WarSyncUnit();
            //    msg.unitUid = room.clientOperationUnit.unitUid;
            //    msg.unitActionType = WarUnitActionType.JoyMove;
            //    msg.joyMove = room.clientOperationUnit.syncJoyMove;


            //    TrueSyncInput.SetByteArray(WarTrueSyncInputKey.PLAYER_JOYMOVE, WarProtoUtil.SerializerSyncMsg<WarSyncUnit>(msg));


            //    room.clientOperationUnit.syncJoyMove = new WarSyncUnitJoy();
            //}

            // 同步跳过波次操作
            //TrueSyncInput.SetByte(WarTrueSyncInputKey.SPAWN_WAVE_SKIP, (byte) room.clientOperationUnit.syncSpawnWaveSkipState);



            // 同步投降操作
            //TrueSyncInput.SetByte(WarTrueSyncInputKey.SURRENDER, (byte)room.clientOperationUnit.syncSurrenderState);



            // 同步创建机关
            //TrueSyncInput.SetBool(WarTrueSyncInputKey.TOWER_CREATE, room.clientOperationUnit.syncCreateTower != null);
            //if(room.clientOperationUnit.syncCreateTower != null)
            //{
            //    TrueSyncInput.SetByteArray(WarTrueSyncInputKey.TOWER_CREATE_MSG, WarProtoUtil.SerializerSyncMsg<WarSyncCreateTowerUnit>(room.clientOperationUnit.syncCreateTower));
            //    room.clientOperationUnit.syncCreateTower = null;
            //}

            // 同步回收单位位操作
            //TrueSyncInput.SetBool(WarTrueSyncInputKey.RECOVERY_UNIT, room.clientOperationUnit.syncRecoveryUnit != null);
            //if (room.clientOperationUnit.syncRecoveryUnit != null)
            //{
            //    TrueSyncInput.SetByteArray(WarTrueSyncInputKey.RECOVERY_UNIT_MSG, WarProtoUtil.SerializerSyncMsg<WarSyncRecoveryUnit>(room.clientOperationUnit.syncRecoveryUnit));
            //    room.clientOperationUnit.syncRecoveryUnit = null;
            //}
        }

        public void OnSyncedUpdateLocalPlayer()
        {
            //room.LTime.frameCount++;
            //room.LTime.time += room.LTime.deltaTime;

            room.OnSyncedUpdate();

            //room.clientOperationUnit.syncSpawnWaveSkipState = TSBallotState.None;
            //room.clientOperationUnit.syncSurrenderState = TSBallotState.None;
        }

        public override void OnSyncedUpdate()
        {
            if (owner.Id == localOwner.Id)
            {
                OnSyncedUpdateLocalPlayer();
            }

            // 同步技能操作
            bool joyChange = TrueSyncInput.GetBool(WarTrueSyncInputKey.PLAYER_JOY_ISCHANGE);
            if (joyChange)
            {
                byte[] bytes = TrueSyncInput.GetByteArray(WarTrueSyncInputKey.PLAYER_JOY);
                WarSyncUnit msg =WarProtoUtil.DeserializeSyncMsg<WarSyncUnit>(bytes);
                room.clientNetS.UnitHero(msg);
                TSLoger.LogFormat("同步技能操作 LegionId={0}, msg.unitUid={1}, msg.unitActionType={2}", LegionId, msg.unitUid, msg.unitActionType);
            }


            // 同步移动操作
            bool joyMoveChange = TrueSyncInput.GetBool(WarTrueSyncInputKey.PLAYER_JOYMOVE_ISCHANGE);
            if (joyMoveChange)
            {
                byte[] bytes = TrueSyncInput.GetByteArray(WarTrueSyncInputKey.PLAYER_JOYMOVE);
                WarSyncUnit msg = WarProtoUtil.DeserializeSyncMsg<WarSyncUnit>(bytes);
                room.clientNetS.UnitHero(msg);
            }


            // 同步跳过波次操作
            TSBallotState syncSpawnWaveSkipState = (TSBallotState) TrueSyncInput.GetByte(WarTrueSyncInputKey.SPAWN_WAVE_SKIP);
            if (syncSpawnWaveSkipState != TSBallotState.None)
            {
                TSLoger.LogFormat("同步跳过波次操作 LegionId={0}", LegionId);
                room.spawnSolider.OnSetSkip(LegionId);
            }


            // 同步投降操作
            TSBallotState syncSurrenderState = (TSBallotState)TrueSyncInput.GetByte(WarTrueSyncInputKey.SURRENDER);
            if (syncSurrenderState != TSBallotState.None)
            {
                TSLoger.LogFormat("同步投降操作 LegionId={0}, syncSurrenderState={1}", LegionId, syncSurrenderState);
                room.surrender.OnBattlot(LegionId, syncSurrenderState);
            }


            // 同步创建机关
            bool hasCreateTower = TrueSyncInput.GetBool(WarTrueSyncInputKey.TOWER_CREATE);
            if (hasCreateTower)
            {
                byte[] bytes = TrueSyncInput.GetByteArray(WarTrueSyncInputKey.TOWER_CREATE_MSG);
                WarSyncCreateTowerUnit msg = WarProtoUtil.DeserializeSyncMsg<WarSyncCreateTowerUnit>(bytes);
                room.clientNetS.CreateTowerUnitNeedSetSkills(msg);
                TSLoger.LogFormat("同步创建机关 LegionId={0}, msg.cellUid={1}, msg.unit.unitId={2}", LegionId, msg.cellUid, msg.unit.unitId);
            }

            // 同步回收单位位操作
            bool hasRecoveryUnit = TrueSyncInput.GetBool(WarTrueSyncInputKey.RECOVERY_UNIT);
            if (hasRecoveryUnit)
            {
                byte[] bytes = TrueSyncInput.GetByteArray(WarTrueSyncInputKey.RECOVERY_UNIT_MSG);
                WarSyncRecoveryUnit msg = WarProtoUtil.DeserializeSyncMsg<WarSyncRecoveryUnit>(bytes);
                room.clientNetS.BeginRecoveryUnit(msg);

                TSLoger.LogFormat("同步回收单位位操作 LegionId={0}, msg.towerUid={1}, msg.operateHeroUid={2}", LegionId, msg.towerUid, msg.operateHeroUid);
            }
        }
    }
}