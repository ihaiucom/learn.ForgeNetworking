using Games.PB;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/15/2017 10:32:55 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarClientNetC : AbstractRoomObject
    {
        public WarClientNetC(WarRoom room)
        {
            this.room = room;
        }



        /** 设置房间状态 */
        public void SetRoomState(int legionId, WarProcessState state, int loaderProgress = -1)
        {
            WarSyncRoomState msg = new WarSyncRoomState();
            msg.legionId = legionId;
            msg.state =(int) state;
            msg.loadProgress = loaderProgress;
            room.clientNet.Send<WarSyncRoomState>(WarSyncActionType.RoomState, msg);
        }



        /** 创建机关单位 */
        public void CreateTowerUnit(int cellUid, int unitId, int unitLevel, int legionId, Vector3 position, Vector3 rotation)
        {
            WarSyncCreateTowerUnit msg = WarProtoUtil.CreateTowerUnit(cellUid, -1, unitId, 0, unitLevel, legionId, position, rotation, null);
            room.clientNet.Send<WarSyncCreateTowerUnit>(WarSyncActionType.CreateTowerUnit, msg);
        }

        /** 设置单位是否活着 */
        public void SetUnitIsLive(int uid, bool isLive)
        {
            WarSyncSetUnitIsLive msg = new WarSyncSetUnitIsLive();
            msg.uid = uid;
            msg.isLive = isLive;
            room.clientNet.Send<WarSyncSetUnitIsLive>(WarSyncActionType.SetUnitIsLive, msg);
        }


        /** 销毁单位 */
        public void RemoveUnit(int uid)
        {
            WarSyncRemoveUnit msg = new WarSyncRemoveUnit();
            msg.uid = uid;
            room.clientNet.Send<WarSyncRemoveUnit>(WarSyncActionType.RemoveUnit, msg);
        }




        /** 客户端发送单位死亡 */
        public void ClientSendUnitDeath(int unitUid, int attackLegionId)
        {
            WarSyncClientSendUnitDeath msg = new WarSyncClientSendUnitDeath();
            msg.unitUid = unitUid;
            msg.attackLegionId = attackLegionId;
            room.clientNet.Send<WarSyncClientSendUnitDeath>(WarSyncActionType.ClientSendUnitDeath, msg);
        }



        //===================================================
        // 战斗波次
        //---------------------------------------------------
        #region 士兵波次
        /** 士兵波次--跳过准备 */
        public void SoliderWaveSkip()
        {
            Loger.LogFormat("SkipWave 发送跳过波次 clientOwnLegionId={0}", room.clientOwnLegionId);

            WarSyncSoliderWaveSkip msg = new WarSyncSoliderWaveSkip();
            msg.legionId = room.clientOwnLegionId;
            room.clientNet.Send<WarSyncSoliderWaveSkip>(WarSyncActionType.SoliderWaveSkip, msg);
        }

        #endregion



        //===================================================
        // 机关操作
        //---------------------------------------------------
        #region 机关操作 -- 占领

        /** 机关操作--占领--开始 */
        public void BeginOccupyUnit(int towerUid, int operateHeroUid)
        {
            WarSyncBeginOccupyUnit msg = new WarSyncBeginOccupyUnit();
            msg.towerUid        = towerUid;
            msg.operateHeroUid  = operateHeroUid;
            room.clientNet.Send<WarSyncBeginOccupyUnit>(WarSyncActionType.BeginOccupyUnit, msg);
        }

        /** 机关操作--占领--结束 */
        public void EndOccupyUnit(int towerUid, int operateHeroUid, bool isComplete)
        {
            WarSyncEndOccupyUnit msg = new WarSyncEndOccupyUnit();
            msg.towerUid        = towerUid;
            msg.operateHeroUid  = operateHeroUid;
            msg.isComplete      = isComplete;
            room.clientNet.Send<WarSyncEndOccupyUnit>(WarSyncActionType.EndOccupyUnit, msg);
        }

        /** 占领单位 */
        public void OccupyUnit(int towerUid, int legionId)
        {
            WarSyncOccupyUnit msg = new WarSyncOccupyUnit();
            msg.towerUid = towerUid;
            msg.legionId = legionId;
            room.clientNet.Send<WarSyncOccupyUnit>(WarSyncActionType.OccupyUnit, msg);
        }
        #endregion


        #region 机关操作 -- 修理/复活
        /** 机关操作--修理--开始 */
        public void BeginRebuildUnit(int towerUid, int operateHeroUid)
        {
            WarSyncBeginRebuildUnit msg = new WarSyncBeginRebuildUnit();
            msg.towerUid = towerUid;
            msg.operateHeroUid = operateHeroUid;
            room.clientNet.Send<WarSyncBeginRebuildUnit>(WarSyncActionType.BeginRebuildUnit, msg);
        }

        /** 机关操作--修理--结束 */
        public void EndRebuildUnit(int towerUid, int operateHeroUid, bool isComplete)
        {
            WarSyncEndRebuildUnit msg = new WarSyncEndRebuildUnit();
            msg.towerUid = towerUid;
            msg.operateHeroUid = operateHeroUid;
            msg.isComplete = isComplete;
            room.clientNet.Send<WarSyncEndRebuildUnit>(WarSyncActionType.EndRebuildUnit, msg);
        }
        #endregion



        #region 机关操作 -- 雇佣
        /** 机关操作--雇佣 */
        public void BeginEmployUnit(int towerUid, int operateHeroUid)
        {
            WarSyncBeginEmployUnit msg = new WarSyncBeginEmployUnit();
            msg.towerUid = towerUid;
            msg.operateHeroUid = operateHeroUid;
            room.clientNet.Send<WarSyncBeginEmployUnit>(WarSyncActionType.BeginEmployUnit, msg);
        }

        /** 机关操作--雇佣 */
        public void EndEmployUnit(int towerUid)
        {
            WarSyncEndEmployUnit msg = new WarSyncEndEmployUnit();
            msg.towerUid = towerUid;
            room.clientNet.Send<WarSyncEndEmployUnit>(WarSyncActionType.EndEmployUnit, msg);
        }
        #endregion


        #region 机关操作 -- 回收
        /** 机关操作--回收 */
        public void RecoveryUnit(int towerUid, int operateHeroUid)
        {
            WarSyncRecoveryUnit msg = new WarSyncRecoveryUnit();
            msg.towerUid = towerUid;
            msg.operateHeroUid = operateHeroUid;
            room.clientNet.Send<WarSyncRecoveryUnit>(WarSyncActionType.RecoveryUnit, msg);
        }
        #endregion




        #region 单位-- 机关 -- 攻击
        /** 单位-- 机关 -- 攻击 */
        public void TowerAttack(int towerUid, int targetUid, int skillId)
        {
            WarSyncTowerAttack msg = new WarSyncTowerAttack();
            msg.towerUid = towerUid;
            msg.targetUid = targetUid;
            msg.skillId = skillId;
            room.clientNet.Send<WarSyncTowerAttack>(WarSyncActionType.TowerAttack, msg);
        }
        #endregion


        #region 单位-- 英雄

        /** 单位-- 英雄 -- Joy */
        public void HeroJoy()
        {
            //if(room.clientOperationUnit.syncJoy.isChange)
            //{
            //    WarSyncUnit msg = new WarSyncUnit();
            //    msg.unitUid = room.clientOperationUnit.unitUid;
            //    msg.unitActionType = WarUnitActionType.Joy;
            //    msg.joy = room.clientOperationUnit.syncJoy;
            //    room.clientOperationUnit.syncJoy = new WarSyncUnitJoy();
            //    room.clientNet.Send<WarSyncUnit>(WarSyncActionType.UnitHero, msg);
            //}

            //if (room.clientOperationUnit.syncJoyMove.isChange)
            //{
            //    WarSyncUnit msg = new WarSyncUnit();
            //    msg.unitUid = room.clientOperationUnit.unitUid;
            //    msg.unitActionType = WarUnitActionType.JoyMove;
            //    msg.joyMove = room.clientOperationUnit.syncJoyMove;
            //    room.clientOperationUnit.syncJoyMove = new WarSyncUnitJoy();
            //    room.clientNet.Send<WarSyncUnit>(WarSyncActionType.UnitHero, msg);
            //}
        }


        /** 单位-- 英雄 -- 待机 */
        public void HeroIdea()
        {

        }

        /** 单位-- 英雄 -- 移动 */
        public void HeroMove()
        {

        }

        /** 单位-- 英雄 -- 攻击 */
        public void HeroAttack()
        {

        }

        /** 单位-- 英雄 -- 移动攻击 */
        public void HeroMoveAttack()
        {

        }

        #endregion



        #region 单位-- 属性
        /** 单位-- 机关 -- 攻击 */
        public void UnitProp(UnitData unit)
        {
            WarSyncUnitProp msg = new WarSyncUnitProp();
            msg.unitUid = unit.uid;
            msg.hp = unit.prop.Hp;
            room.clientNet.Send<WarSyncUnitProp>(WarSyncActionType.UnitProp, msg);
        }
        #endregion

        //===================================================
        // 投降
        //---------------------------------------------------
        #region 投降
        /** 投降--投票 */
        public void SurrenderBattlot()
        {
            SurrenderBattlot(true);
        }

        public void SurrenderBattlot(bool isYes)
        {
            WarSyncSurrenderBattlot msg = new WarSyncSurrenderBattlot();
            msg.legionId = room.clientOwnLegionId;
            msg.select = isYes ? WarSyncSurrenderSelectType.Yes : WarSyncSurrenderSelectType.No;
            room.clientNet.Send<WarSyncSurrenderBattlot>(WarSyncActionType.SurrenderBattlot, msg);
        }
        #endregion
    }
}
