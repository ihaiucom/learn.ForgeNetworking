using FMODUnity;
using Games.SkillTrees;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:43:26 PM
*  @Description:    战斗房间 门面
* ==============================================================================
*/
namespace Games.Wars
{

    public class WarRoom
    {
        /** 单位UID */
        private int unitUid = 1;
        /** 单位UID */
        public int UNIT_UID
        {
            get
            {
                return ++unitUid;
            }
        }


        /** 光环单位UID */
        private int haloUnitUid = 100000;
        public int HALO_UNIT_UID
        {
            get
            {
                return ++haloUnitUid;
            }
        }

        /** 技能UID */
        private int skillUid = 1;
        /** 技能UID */
        public int SKILL_UID
        {
            get
            {
                return ++skillUid;
            }
        }

        // 事件--启动
        public Action                   EventStarted;
        // 事件--启动
        public Action<WarOverType>      EventOver;

        // 战斗房间设置
        public WarRoomSetting           setting;

        /** 代理模式 */
        public WarProxieModel           proxieModel         = WarProxieModel.ClientAndServer;
        /** 运行模式 */
        public WarRunModel              runMode             = WarRunModel.Normal;
        /** xVSy */
        public WarVSMode                vs                  = WarVSMode.One;
        /** 关卡模式 */
        public StageType                stageType           = StageType.Dungeon;
        /** 展示模式 */
        public WarDisplayModel          displayModel        = WarDisplayModel.Normal;
        /** 是否是网络模式 */
        public bool isNetModel
        {
            get
            {
                return (stageType == StageType.PVE || stageType == StageType.PVP) && War.isDebugModel == false;
            }
        }


        /** 战斗时间 */
        public WarTime                  Time                = new WarTime();
        /** 战斗逻辑时间 */
        public WarLTime                 LTime              = new WarLTime();

        /** 关卡配置 */
        public StageConfig              stageConfig;
        /** 战斗进入数据 */
        public WarEnterData             enterData;
        /** 战斗结算数据 */
        public WarOverData              overData;
        /** 战场数据 */
        public WarSceneData             sceneData;
        /** 技能树任务 */
        public SkillTreeManager         skillTreeManager;
        /** 光环 BUFF */
        public HaloBuffManager          haoleBuff;
        /** 战斗对象构建器 */
        public WarObjectCreater         creater;
        /** 战斗卸载控制器 */
        public WarUninstallControler    uninstallControler;
        /** 战斗主线程 */
        public WarMainThreadManager     mainThread;
        /** 战斗常量配置 */
        public WarConstConfig           constConfig;
        /// <summary>
        /// 战斗场景过程动画控制器
        /// </summary>
        public WarRoomStoryManager      storyManager;
        /// <summary>
        /// 战斗场景技能控制器
        /// </summary>
        public WarRoomSkillManager      skillManager;

        /** [临时]主机势力ID */
        public int hostLegionId;

        #region Game Time
        // 游戏中单位生存时间，大于0才做判断
        public float gameUnitLiveTime = 0;
        // 活动副本怪物总数量
        public int monsterAllCount = 0;
        // 游戏时间限制
        public float gameTimeLimit = -1;
        // 游戏当前时间
        public float gameTime = 0;
        // 游戏剩余时间
        public float gameTimeleft
        {
            get
            {
                if (isGameTimeLimit)
                    return gameTimeLimit - gameTime;
                return 0;
            }
        }
        public float gameTimeLeftPer
        {
            get
            {
                if (isGameTimeLimit)
                {
                    return gameTimeleft / gameTimeLimit;
                }
                return 0;
            }
        }
        // 是否做游戏时间限制
        public bool isGameTimeLimit
        {
            get
            {
                return gameTimeLimit > 0;
            }
        }
        #endregion




        #region Client
        /** 流程状态 */
        public WarProcessState              clientProcessState = WarProcessState.None;
        /** 加载状态 */
        public WarLoadState                clientLoadState = WarLoadState.None;
        /** 加载进度 */
        public int                          clientLoadProgress = 0;

        /** 网络 */
        public WarClientNet                 clientNet;
        /** 网络请求 */
        public WarClientNetC                clientNetC;
        /** 网络接收 */
        public WarClientNetS                clientNetS;
        /** 自己势力ID */
        public int                          clientOwnLegionId = -1;
        /** 操作单位接口 */
        public WarOperationUnit             clientOperationUnit;

        /** 创建 */
        public WarPunScene punScene;
        #endregion


        #region Controller
        /** 战斗启动控制器 */
        public WarRoomInstallController     installController;

        /** 战斗产兵控制器 */
        public WarSpawnSoliderController    spawnSolider;

        /** 投降投票 */
        public WarSurrenderController       surrender;

        #endregion



#if NOT_USE_UNITY
        /** 战斗驱动 */
        public WarConsoleDirve              drive;

        /** 资源 */
        public WarConsoleRes                clientRes;

        /** 战斗视图 */
        public WarConsoleViewAgent          clientViewAgent;

        /** 战斗场景视图 */
        public WarConsoleSceneView        clientSceneView;
#else
        /** 战斗驱动 */
        public WarUnityDirve            drive;

        /** 资源 */
        public WarUnityRes              clientRes;

        /** 战斗视图 */
        public WarUnityViewAgent        clientViewAgent;

        /** 战斗场景视图 */
        public WarUnitySceneView        clientSceneView;
#endif

        public bool IsGameing
        {
            get
            {
                //if (proxieModel.PServer())
                //{
                //    return serverProcessState == WarProcessState.Gameing;
                //}

                //if (proxieModel.PClient())
                //{
                return clientProcessState == WarProcessState.Gameing;
                //}

                //return false;
            }
        }


        public WarRoom()
        {
            constConfig = new WarConstConfig(this);


            // Client
            clientNet = new WarClientNet(this);
            clientNetC = new WarClientNetC(this);
            clientNetS = new WarClientNetS(this);
            clientOperationUnit = new WarOperationUnit(this);
            //WarInstantiationEffect.Instance.Init(this);

            // Controller
            installController   = new WarRoomInstallController(this);
            spawnSolider        = new WarSpawnSoliderController(this);
            surrender     = new WarSurrenderController(this);


            mainThread          = new WarMainThreadManager(this);
            creater             = new WarObjectCreater(this);
            skillTreeManager    = new SkillTreeManager();
            haoleBuff     = new HaloBuffManager(this);
            sceneData           = new WarSceneData(this);
            uninstallControler  = new WarUninstallControler(this);
            storyManager        = new WarRoomStoryManager(this);
            skillManager        = new WarRoomSkillManager(this);
            CreateProxy();
        }


        /** 创建代理 */
        public void CreateProxy()
        {
            // 驱动
            drive = WarProxyUtil.CreateDirve(this);
            // 资源
            clientRes = WarProxyUtil.CreateRes(this);
            // 视图代理
            clientViewAgent = WarProxyUtil.CreateViewAgent(this);
            // 场景视图
            clientSceneView = WarProxyUtil.CreateSceneView(this);
        }

        /** 销毁驱动 */
        private void DesotryDrive()
        {
            if (drive == null)
            {
                drive.DestoryDirve();
            }
        }


        /** 启动战斗 */
        public void Enter(WarEnterData enterData)
        {
            this.enterData = enterData;


            // 关卡配置
            this.stageConfig = Game.config.stage.GetConfig(enterData.stageId);

            // 战斗房间设置
            this.setting = WarRoomSetting.Create(stageConfig.stageType, enterData.activityType);
            // pvp天梯模式，需修改配置中的初始化机关信息
            if (stageConfig.stageType == StageType.PVPLadder)
            {
                Dictionary<int, WarEnterUnitData> dicUnit = new Dictionary<int, WarEnterUnitData>();
                Dictionary<int, WarEnterUnitData> otherDicUnit = new Dictionary<int, WarEnterUnitData>();
                for (int i = 0; i < stageConfig.buildCellList.Count; i++)
                {
                    StageBuildCellConfig buildCellConfig = stageConfig.buildCellList[i];
                    if (buildCellConfig.initUnit.legionId == enterData.ownLegionId)
                    {
                        foreach (WarEnterUnitData item in enterData.initUnitList)
                        {
                            if (dicUnit.ContainsKey(item.unitId)) continue;

                            buildCellConfig.initUnit.unitId = item.unitId;
                            buildCellConfig.initUnit.avatarId = item.avatarId;
                            buildCellConfig.initUnit.unitLevel = item.unitLevel;
                            dicUnit.Add(item.unitId, item);
                            break;
                        }
                    }
                    else
                    {
                        foreach (WarEnterUnitData item in enterData.initOhterUnitList)
                        {
                            if (otherDicUnit.ContainsKey(item.unitId)) continue;

                            buildCellConfig.initUnit.unitId = item.unitId;
                            buildCellConfig.initUnit.avatarId = item.avatarId;
                            buildCellConfig.initUnit.unitLevel = item.unitLevel;
                            otherDicUnit.Add(item.unitId, item);
                            break;
                        }
                    }
                }
            }


            this.stageType = stageConfig.stageType;
            this.gameTimeLimit = stageConfig.timeLimit;
            this.gameTime = 0;
            this.gameUnitLiveTime = enterData.monsterLiveTime;
            this.monsterAllCount = enterData.monsterAllCount;
            WarPhotonRoom.RoomName = enterData.PhotonRoomName;
#if UNITY_EDITOR
            //WarPhotonRoom.RoomName = System.Environment.UserName + "-" + WarPhotonRoom.RoomName;
#endif
            Loger.Log("WarPhoton.RoomName=" + WarPhotonRoom.RoomName);

            WarPhotonRoom.Install(this);
            WarPhotonRoom.EventJoinRoom = OnJoinRoom;
            WarPhotonRoom.ConnectToMaster();
        }

        public void OnJoinRoom()
        {
            Loger.Log("$$$WarRoom OnJoinRoom");

            enterData.Init();
            this.hostLegionId = enterData.hostLegionId;
            this.clientOwnLegionId = enterData.ownLegionId;
            if (hostLegionId == clientOwnLegionId)
            {
                this.proxieModel = WarProxieModel.ClientAndServer;
            }
            else
            {
                this.proxieModel = WarProxieModel.Client;
            }


            Loger.LogFormat("WarEnterData hostLegionId={0}, clientOwnLegionId={1}, proxieModel={2}, stageType={3}, isNetModel={4}", enterData.hostLegionId, enterData.ownLegionId, proxieModel, stageType, isNetModel);


            WarPhotonRoom.SetPlayerProperties();

            clientNet.AddListen();
            installController.Start();
        }

        public void OnStart()
        {
            WarAudioMusic.Install.Start();
            storyManager.Init();
            if (EventStarted != null)
            {
                EventStarted();
            }
        }

        //========================================
        // 结束战斗
        //----------------------------------------

        /** 退出战斗 */
        public void Exit()
        {
            Uninstall();
        }

        /** 战斗结束 */
        internal void Over(WarOverData overData)
        {
            if (EventOver != null)
                EventOver(overData.overType);

            clientProcessState = WarProcessState.GameOver;
            clientSceneView.OnGameOver();
            this.overData = overData;
            Loger.Log("战斗结束 " + overData);
            clientOperationUnit.Stop();
            //WarInstantiationEffect.Instance.Stop();
            skillManager.Stop();
            WarUI.Close();

            Game.audio.StopMusic();
            if (overData.overType == WarOverType.Win)
            {
                Game.audio.PlaySoundUISFX(SoundKeys.Music_Battle_Victory);
            }
            else
            {
                Game.audio.PlaySoundUISFX(SoundKeys.Music_Battle_Lost);
            }
            if (overData.stageType != StageType.PVEActivity)
            {
                Time.timeScale = 0.5f;
            }
            Game.mainThread.StartCoroutine(DelayOpenOverPanel());
        }

        IEnumerator DelayOpenOverPanel()
        {
            if(!overData.isManualExit)
            {
                if (clientOperationUnit != null && !clientOperationUnit.IsLive)
                {
                    yield return new WaitForSeconds(1.5f);
                }
                else if (sceneData.GetSoliderNum() <= 1)
                {
                    if (isGameTimeLimit == false || (isGameTimeLimit || gameTimeleft > 3))
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }

            Time.timeScale = 1f;


            DesotryDrive();
            if (!War.isDebugModel)
            {
                //if(WarPhotonRoom.IsMaster)
                {
                    CsharpCallLuaFun.OnWarOver(overData);
                }
            }
            else
            {
                CsharpCallLuaFun.OpenWarOverPanel(overData);
            }
            WarPhotonRoom.LeaveRoom();
        }

        /** 卸载战斗 */
        private void Uninstall()
        {
            Time.timeScale = 1f;
            WarPhotonRoom.LeaveRoom();
            //uninstallControler.Start();
            //clientOperationUnit.Stop();

            clientNet.RemoveListen();
            clientProcessState = WarProcessState.Exit;
            clientSceneView.Exit();
            DesotryDrive();
            clientOperationUnit.Stop(); //清除点击事件
            //WarInstantiationEffect.Instance.Stop();
            storyManager.Stop();
            skillManager.Stop();
            //clientInstantiationEffect.Stop();
            WarUI.CloseAndDestory();
            clientRes.Uninstall();
        }


        //========================================
        // 暂停，继续
        //----------------------------------------

        public bool gameIsPause = false;

        /** 暂停 */
        public void Pause()
        {
            gameIsPause = true;
            Time.timeScale = 0;
        }

        /** 继续 */
        public void Resume()
        {
            gameIsPause = false;
            Time.timeScale = 1;
        }




        //========================================
        // Update 驱动
        //----------------------------------------
        public void Update()
        {
            mainThread.Update();
            //WarInstantiationEffect.Instance.OnSyncedUpdate();
            if (IsGameing)
            {
                clientOperationUnit.Update();
                storyManager.Tick();
                skillManager.Tick();
            }
        }

        public void OnSyncedUpdate()
        {
            if (displayModel != WarDisplayModel.Normal) return;

            gameTime += LTime.deltaTime;

            // 更新场景数据
            sceneData.OnSyncedUpdate();

            // 更新场景视图
            clientSceneView.OnSyncedUpdate();

            // [没用到的技能树]
            //skillTreeManager.Tick();

            // 出兵控制器
            spawnSolider.OnSyncedUpdate();

            // 投降倒计时
            surrender.OnSyncedUpdate();


            if (IsGameing)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    // 检测战斗结束
                    sceneData.TickGameOver();
                }
            }
        }


    }
}
