#if !NOT_USE_UNITY
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 10:43:17 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗资源 */
    public class WarUnityRes : WarRes
    {
        public static void UnloadScene()
        {
            Scene scene = SceneManager.GetSceneByName(stageScene);
            if (scene != null)
            {
                SceneManager.UnloadScene(scene);
            }
        }

        public WarUnityRes(WarRoom room)
        {
            this.room = room;
        }

        public void Uninstall()
        {
            //Game.mainThread.StartCoroutine(UninstallScene());
        }

        IEnumerator UninstallScene()
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(stageScene);
            yield return op;

        }

        /** 获取资源 */
        public T GetAsset<T>(string path) where T : UnityEngine.Object
        {
            return Game.asset.TryGetAsset<T>(path);
        }

        /** 获取预设实例 */
        public GameObject GetGameObjectInstall(string path, Transform parent = null, bool forWorld = false)
        {
            GameObject prefab = Game.asset.TryGetAsset<GameObject>(path);
            if (prefab == null)
            {
                Loger.LogErrorFormat("没有预设 path={0}", path);
                return null;
            }

            GameObject result = GameObject.Instantiate(prefab);
            if (parent != null)
            {
                result.name = result.name.Replace("(Clone)", string.Empty);
                Transform child = result.transform;
                child.SetParent(parent);
                child.localEulerAngles = Vector3.zero;
                child.localPosition = Vector3.zero;
                child.localScale = Vector3.one;
            }
            if (forWorld)
            {
                result.transform.SetParent(null);
            }
            return result;
        }

       
        /// <summary>
        /// 同步位置和角度
        /// </summary>
        /// <param name="o"></param>
        /// <param name="tf"></param>
        /// <param name="p"></param>
        public void SetParentInit(GameObject o,Transform tf, Transform p)
        {
            if (o != null)
            {
                tf = o.transform;
            }
            tf.SetParent(p);
            tf.localEulerAngles = Vector3.zero;
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            tf.SetParent(null);
        }

        /** 将对象放回对象池 */
        public void Despawn(GameObject go)
        {
            // 临时销毁
            GameObject.Destroy(go);
        }

        /** 关卡场景名称 */
        private static string          stageScene = "";
        /** 资源列表 */
        private List<string>    filenameList = new List<string>();

        /** 生成预加载资源列表 */
        public List<string> GenerateList()
        {
            stageScene = room.stageConfig.sceneName;
            List<string> list = new List<string>();
            list.Add(WAR_UI);
            //list.Add(WAR_PREFAB_UNIT_PLAYER);
            list.Add(WAR_PREFAB_UNIT_HERO);
            list.Add(WAR_PREFAB_UNIT_SOLIDER);
            list.Add(WAR_PREFAB_UNIT_BUILD);
            list.Add(WAR_PREFAB_BUILDCELL);

            List<int> unitIdList = new List<int>();
            
            // 基地
            foreach (WarEnterMainbaseData mainbase in room.enterData.mainbaseList)
            {
                CheckUnitIdList(unitIdList, mainbase.unit.unitId);
            }

            // 势力
            foreach (WarEnterGroupData group in room.enterData.groupList)
            {
                foreach (WarEnterLegionData legion in group.legionList)
                {
                    // 英雄
                    CheckUnitIdList(unitIdList, legion.hero.unitId);
                    // 塔
                    foreach (WarEnterUnitData tower in legion.towerList)
                    {
                        CheckUnitIdList(unitIdList, tower.unitId);
                    }
                }
            }

            // 怪物
            foreach (StageWaveConfig wave in room.stageConfig.waveList)
            {
                foreach (StageWaveUnitConfig unit in wave.unitList)
                {
                    CheckUnitIdList(unitIdList, unit.unit.unitId);
                }
            }
            // 特效路径
            List<string> effectList = new List<string>();
            // 单位资源
            foreach (int unitId in unitIdList)
            {
                UnitConfig unitConfig = Game.config.unit.GetConfig(unitId);
                if (unitConfig == null) continue;
                AvatarConfig avatarConfig = Game.config.avatar.GetConfig(unitConfig.avatarId);
                if (avatarConfig == null) continue;
                // 模型
                CheckFilenameList(list, avatarConfig.model);
                // 图标
                CheckFilenameList(list, avatarConfig.icon);
                // 特效
                List<string> str = unitConfig.GetSkillEffectPath(unitConfig.skillList);
                if (str != null)
                {
                    effectList.AddRange(str);
                }
            }

            // 修理建造单独处理
            effectList.Add("PrefabFx/AttackEffect/Effect_Build");
            effectList.Add("PrefabFx/AttackEffect/Effect_Fix");

            // 去重
            List<string> newEffectList = new List<string>();
            foreach (string str in effectList)
            {
                if (!newEffectList.Contains(str))
                {
                    newEffectList.Add(str);
                }
            }
            if (newEffectList != null)
            {
                list.AddRange(newEffectList);
            }
            filenameList = list;
            return list;
        }

        private void CheckFilenameList(List<string> list, string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;
            if (list.IndexOf(filename) != -1)
                return;
            list.Add(filename);
        }

        private void CheckUnitIdList(List<int> unitIdList, int unitId)
        {
            if (unitId <= 0)
                return;

            UnitConfig unitConfig = Game.config.unit.GetConfig(unitId);
            if (unitConfig == null)
            {
                Loger.LogFormat("没有单位配置 UnitConfig unitId = {0}", unitId);
                return;
            }

            AvatarConfig avatarConfig = Game.config.avatar.GetConfig(unitConfig.avatarId);
            if (avatarConfig == null)
            {
                Loger.LogFormat("没有Avatar配置 AvatarConfig unitId = {0}, avatarId={0}", unitId, unitConfig.avatarId);
                return;
            }


            if (unitIdList.IndexOf(unitId) != -1)
                return;

            unitIdList.Add(unitId);
        }



        enum LoadStep
        {

            /** 等待打开加载面板 */
            WaitOpenLoader,
            /** 关卡场景 */
            Scene,
            /** 资源 */
            Asset,
            /** 初始化对象池 */
            Pool,
            /** 加载完成 */
            End,
        }


        public int loaderId = 3;

        /** 场景异步加载 */
        private AsyncOperation  sceneOP;
        /** 加载步骤 */
        private LoadStep        loadStep;

        /** 资源数量 */
        private int             assetCount = 0;
        /** 已经加载的资源数量 */
        private int             assetIndex = 0;

        private Action completeEvent;

        /** 关闭加载面板 */
        public void CloseLoadPanel()
        {
            CsharpCallLuaFun.CloseLoader(loaderId, OnCloseLoaderFinish);
        }

        /** 加载 */
        public void Load(Action complete)
        {
            this.completeEvent = complete;
            room.mainThread.updateEvent += Tick;
            loadStep = LoadStep.WaitOpenLoader;
            Game.camera.modelLightEnable= false;
            CsharpCallLuaFun.OpenLoader(loaderId, OnOpenLoaderFinish);
            CsharpCallLuaFun.SetLoaderProgress(loaderId, 0);

            assetCount = filenameList.Count;
            assetIndex = 0;

        }

        void OnOpenLoaderFinish()
        {
            CsharpCallLuaFun.CloseAllMenu();
            LoadScene();
        }

        void OnCloseLoaderFinish()
        {
        }

        /** 加载场景 */
        private void LoadScene()
        {
            Loger.LogFormat("stageScene={0}, loadStep={1}", stageScene, loadStep);
            if(loadStep == LoadStep.WaitOpenLoader)
            {
                loadStep = LoadStep.Scene;
                sceneOP = SceneManager.LoadSceneAsync(stageScene, LoadSceneMode.Single);
                sceneOP.allowSceneActivation = false;
            }
        }

        private float waitOpenLoaderTimeOut = 4;
        private float waitOpenLoaderTime = 0;

        private void Tick()
        {
            if (loadStep == LoadStep.WaitOpenLoader)
            {
                waitOpenLoaderTime += Time.deltaTime;
                if(waitOpenLoaderTime >= waitOpenLoaderTimeOut)
                {
                    OnOpenLoaderFinish();
                }
                return;
            }

            float sceneRate = 30;
            float assetRate = 25;
            float poolRate = 20;

            sceneRate = assetCount > 0 ? sceneRate : sceneRate + assetRate;

            if (loadStep == LoadStep.Scene)
            {
                if (sceneOP.progress < 0.9f)
                {
                    room.clientLoadProgress = Mathf.FloorToInt(sceneOP.progress * sceneRate);
                }
                else if (!sceneOP.allowSceneActivation)
                {
                    sceneOP.allowSceneActivation = true;
                }

                if (sceneOP.isDone)
                {
                    loadStep = assetCount > 0 ? LoadStep.Asset : LoadStep.Pool;
                }
            }
            else if (loadStep == LoadStep.Asset)
            {
                Game.asset.LoadAsset(filenameList[assetIndex]);
                assetIndex++;
                room.clientLoadProgress = Mathf.FloorToInt(assetRate + assetIndex / assetCount);
                if (assetIndex >= assetCount)
                {
                    loadStep = LoadStep.Pool;
                }
            }
            else if (loadStep == LoadStep.Pool)
            {
                loadStep = LoadStep.End;
            }
            else
            {
                if (room.clientLoadProgress < 100)
                {
                    ++room.clientLoadProgress;
                }
                else
                {
                    room.mainThread.updateEvent -= Tick;
                    if (completeEvent != null)
                        completeEvent();
                }
            }


            CsharpCallLuaFun.SetLoaderProgress(loaderId, room.clientLoadProgress);
        }


    }
}
#endif