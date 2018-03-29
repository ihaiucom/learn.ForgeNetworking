using Games.PB;
using System;
using System.Collections.Generic;
using System.IO;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/30/2017 1:41:03 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarClientNet : AbstractRoomObject
    {

        public WarClientNet(WarRoom room)
        {
            this.room = room;
        }

        public void AddListen()
        {
            if (room.stageType == StageType.Dungeon)
            {
                return;
            }
            //Game.proto.AddCallback<SMSG_SyncLoadingProgress_Ntf>(S_SyncLoadingProgress);
            //Game.proto.AddCallback<SMSG_StartGameTeamCopy_Ntf>(S_StartGameTeamCopy);
            Game.proto.AddCallback<S_BattleSync_16032>(S_BattleSync);
            Game.proto.AddCallback<S_BattleLinestate_16033>(S_BattleLinestate);
            //Game.proto.AddCallback<SMSG_BattleOver_Ntf>(S_BattleOver);

            //C_SyncLoadingProgress(100);
            room.mainThread.updateEvent += OnFrame;
        }

        public void RemoveListen()
        {
            room.mainThread.updateEvent -= OnFrame;
            //Game.proto.RemoveCallback<SMSG_SyncLoadingProgress_Ntf>(S_SyncLoadingProgress);
            //Game.proto.RemoveCallback<SMSG_StartGameTeamCopy_Ntf>(S_StartGameTeamCopy);
            Game.proto.RemoveCallback<S_BattleSync_16032>(S_BattleSync);
            Game.proto.RemoveCallback<S_BattleLinestate_16033>(S_BattleLinestate);
            //Game.proto.RemoveCallback<SMSG_BattleOver_Ntf>(S_BattleOver);
        }

        /// <summary>
        /// 玩家离线
        /// </summary>
        /// <param name="msg"></param>
        public void S_BattleLinestate(S_BattleLinestate_16033 msg)
        {
            //LegionData legionData = room.sceneData.GetLegionByRoleId((int)msg.char_id);
            //string name = msg.char_id + "";
            //if(legionData != null)
            //{
            //    name = legionData.roleName;
            //    legionData.isOnline = msg.state == 1;

            //    if(!legionData.isOnline && legionData.legionId == room.hostLegionId)
            //    {
            //        CsharpCallLuaFun.ShowToastMsg("丢失主机");
            //        room.sceneData.GameOverPVE(WarOverType.Fail);
            //    }
            //}
            //CsharpCallLuaFun.ShowToastMsg(name + (msg.state == 1 ? "上线" : "离线"));

        }


        ///// <summary>
        ///// 战斗同步加载进度
        ///// </summary>
        //public void C_SyncLoadingProgress(int progress)
        //{
        //    CMSG_LoadingProgress_Req msg = new CMSG_LoadingProgress_Req();
        //    msg.progress = (uint)progress;
        //    Game.proto.SendProtoMsg<CMSG_LoadingProgress_Req>(msg);
        //}

        //public void S_SyncLoadingProgress(SMSG_SyncLoadingProgress_Ntf msg)
        //{
        //}

        ///// <summary>
        ///// 通知玩家开始游戏
        ///// </summary>
        //public void S_StartGameTeamCopy(SMSG_StartGameTeamCopy_Ntf msg)
        //{
        //}

        /// <summary>
        /// 战斗同步
        /// </summary>
        public void C_BattleSync(List<BattleSyncData> list)
        {
            C_BattleSync_16031 msg = new C_BattleSync_16031();
            msg.list.AddRange(list);
            //Game.proto.SendProtoMsg<CMSG_BattleSync_Req>(msg);
            WarPhotonRoom.SendProtoMsg<C_BattleSync_16031>(msg);
        }

        public void C_BattleSync(BattleSyncData action)
        {
            C_BattleSync_16031 msg = new C_BattleSync_16031();
            msg.list.Add(action);
            //Game.proto.SendProtoMsg<CMSG_BattleSync_Req>(msg);
            WarPhotonRoom.SendProtoMsg<C_BattleSync_16031>(msg);
        }

        public void S_BattleSync(S_BattleSync_16032 msg)
        {
            for (int i = 0; i < msg.list.Count; i++)
            {
                OnActionMsg(msg.list[i]);
            }
            //receiveList.AddRange(msg.list);
        }



        ///// <summary>
        ///// 战斗结束
        ///// </summary>
        //public void S_BattleOver(SMSG_BattleOver_Ntf msg)
        //{
        //    WarOverData over = new WarOverData();
        //    over.overType = (WarOverType)msg.data.over_type;
        //    over.starNum = (int)msg.data.star;
        //    over.stageType = room.stageType;
        //    over.stageId = room.stageConfig.stageId;
        //    room.clientNetS.GameOver(over);
        //}


        private List<BattleSyncData> sendList       = new List<BattleSyncData>();
        private List<BattleSyncData> receiveList    = new List<BattleSyncData>();


        public void Send<T>(WarSyncActionType actionType, T action)
        {
            Send(WarProtoUtil.CreateBattleSyncMsg<T>(actionType, action));
        }


        public void SendEmpty(WarSyncActionType actionType)
        {
            BattleSyncData msg = new BattleSyncData();
            msg.type = (uint)actionType;
            Send(msg);
        }


        public void Send(BattleSyncData action)
        {
            action.proxie = (int)WarProxieModel.Server;
            if(WarProtoUtil.IsNeedPasMater(action.type))
            {
                action.proxie = (int)WarProxieModel.ClientAndServer;
            }

            if (!room.isNetModel)
            {
                OnAction(action);
                return;
            }
            //C_BattleSync(action);
            sendList.Add(action);
        }

        private float frameTime = 0;
        public void OnFrame()
        {
            frameTime -= LTime.deltaTime;
            if (frameTime <= 0)
            {
                if (sendList.Count > 0)
                {
                    //frameTime = 0.05f;
                    C_BattleSync(sendList);
                    sendList.Clear();
                }
            }


            for (int i = 0; i < receiveList.Count; i++)
            {
                OnActionMsg(receiveList[i]);
            }
            receiveList.Clear();
        }

        private void OnActionMsg(BattleSyncData msg)
        {
            if (msg.proxie == (uint)WarProxieModel.ClientAndServer)
            {
                OnAction(msg);
            }
            else if (msg.proxie == (uint)WarProxieModel.Client && !room.proxieModel.PServer())
            {
                OnAction(msg);
            }
        }
        public void OnAction(BattleSyncData msg)
        {
            WarSyncActionType actionType = (WarSyncActionType) msg.type;
            switch (actionType)
            {
                // 设置房间状态
                case WarSyncActionType.RoomState:
                    WarSyncRoomState roomState = WarProtoUtil.Deserialize<WarSyncRoomState>(msg);
                    room.clientNetS.SetRoomState(roomState);
                    break;
                // 设置游戏开始
                case WarSyncActionType.Start:
                    WarSyncStart roomStart = WarProtoUtil.Deserialize<WarSyncStart>(msg);
                    room.clientNetS.SetRoomStateStart(roomStart);
                    break;
                // 设置游戏结束
                case WarSyncActionType.Over:
                    WarSyncOver roomOver = WarProtoUtil.Deserialize<WarSyncOver>(msg);

                    WarOverData over = new WarOverData();
                    over.overType = (WarOverType)roomOver.overType;
                    over.starNum = (int)roomOver.star;
                    over.stageType = room.stageType;
                    over.stageId = room.stageConfig.stageId;
                    room.clientNetS.GameOver(over);
                    break;
                // 创建势力组
                case WarSyncActionType.CreateLegionGroup:
                    WarSyncCreateLegionGroup legionGroup = WarProtoUtil.Deserialize<WarSyncCreateLegionGroup>(msg);
                    room.clientNetS.CreateLegionGroup(legionGroup);
                    break;
                // 创建势力
                case WarSyncActionType.CreateLegion:
                    WarSyncCreateLegion legion = WarProtoUtil.Deserialize<WarSyncCreateLegion>(msg);
                    room.clientNetS.CreateLegion(legion);
                    break;
                // 生成势力关系
                case WarSyncActionType.LegionMatrix:
                    WarSyncLegionMatrix legionMatrix = WarProtoUtil.Deserialize<WarSyncLegionMatrix>(msg);
                    room.clientNetS.GenerateLegionMatrix(legionMatrix);
                    break;
                // 生成地图环境信息
                case WarSyncActionType.GenerageMapInfo:
                    room.clientNetS.GenerageMapInfo();
                    break;
                // 创建英雄单位
                case WarSyncActionType.CreateHeroUnit:
                // 创建主基地单位
                case WarSyncActionType.CreateMainbaseUnit:
                    WarSyncCreateUnit unit = WarProtoUtil.Deserialize<WarSyncCreateUnit>(msg);
                    room.clientNetS.CreateUnit(unit);
                    break;
                // 创建机关单位
                case WarSyncActionType.CreateTowerUnit:
                    WarSyncCreateTowerUnit tower = WarProtoUtil.Deserialize<WarSyncCreateTowerUnit>(msg);
                    room.clientNetS.CreateTowerUnit(tower);
                    break;
                // 创建士兵单位
                case WarSyncActionType.CreateSoliderUnit:
                    WarSyncCreateSoliderUnit solider = WarProtoUtil.Deserialize<WarSyncCreateSoliderUnit>(msg);
                    room.clientNetS.CreateSoliderUnit(solider);
                    break;
                // 设置单位是否活着
                case WarSyncActionType.SetUnitIsLive:
                    WarSyncSetUnitIsLive setLive = WarProtoUtil.Deserialize<WarSyncSetUnitIsLive>(msg);
                    room.clientNetS.SetUnitIsLive(setLive.uid, setLive.isLive);
                    break;
                // 销毁单位
                case WarSyncActionType.RemoveUnit:
                    WarSyncRemoveUnit removeUnit = WarProtoUtil.Deserialize<WarSyncRemoveUnit>(msg);
                    room.clientNetS.RemoveUnit(removeUnit.uid);
                    break;

                //// 士兵波次--准备
                //case WarSyncActionType.SoliderWaveReady:
                //    WarSyncSoliderWaveReady waveReady = WarProtoUtil.Deserialize<WarSyncSoliderWaveReady>(msg);
                //    room.clientNetS.SoliderWaveReady(waveReady);
                //    break;
                //// 士兵波次--跳过
                //case WarSyncActionType.SoliderWaveSkip:
                //    WarSyncSoliderWaveSkip waveSkip = WarProtoUtil.Deserialize<WarSyncSoliderWaveSkip>(msg);
                //    room.clientNetS.SoliderWaveSkip(waveSkip);
                //    break;
                //// 士兵波次--开始出兵
                //case WarSyncActionType.SoliderWaveStart:
                //    WarSyncSoliderWaveStart waveStart = WarProtoUtil.Deserialize<WarSyncSoliderWaveStart>(msg);
                //    room.clientNetS.SoliderWaveStart(waveStart);
                //    break;

                // 属性--势力--能量
                //case WarSyncActionType.SetLegionEnergy:
                //    WarSyncLegionEnergy legionEnergy = WarProtoUtil.Deserialize<WarSyncLegionEnergy>(msg);
                //    room.clientNetS.LegionEnergy(legionEnergy);
                //    break;

                // 开始占领操作
                case WarSyncActionType.BeginOccupyUnit:
                    WarSyncBeginOccupyUnit begionOccupyUnit = WarProtoUtil.Deserialize<WarSyncBeginOccupyUnit>(msg);
                    room.clientNetS.BeginOccupyUnit(begionOccupyUnit);
                    break;
                // 结束占领操作
                case WarSyncActionType.EndOccupyUnit:
                    WarSyncEndOccupyUnit endOccupyUnit = WarProtoUtil.Deserialize<WarSyncEndOccupyUnit>(msg);
                    room.clientNetS.EndOccupyUnit(endOccupyUnit);
                    break;
                // 占领单位
                case WarSyncActionType.OccupyUnit:
                    WarSyncOccupyUnit occupyUnit = WarProtoUtil.Deserialize<WarSyncOccupyUnit>(msg);
                    room.clientNetS.OccupyUnit(occupyUnit);
                    break;


                // 开始修理操作
                case WarSyncActionType.BeginRebuildUnit:
                    WarSyncBeginRebuildUnit begionRebuildUnit = WarProtoUtil.Deserialize<WarSyncBeginRebuildUnit>(msg);
                    room.clientNetS.BeginRebuildUnit(begionRebuildUnit);
                    //LLL.LL("----- " + WarSyncActionType.BeginRebuildUnit);
                    break;
                // 结束修理操作
                case WarSyncActionType.EndRebuildUnit:
                    WarSyncEndRebuildUnit endRebuildUnit = WarProtoUtil.Deserialize<WarSyncEndRebuildUnit>(msg);
                    room.clientNetS.EndRebuildUnit(endRebuildUnit);
                    //LLL.LL("----- " + WarSyncActionType.EndRebuildUnit);
                    break;


                // 开始雇佣操作
                case WarSyncActionType.BeginEmployUnit:
                    WarSyncBeginEmployUnit begionEmploydUnit = WarProtoUtil.Deserialize<WarSyncBeginEmployUnit>(msg);
                    room.clientNetS.BeginEmployUnit(begionEmploydUnit);
                    break;
                // 结束雇佣操作
                case WarSyncActionType.EndEmployUnit:
                    WarSyncEndEmployUnit endEmployUnit = WarProtoUtil.Deserialize<WarSyncEndEmployUnit>(msg);
                    room.clientNetS.EndEmployUnit(endEmployUnit);
                    break;

                // 回收单位操作
                case WarSyncActionType.RecoveryUnit:
                    WarSyncRecoveryUnit recoveryUnit = WarProtoUtil.Deserialize<WarSyncRecoveryUnit>(msg);
                    room.clientNetS.RecoveryUnit(recoveryUnit);
                    break;

                // 单位-- 机关 -- 攻击
                case WarSyncActionType.TowerAttack:
                    WarSyncTowerAttack towerAttack = WarProtoUtil.Deserialize<WarSyncTowerAttack>(msg);
                    room.clientNetS.TowerAttack(towerAttack);
                    break;



                //// 单位-- 单位--士兵
                //case WarSyncActionType.UnitSolider:
                //    WarSyncUnit unitSolider = WarProtoUtil.Deserialize<WarSyncUnit>(msg);
                //    room.clientNetS.UnitSolider(unitSolider);
                //    break;

                // 单位-- 单位--英雄
                case WarSyncActionType.UnitHero:
                    WarSyncUnit unitHero = WarProtoUtil.Deserialize<WarSyncUnit>(msg);
                    room.clientNetS.UnitHero(unitHero);
                    break;
                // 单位-- 单位--属性
                case WarSyncActionType.UnitProp:
                    WarSyncUnitProp unitProp = WarProtoUtil.Deserialize<WarSyncUnitProp>(msg);
                    room.clientNetS.UnitProp(unitProp);
                    break;
                // 投降--信息
                case WarSyncActionType.SurrenderInfo:
                    WarSyncSurrenderInfo surrenderInfo = WarProtoUtil.Deserialize<WarSyncSurrenderInfo>(msg);
                    room.clientNetS.SurrenderInfo(surrenderInfo);
                    break;
            }
        }
    }
}
