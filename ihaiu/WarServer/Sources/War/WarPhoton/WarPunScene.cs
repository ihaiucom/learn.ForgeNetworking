using Games.PB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrueSync;
using UnityEngine;
using System;

namespace Games.Wars
{
    public class WarPunScene : Photon.PunBehaviour, IPunObservable
    {
        public WarRoom room;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (room == null || room.clientProcessState == WarProcessState.GameOver || room.clientProcessState == WarProcessState.Exit) return;
            room.spawnSolider.OnPhotonSerializeView(stream, info);
            room.surrender.OnPhotonSerializeView(stream, info);

            if (stream.isWriting)
            {
            }
            else
            {
            }
        }

        //// 回收单位 -- 开始
        //public void BeginRecoveryUnit(int unitUid, int operateUid)
        //{
        //    photonView.RPC("RPC_BeginRecoveryUnit", PhotonTargets.All, unitUid, operateUid);
        //}

        //[PunRPC]
        //public void RPC_BeginRecoveryUnit(int unitUid, int operateUid)
        //{
        //    UnitAgent unit = room.clientSceneView.GetUnit(unitUid);
        //    UnitAgent operateUnit = room.clientSceneView.GetUnit(operateUid);
        //    if (unit != null && operateUnit != null)
        //    {
        //        operateUnit.unitControl.OnMoveUnitRecovery(unit);
        //    }
        //}





        // 创建机关
        public void CreateTowerUnit(int cellUid, int avatarId, int legionId, int unitId, int unitLevel)
        {
            if(photonView.isMine)
            {
                RPC_CreateTowerUnit(cellUid, avatarId, legionId, unitId, unitLevel);
            }
            else
            {
                photonView.RPC("RPC_CreateTowerUnit", PhotonTargets.MasterClient, cellUid, avatarId, legionId, unitId, unitLevel);
            }
        }

        [PunRPC]
        public void RPC_CreateTowerUnit(int cellUid, int avatarId, int legionId, int unitId, int unitLevel)
        {
            BuildCellData cell = room.sceneData.GetBuildCell(cellUid);
            if (cell.hasUnit)
            {
                return;
            }

            WarPhotonRoom.CreateUnitTower(legionId, cellUid, unitId, avatarId, unitLevel);


            // 创建机关销毁能量
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(unitId, unitLevel);
            LegionData legionData = room.sceneData.GetLegion(legionId);
            int engeryCost = unitLevelConfig.buildCost * -1;
            if (room.stageType == StageType.PVEActivity || room.stageType == StageType.PVPLadder)
            {
                engeryCost = 0;
            }
            legionData.Energy += engeryCost;
            LegionEnergy(legionData.legionId, legionData.Energy);
        }


        // 能量
        public void LegionEnergy(int legionId, float energy)
        {
            photonView.RPC("RPC_LegionEnergy", PhotonTargets.All, legionId, energy);
        }


        [PunRPC]
        public void RPC_LegionEnergy(int legionId, float energy)
        {
            LegionData legionData = room.sceneData.GetLegion(legionId);
            legionData.Energy = energy;
        }

        public void LegionEnergyAdd(int legionId,  float energyAdd,  Vector3 pos)
        {
            photonView.RPC("RPC_LegionEnergyAdd", PhotonTargets.All, legionId,  energyAdd,  pos);
        }

        [PunRPC]
        public void RPC_LegionEnergyAdd(int legionId,  float energyAdd, Vector3 pos)
        {
            LegionData legionData = room.sceneData.GetLegion(legionId);

            bool needEffect = false;
            // 如果需要能量改变特效
            if (legionData.legionId == room.clientOwnLegionId && energyAdd > 0)
            {
                needEffect = true;
                object[] args = new object[] { legionId, energyAdd };
                // 唯一id           世界坐标           动画结束的回调
                WarUI.Instance.OnShowDieEffect(pos, LegionEnergyAddEnd, args);
            }

            if (!needEffect)
            {
                legionData.Energy += energyAdd;
            }
        }

        public void GameOverPVE(WarOverType overType)
        {
            if(photonView.isMine)
            {
                RPC_GameOverPVE(overType);
                photonView.RPC("RPC_GameOverPVE", PhotonTargets.Others, overType);
            }
        }

        [PunRPC]
        public void RPC_GameOverPVE(WarOverType overType)
        {
            room.sceneData.RPC_GameOverPVE(overType);
        }


        void LegionEnergyAddEnd(object[] args)
        {
            int legionId = (int) args[0];
            int energyAdd = (int)args[1];
            LegionData legionData = room.sceneData.GetLegion(legionId);
            legionData.Energy += energyAdd;
        }




        private void Start()
        {
            room = War.currentRoom;
            room.punScene = this;

            room.spawnSolider.photonView = photonView;
            room.surrender.photonView = photonView;
            

            room.OnStart();
        }

        private void Update()
        {
            if(room.IsGameing)
                room.OnSyncedUpdate();
        }


    }
}