using Assets.Scripts.Common;
using Games.PB;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 1:22:44 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗产兵控制器 */
    public partial class WarSpawnSoliderController : AbstractRoomObject
    {
        public WarSpawnSoliderController(WarRoom room)
        {
            this.room = room;
        }

        public PhotonView photonView;

        /** 状态 */
        public WarSpawnWaveState state = WarSpawnWaveState.None;
        /** 总波数 */
        public int waveCount = 0;
        /** 当前第几波 */
        public int waveIndex = -1;
        /** 等待时间倒计时 */
        public float waitTime = 0;
        /** 准备时间倒计时 */
        public float readyTime = 0;
        /** 当前波配置 */
        public StageWaveConfig waveConfig;
        /** 单位生产器 */
        private List<SpawnUnitControler> spawnUnitList = new List<SpawnUnitControler>();
        /** 生产完成的线路数量 */
        private int spawnUnitFinialNum = 0;







        private void SetReady()
        {
            state = WarSpawnWaveState.ReadyTime;
            if (OnReadyEvent != null)
            {
                OnReadyEvent();
            }
            WarAudioMusic.Install.WaveReady(waveIndex);
        }

        private void SetSpawn()
        {
            Loger.LogFormat("SkipWave SoliderWaveStart()");
            readyTime = 0;
            state = WarSpawnWaveState.Spawn;

            if (OnSpawnEvent != null)
            {
                OnSpawnEvent();
            }
            WarAudioMusic.Install.WaveStart(waveIndex);
        }




        public void Start()
        {
            waveCount = room.stageConfig.waveList.Count;
            SetWave(0);
            SetReady();
        }

        private void SetWave(int waveIndex)
        {

            room.clientOperationUnit.ResetBallotSpawnWaveSkip();
            skipLegionDict.Clear();
            skipLegionList.Clear();

            for (int i = 0; i < spawnUnitList.Count; i++)
            {
                spawnUnitList[i].Release();
            }
            spawnUnitList.Clear();

            this.waveIndex = waveIndex;
            waveConfig = room.stageConfig.GetWave(waveIndex);
            if (waveConfig != null)
            {
                readyTime = waveConfig.readyTime;
                waitTime = waveConfig.waitTime;

                foreach (StageWaveUnitConfig waveUnitConfig in waveConfig.unitList)
                {
                    SpawnUnitControler controller = ClassObjPool<SpawnUnitControler>.Get();
                    controller.Set(room, waveUnitConfig);
                    spawnUnitList.Add(controller);
                }
            }
            else
            {
                // 不存在怪物波次
                readyTime = 5;
            }
        }



        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {

                stream.SendNext(waveIndex);
                stream.SendNext(state);
                stream.SendNext(readyTime);
                stream.SendNext(spawnUnitFinialNum);
                stream.SendNext(skipNum);
                stream.SendNext(skipTotal);
            }
            else
            {
                int waveIndex = (int)stream.ReceiveNext();
                WarSpawnWaveState state = (WarSpawnWaveState)stream.ReceiveNext();
                readyTime = (float)stream.ReceiveNext();
                spawnUnitFinialNum = (int)stream.ReceiveNext();
                skipNum = (int)stream.ReceiveNext();
                skipTotal = (int)stream.ReceiveNext();

                if (this.waveIndex != waveIndex)
                {
                    waveCount = room.stageConfig.waveList.Count;
                    SetWave(waveIndex);
                }

                if (this.state != state)
                {
                    this.state = state;
                    switch (state)
                    {
                        case WarSpawnWaveState.ReadyTime:
                            SetReady();
                            break;
                        case WarSpawnWaveState.Spawn:
                            SetSpawn();
                            break;
                    }
                }
            }
        }

        public void OnSyncedUpdate()
        {
            if(!photonView.isMine)
            {
                return;
            }

            if(state == WarSpawnWaveState.None)
            {
                Start();
            }

            switch (state)
            {
                // 准备倒计时
                case WarSpawnWaveState.ReadyTime:
                    readyTime -= LTime.deltaTime;
                    if (readyTime <= 0)
                    {
                        SetSpawn();
                    }
                    break;
                // 产兵种
                case WarSpawnWaveState.Spawn:

                    spawnUnitFinialNum = 0;
                    for (int i = 0; i < spawnUnitList.Count; i++)
                    {
                        if (!spawnUnitList[i].IsFinal)
                        {
                            spawnUnitList[i].Tick();
                        }
                        else
                        {
                            spawnUnitFinialNum++;
                        }
                    }

                    // 检测是否所有航线都产完
                    if (spawnUnitFinialNum >= spawnUnitList.Count)
                    {
                        WarUI.Instance.mUIMiniMap.clearFlashDic();
                        // 不是最后一波，就设置进入下一波
                        if (waveIndex < waveCount - 1)
                        {
                            waveIndex++;
                            SetWave(waveIndex);
                            state = WarSpawnWaveState.Waiting;
                        }
                        // 整个战斗生成结束
                        else
                        {
                            state = WarSpawnWaveState.Final;
                        }
                    }
                    break;

                // 等待
                case WarSpawnWaveState.Waiting:
                    // 需要检测清场
                    if (waveConfig.needCheckClearance)
                    {
                        if (room.sceneData.GetSoliderNum() == 0)
                        {
                            SetReady();
                        }
                    }
                    // 等待时间
                    else
                    {
                        waitTime -= Time.deltaTime;
                        if (waitTime <= 0)
                        {
                            SetReady();
                        }
                    }

                    break;
            }
        }

        #region SpawnUnitControler
        public class SpawnUnitControler : PooledClassObject
        {
            /** 房间 */
            public WarRoom room;
            /** 单位配置 */
            public StageWaveUnitConfig waveUnitConfig;
            /** 已经生产的数量 */
            public int num;
            /** 生产延时时间倒计时 */
            public float delayTime;
            /** 生产间隔时间倒计时 */
            public float intervalTime;
            /** 出生位置 */
            public Vector3 position
            {
                get
                {
                    return room.sceneData.GetRouteBeginPoint(waveUnitConfig.routeId);
                }
            }

            /** 出生朝向 */
            public Vector3 rotation
            {
                get
                {
                    return room.sceneData.GetRouteBeginDirection(waveUnitConfig.routeId);
                }
            }

            /** 是否生产完成 */
            public bool IsFinal
            {
                get
                {
                    return num >= waveUnitConfig.num;
                }
            }

            public void Set(WarRoom room, StageWaveUnitConfig waveUnitConfig)
            {
                this.room = room;
                this.waveUnitConfig = waveUnitConfig;
                this.delayTime = waveUnitConfig.delay;
                this.intervalTime = 0;
                this.num = 0;
            }


            #region Pool
            public override void OnRelease()
            {
                room = null;
                waveUnitConfig = null;
                num = 0;
                delayTime = 0;
                intervalTime = 0;

            }
            #endregion

            public void Tick()
            {
                if (IsFinal) return;
                delayTime -= room.LTime.deltaTime;
                if (delayTime > 0)
                {
                    return;
                }

                intervalTime -= room.LTime.deltaTime;
                if (intervalTime <= 0)
                {
                    intervalTime = waveUnitConfig.interval;
                    num++;

                    WarPhotonRoom.CreateUnitSolider(
                        waveUnitConfig.legionId,
                        waveUnitConfig.routeId,
                        waveUnitConfig.unit.unitId,
                        waveUnitConfig.unit.unitLevel
                        );

                    //List<WarSyncCreateSkill> skills = WarProtoUtil.GenerateSyncSkillList(room, waveUnitConfig.unit.unitId, 1);

                    //WarSyncCreateSoliderUnit msg = new WarSyncCreateSoliderUnit();
                    //msg.routeId = waveUnitConfig.routeId;
                    //msg.unit = WarProtoUtil.GenerateUnit(
                    //    room.UNIT_UID, 
                    //    waveUnitConfig.unit.unitId, 
                    //    waveUnitConfig.unit.unitLevel, 
                    //    waveUnitConfig.legionId, 
                    //    position, 
                    //    rotation, 
                    //    skills);

                    //TSLoger.LogFormat("产兵 routeId={0}, unitId={1}, unitLevel={2}, legionId={3}, uid={4}",
                    //    waveUnitConfig.routeId,
                    //    waveUnitConfig.unit.unitId,
                    //    waveUnitConfig.unit.unitLevel,
                    //    waveUnitConfig.legionId,
                    //    msg.unit.uid
                    //    );
                    //room.clientNetS.CreateSoliderUnit(msg);
                }
            }
        }

        #endregion


    }


}
