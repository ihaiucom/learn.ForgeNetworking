using Games.PB;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/15/2017 10:33:03 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarClientNetS : AbstractRoomObject
    {

        public WarClientNetS(WarRoom room)
        {
            this.room = room;
        }


        /** 设置房间状态 */
        virtual public void SetRoomState(WarSyncRoomState msg)
        {
            room.enterData.SetLegionStateAndProgress(msg.legionId, (WarProcessState)msg.state, msg.loadProgress);

        }


        /** 设置游戏开始 */
        public void SetRoomStateStart(WarSyncStart msg)
        {
            foreach (WarEnterGroupData group in room.enterData.groupList)
            {
                foreach (WarEnterLegionData legion in group.legionList)
                {
                    room.enterData.SetLegionStateAndProgress(legion.legionId, WarProcessState.Gameing, 100);
                }
            }
            room.clientProcessState = WarProcessState.Gameing;
        }

        /** 创建势力组 */
        public void CreateLegionGroup(WarSyncCreateLegionGroup data)
        {
            LegionGroupData group = room.creater.CreateLegionGroup(data.legionGroupId);
            room.sceneData.AddLegionGroup(group);
        }

        /** 创建势力 */
        public void CreateLegion(WarSyncCreateLegion data)
        {
            LegionData legion = room.creater.CreateLegion(data);
            LegionGroupData group = room.sceneData.GetLegionGroup(data.legionGroupId);
            group.AddLegion(legion);
            room.sceneData.AddLegion(legion);
        }

        /** 生成势力关系 */
        public void GenerateLegionMatrix(WarSyncLegionMatrix data)
        {
            room.sceneData.relationMatrixConfig.Generate(data.legionIdList, data.matrix);
        }

        // 生成地图环境信息
        virtual public void GenerageMapInfo()
        {
            // 生成玩家出生区域
            GenerateSpawnPlayerRegion();
            // 生成怪物航线
            GenerateRoute();
            // 生成建筑格子
            GenerateBuildCell();
        }


        /** 生成玩家出生区域及安全区域 */
        public void GenerateSpawnPlayerRegion()
        {
            foreach (RegionData item in room.stageConfig.spawnPlayerRegionList)
            {
                room.sceneData.AddRegion(item.Clone());
            }
            foreach (RegionData item in room.stageConfig.spawnRegionListSafe)
            {
                room.sceneData.AddRegionSafe(item.Clone());
            }
        }


        /** 生成航线 */
        public void GenerateRoute()
        {
            foreach (StageRouteConfig item in room.stageConfig.routeList)
            {
                room.sceneData.AddRoute(item);
            }
        }


        /** 生成建筑格子 */
        public void GenerateBuildCell()
        {
            int count = room.stageConfig.buildCellList.Count;

            for (int i = 0; i < count; i++)
            {

                StageBuildCellConfig item = room.stageConfig.buildCellList[i];
                BuildCellData cell = new BuildCellData(room);
                cell.uid = item.uid;
                cell.routeId = item.routeId;
                cell.position = item.position;
                cell.rotation = item.rotation;
                cell.size = item.size;
                cell.switchBuildCellList.AddRange(item.initUnit.switchBuildCellList);

                room.sceneData.AddBuildCell(cell);
                room.clientViewAgent.AddBuildCell(cell);
            }
        }


        /// <summary>
        /// 创建士兵单位
        /// </summary>
        /// <param name="msg"></param>
        public void CreateSoliderUnit(WarSyncCreateSoliderUnit msg)
        {
            UnitData unit = CreateUnitData(msg.unit);
            unit.routeId = msg.routeId;
            UnitAgent unitAgent = CreateUnitAgent(unit);
            OnCreateUnit(unit, unitAgent);


            #region 怪物刷新，出生点闪烁
            StageRouteConfig display = room.sceneData.GetRouteParentRoot(unit.routeId);
            WarUI.Instance.mUIMiniMap.OnShowFlash(display.path[0], display.routeId);
            #endregion
        }


        /** 创建机关 */
        public void CreateTowerUnitNeedSetSkills(WarSyncCreateTowerUnit msg)
        {
            BuildCellData cell = room.sceneData.GetBuildCell(msg.cellUid);
            if (cell.hasUnit)
            {
                return;
            }

            msg.unit.uid = room.UNIT_UID;

            WarEnterUnitData item = room.enterData.GetLegionUnit(msg.unit.legionId, msg.unit.unitId);
            msg.unit.skills.Clear();
            msg.unit.skills.AddRange(WarProtoUtil.GenerateSyncSkillList(room, item));
            CreateTowerUnit(msg);

            // 创建机关销毁能量
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(msg.unit.unitId, msg.unit.unitLevel);
            LegionData legionData = room.sceneData.GetLegion(msg.unit.legionId);
            int engeryCost = unitLevelConfig.buildCost * -1;
            legionData.Energy += engeryCost;
            LegionEnergy(legionData.legionId, legionData.Energy, engeryCost);

        }

        /// <summary>
        /// 创建机关单位
        /// </summary>
        /// <param name="msg"></param>
        public void CreateTowerUnit(WarSyncCreateTowerUnit msg)
        {
            UnitData unit = CreateUnitData(msg.unit);
            room.sceneData.SetBuildCellUnit(msg.cellUid, unit);
            unit.buildCellUid = msg.cellUid;
            UnitAgent unitAgent = CreateUnitAgent(unit);
            OnCreateUnit(unit, unitAgent);
        }



        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="msg"></param>
        public void CreateUnit(WarSyncCreateUnit msg)
        {
            UnitData unit = CreateUnitData(msg);
            UnitAgent unitAgent = CreateUnitAgent(unit);
            OnCreateUnit(unit, unitAgent);
        }


        /** 创建单位数据 */
        public UnitData CreateUnitData(WarSyncCreateUnit msg)
        {
            UnitData unit = room.creater.CreateUnit(msg);
            unit.room = room;
            unit.unitData = unit;
            room.sceneData.AddUnit(unit);
            switch (unit.unitType)
            {
                case UnitType.Hero:
                    // 势力 主单位
                    unit.legionData.mainUnit = unit;
                    // TODO 临时 给初始能量
                    unit.prop.AddProp(PropId.Energy, PropType.Base, room.stageConfig.energy);
                    unit.prop.AddProp(PropId.EnergyMax, PropType.Base, room.stageConfig.energyMax);
                    break;
                case UnitType.Build:
                    switch (unit.buildType)
                    {
                        case UnitBuildType.Mainbase:
                            // 势力 主基地
                            //unit.legionData.mainbaseUnit = unit;
                            if (unit.legionData.group != null)
                            {
                                foreach (LegionData legion in unit.legionData.group.list)
                                {
                                    legion.mainbaseUnit = unit;
                                }
                            }
                            break;
                    }
                    break;
            }

            unit.prop.InitNonrevertFinals();
            unit.prop.Calculate();
            return unit;
        }


        /** 创建单位视图 */
        public UnitAgent CreateUnitAgent(UnitData unitData)
        {
            UnitAgent unitAgent = room.clientViewAgent.AddUnit(unitData);
            return unitAgent;
        }

        private void OnCreateUnit(UnitData unitData, UnitAgent unitAgent)
        {
            unitData.UsePassiveSkillBirth();
        }

        /** 销毁单位 */
        virtual public void RemoveUnit(int uid, int attackUid = 0,bool cloneUnit = false)
        {
            UnitData unit = room.sceneData.GetUnit(uid);
            if (unit == null)
            {
                // TSLoger.LogFormat("移除单位 uid={0}, unit=null", uid);
                return;
            }

            if (!cloneUnit)
            {
                if (unit.unitType == UnitType.Build)
                {
                    if (unit.buildType == UnitBuildType.Mainbase)
                    {
                        SetUnitIsLive(uid, false);
                        return;
                    }
                }
                else if (unit.unitType == UnitType.Hero)
                {
                    SetUnitIsLive(uid, false);
                    return;
                }
            }
            SetUnitIsLive(uid, false, attackUid);
            
            UnitAgent unitAgent = room.clientSceneView.GetUnit(uid);
            room.clientViewAgent.RemoveUnit(uid);
            room.sceneData.SetBuildCellEmpty(unit.buildCellUid);
            room.sceneData.RemoveUnit(unit);
            
        }


        /** 设置单位是否活着 */
        //attackUid==-1表示需要自动消失，无需死亡动作
        virtual public void SetUnitIsLive(int uid, bool isLive,int attackUid = 0)
        {

            UnitData unit = room.sceneData.GetUnit(uid);
            unit.isLive = isLive;
            unit.prop.Hp = unit.prop.HpMax;
            // TSLoger.LogFormat("设置单位是否活着 uid={0}, isLive={1}, unit={2}", uid, isLive, unit.ToStringBase());

            UnitAgent unitAgent = room.clientSceneView.GetUnit(uid);
            if (isLive)
            {
                unitAgent.ActionRestLive();
            }
            else
            {
                if (unit.unitType == UnitType.Hero && !unit.isCloneUnit)
                {
                    unit.legionData.reliveSecond = 20;
                }
                UIHandler.OnUnitDeathed(unitAgent.unitData.avatarConfig.id);
                unitAgent.ActionDeath(attackUid < 0);
            }
        }




        /** 创建技能 */
        public void CreateSkill(int index, int skillUid, int skillId, int skillLevel, int unitUid, int legionId)
        {

        }

        /** 移动 */
        public void Move(WarSyncMove msg)
        {
            //Loger.LogFormat("ClientNetS Move unit uid={0}  move opindex={1}", msg.uid, msg.opindex);
            UnitData unit = room.sceneData.GetUnit(msg.uid);
            if (unit != null)
            {
                if (unit.isServerObject)
                {
                    if (!room.proxieModel.PServer())
                    {
                        SetMove(msg, unit);
                    }
                }
                else if (!unit.isClientOwnObject)
                {
                    SetMove(msg, unit);
                }
            }
        }

        private void SetMove(WarSyncMove msg, UnitData unit)
        {
            unit.position = msg.position.ProtoToVector3();
            unit.rotation = msg.rotation.ProtoToVector3();
            UnitAgent unitAgent = room.clientSceneView.GetUnit(msg.uid);
            if (unitAgent != null)
            {
                //unitAgent.transform.position    = unit.position;
                unitAgent.transform.eulerAngles = unit.rotation;
                unitAgent.animatorManager.Do_Run();
            }
        }


        /** 释放技能 */
        public void Skill(WarSyncSkill msg)
        {

        }



        //===================================================
        // 战斗波次
        //---------------------------------------------------
        #region 士兵波次

        ///** 士兵波次--准备 */
        //public void SoliderWaveReady(WarSyncSoliderWaveReady msg)
        //{
        //    room.clientSpawnSolider.SetReady(msg.waveIndex, msg.waveCount, msg.waveReadyTime);
        //}

        ///** 士兵波次--跳过准备 */
        //public void SoliderWaveSkip(WarSyncSoliderWaveSkip msg)
        //{
        //    room.clientSpawnSolider.SetSkip(msg.legionIdList);
        //}


        ///** 士兵波次--跳过准备 */
        //public void SoliderWaveStart(WarSyncSoliderWaveStart msg)
        //{
        //    room.clientSpawnSolider.SetSpawn();
        //}

        #endregion



        //===================================================
        // 战斗属性
        //---------------------------------------------------
        #region 属性


        public void LegionEnergyAdd(int legionId, float energyAdd, Vector3 position)
        {
            room.punScene.RPC_LegionEnergyAdd(legionId, energyAdd, position);

            //WarSyncLegionEnergy msg = new WarSyncLegionEnergy();
            //msg.legionId = legionId;
            //msg.energyAdd = (int)energyAdd;
            //msg.needEffect = true;
            //msg.effectPosition = position.Vector3ToProto();
            //LegionEnergy(msg);
        }


        public void LegionEnergy(int legionId, float energy, float energyAdd)
        {
            room.punScene.RPC_LegionEnergy(legionId, energy);
            //WarSyncLegionEnergy msg = new WarSyncLegionEnergy();
            //msg.legionId = legionId;
            //if (energy >= 0) msg.energy = (int)energy;
            //if (energyAdd != 0) msg.energyAdd = (int)energyAdd;
            //LegionEnergy(msg);
        }


        ///** 势力--能量 */
        //public void LegionEnergy(WarSyncLegionEnergy msg)
        //{
        //    LegionData legionData = room.sceneData.GetLegion(msg.legionId);

        //    if (msg.energySpecified)
        //    {
        //        legionData.Energy = msg.energy;
        //    }

        //bool needEffect = false;
        //// 如果需要能量改变特效
        //if(legionData.legionId == room.clientOwnLegionId &&  msg.energyAddSpecified && msg.energyAdd > 0 && msg.needEffect)
        //{
        //    needEffect = true;
        //        // 唯一id           世界坐标           动画结束的回调
        //    WarUI.Instance.OnShowDieEffect(msg.effectPosition.ProtoToVector3(), call, msg);
        //}

        //if(!needEffect && msg.needEffect)
        //{
        //    legionData.Energy += msg.energyAdd;
        //}

        //}


        //void call(WarSyncLegionEnergy msg)
        //{
        //    LegionData legionData = room.sceneData.GetLegion(msg.legionId);
        //    legionData.Energy += msg.energyAdd;
        //}
        #endregion


        //===================================================
        // 机关操作
        //---------------------------------------------------
        #region 机关操作 -- 占领

        /** 开始占领操作 */
        virtual public void BeginOccupyUnit(WarSyncBeginOccupyUnit msg)
        {
            // 可以在这里设置其他玩家状态
        }

        /** 结束占领操作 */
        virtual public void EndOccupyUnit(WarSyncEndOccupyUnit msg)
        {
            // 可以在这里设置其他玩家状态
        }

        /** 占领单位 */
        virtual public void OccupyUnit(WarSyncOccupyUnit msg)
        {
            UnitData unit = room.sceneData.GetUnit(msg.towerUid);
            unit.legionId = msg.legionId;
        }

        #endregion




        #region 机关操作 -- 修理/复活
        /** 机关操作--修理--开始 */
        virtual public void BeginRebuildUnit(WarSyncBeginRebuildUnit msg)
        {
            // 可以在这里设置其他玩家状态
        }

        /** 机关操作--修理--结束 */
        virtual public void EndRebuildUnit(WarSyncEndRebuildUnit msg)
        {
            //// 可以在这里设置其他玩家状态
            //UnitData towerUnit = room.sceneData.GetUnit(msg.towerUid);
            //if (towerUnit != null && towerUnit.isLive && towerUnit.unitAgent != null)
            //{
            //    towerUnit.unitAgent.RestLifeFinal();
            //}
        }
        #endregion



        #region 机关操作 -- 雇佣
        /** 机关操作--雇佣 */
        virtual public void BeginEmployUnit(WarSyncBeginEmployUnit msg)
        {
            UnitData opUnit = room.sceneData.GetUnit(msg.operateHeroUid);

            if (opUnit != null)
            {
                LegionData legionData = opUnit.legionData;

                UnitData unit = room.sceneData.GetUnit(msg.towerUid);
                if (unit != null && legionData != null)
                {
                    unit.SetEmploy(legionData.legionId);
                    UnitAgent unitAgent = room.clientSceneView.GetUnit(msg.towerUid);
                    UnitAgent opUnitAgent = room.clientSceneView.GetUnit(msg.operateHeroUid);
                    if (unitAgent != null && opUnitAgent != null && unitAgent.animatorManager.TowerPlayPos != null)
                    {
                        if (opUnit.clientIsOwn)
                        {
                            opUnitAgent.LookAt(unitAgent.unitData);
                            opUnitAgent.unitControl.OnMovePointer(true, unitAgent);
                        }
                        else
                        {
                            opUnitAgent.Move(unitAgent.position, unitAgent.unitData.prop.MovementSpeed, 2, true, null);
                        }
                    }
                }
            }
        }

        /** 机关操作--雇佣 */
        virtual public void EndEmployUnit(WarSyncEndEmployUnit msg)
        {
            UnitData unit = room.sceneData.GetUnit(msg.towerUid);
            unit.RemoveEmploy();
        }
        #endregion



        #region 机关操作 -- 回收
        public void BeginRecoveryUnit(WarSyncRecoveryUnit msg)
        {

            UnitAgent unit = room.clientSceneView.GetUnit(msg.towerUid);
            UnitAgent operateUnit = room.clientSceneView.GetUnit(msg.operateHeroUid);
            if (unit != null && operateUnit != null)
            {
                operateUnit.unitControl.OnMoveUnitRecovery(unit);
            }

        }
        public void EndRecoveryUnit(int towerUid, int operateHeroUid)
        {
            WarSyncRecoveryUnit msg = new WarSyncRecoveryUnit();
            msg.towerUid = towerUid;
            msg.operateHeroUid = operateHeroUid;
            EndRecoveryUnit(msg);
        }

        public void EndRecoveryUnit(WarSyncRecoveryUnit msg)
        {
            // 回收能量
            UnitData unit = room.sceneData.GetUnit(msg.towerUid);
            if (unit == null)
            {
                return;
            }
            int engery = room.constConfig.GetRecoverEngery(unit);
            unit.legionData.Energy += engery;
            LegionEnergy(unit.LegionId, unit.legionData.Energy, engery);

            // 广播回收机关
            RecoveryUnit(msg);

            // 销毁单位
            RemoveUnit(msg.towerUid);
        }

        /** 机关操作--回收 */
        virtual public void RecoveryUnit(WarSyncRecoveryUnit msg)
        {
            UnitAgent unitAgent = room.clientSceneView.GetUnit(msg.towerUid);
            if (unitAgent != null)
            {
                #region 添加回收特效
                GameObject go = room.clientRes.GetGameObjectInstall("PrefabFx/AttackEffect/CT3001", unitAgent.modelTform);
                go.transform.SetParent(null);
                GameObject.Destroy(go, 5);
                #endregion
            }
        }
        #endregion



        #region 单位-- 机关 -- 攻击
        /** 单位-- 机关 -- 攻击 */
        public void TowerAttack(WarSyncTowerAttack msg)
        {
            UnitAgent tower = room.clientSceneView.GetUnit(msg.towerUid);
            if (tower != null)
            {
                UnitData target = room.sceneData.GetUnit(msg.targetUid);
                if (!room.sceneData.CheckUnitInSafeRegion(target) && !target.isCloneUnit)
                {
                    tower.ActionAttack(target, msg.skillId);
                }
            }
        }
        #endregion



        //#region 单位--士兵
        ///** 单位--士兵 */
        //public void UnitSolider(WarSyncUnit msg)
        //{
        //    UnitAgent solider = room.clientSceneView.GetUnit(msg.unitUid);
        //    if (solider != null && solider.soliderBehaviourSync != null)
        //    {
        //        solider.soliderBehaviourSync.OnMsg(msg);
        //    }
        //}
        //#endregion


        #region 单位--英雄
        /** 单位--士兵 */
        public void UnitHero(WarSyncUnit msg)
        {

            UnitAgent unit = room.clientSceneView.GetUnit(msg.unitUid);
            if (unit != null)
            {
                if (msg.joy != null && msg.joy.opindexSpecified)
                {
                    // 控制技能方向
                    unit.warUnitcontrol.UIHandler_OnJoyPos((WarUIAttackType)msg.joy.opindex, msg.joy.pos.ProtoToVector3(), msg.joy.run, msg.joy.realpos);

                    //if (!unit.unitData.clientIsOwn)
                    //{
                    //    if (Vector3.Distance(unit.position, msg.joy.position.ProtoToVector3()) > 4)
                    //    {
                    //        unit.position = msg.joy.position.ProtoToVector3();
                    //    }
                    //}
                }
                if (msg.joyMove != null && msg.joyMove.opindexSpecified)
                {
                    unit.warUnitcontrol.UIHandler_OnJoyPos((WarUIAttackType)msg.joyMove.opindex, msg.joyMove.pos.ProtoToVector3(), msg.joyMove.run, msg.joyMove.realpos);
                    // 控制角色移动
                    //if (!unit.unitData.clientIsOwn)
                    //{
                    //    if (Vector3.Distance(unit.position, msg.joyMove.position.ProtoToVector3()) > 4)
                    //    {
                    //        unit.position = msg.joyMove.position.ProtoToVector3();
                    //    }
                    //}
                }

                if (msg.joy != null && msg.joy.statetypeSpecified)
                {
                    unit.warUnitcontrol.OnUseSkill((WarUIAttackType)msg.joy.statetype, (ButtonState)msg.joy.stateval, (int)msg.bullteNum);
                }
            }
        }
        #endregion

        #region 单位--属性
        /** 单位--属性 */
        public void UnitProp(WarSyncUnitProp msg)
        {

            UnitAgent unit = room.clientSceneView.GetUnit(msg.unitUid);
            if (unit != null)
            {
                if (!unit.unitData.clientIsOwn)
                {
                    if (msg.hpSpecified)
                    {
                        unit.unitData.prop.Hp = msg.hp;
                    }
                }
            }
        }
        #endregion


        //===================================================
        // 投降
        //---------------------------------------------------
        #region 投降
        /** 投降--信息 */
        virtual public void SurrenderInfo(WarSyncSurrenderInfo msg)
        {
            //room.clientSurrender.OnInfo(msg);
        }
        #endregion


        /** 游戏结束 */
        virtual public void GameOver(WarOverData overData)
        {
            room.Over(overData);
        }
    }
}
