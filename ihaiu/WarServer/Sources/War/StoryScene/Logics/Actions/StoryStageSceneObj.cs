using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public class StoryActionSceneObj : StoryAction
    {
        private StoryActionSceneObjConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionSceneObjConfig)config;

            endType = StoryActionEndType.Call;
            if (manager.sceneObjList == null)
            {
                GameObject go = GameObject.Find("Map");
                if (go != null)
                {
                    manager.sceneObjList = go.GetComponent<SceneObjList>();
                }
            }
            aniList = new List<AnimatorManager>();
            currentObj = new List<GameObject>();
            if (actionConfig.unitId > 0)
            {
                List<UnitData> unitList = room.sceneData.GetUnitWithUnitID(actionConfig.unitId);
                if (unitList.Count > 0)
                {
                    for (int i = 0; i < unitList.Count; i++)
                    {
                        aniList.Add(unitList[i].unitAgent.animatorManager);
                        Transform Shadow = unitList[i].unitAgent.transform.Find("Shadow");
                        if (Shadow != null)
                        {
                            currentObj.Add(Shadow.gameObject);
                        }
                        SkinnedMeshRenderer[] smr = unitList[i].unitAgent.animatorManager.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                        if (smr != null && smr.Length > 0)
                        {
                            for (int kk = 0; kk < smr.Length; kk++)
                            {
                                currentObj.Add(smr[kk].gameObject);
                            }
                        }
                    }
                }
            }
            if (currentObj.Count <= 0 && manager.sceneObjList != null && actionConfig.objIndex >= 0 && manager.sceneObjList.sceneObjList.Length > actionConfig.objIndex)
            {
                currentObj.Add(manager.sceneObjList.sceneObjList[actionConfig.objIndex]);
            }
            //if (currentObj == null && manager.sceneObjList != null && actionConfig.objIndex >= 0 && manager.sceneObjList.sceneObjList.Length > actionConfig.objIndex)
            //{
            //    currentObj = manager.sceneObjList.sceneObjList[actionConfig.objIndex];
            //}
        }

        protected override void OnSkip()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.SceneObjManager:
                    {
                        // 物件做动作
                        if (currentObj.Count > 0)
                        {
                            for (int i = 0; i < currentObj.Count; i++)
                            {
                                if (currentObj[i] != null)
                                {
                                    AnimatorManager aniM = currentObj[i].GetComponent<AnimatorManager>();
                                    if (aniM != null)
                                    {
                                        aniM.ActionAni(actionConfig.objAnimation);
                                    }
                                }
                            }
                        }
                        if (aniList.Count > 0)
                        {
                            for (int i = 0; i < aniList.Count; i++)
                            {
                                if (aniList[i] != null)
                                {
                                    aniList[i].ActionAni(actionConfig.objAnimation);
                                }
                            }
                        }
                    }
                    break;
                case StoryActionType.SceneObjRemove:
                    {
                        // 物件显示或隐藏
                        if (currentObj.Count > 0)
                        {
                            for (int i = 0; i < currentObj.Count; i++)
                            {
                                if (currentObj[i] != null)
                                {
                                    currentObj[i].SetActive(actionConfig.objShowOrHide);
                                }
                            }
                        }
                    }
                    break;
            }
            End();
        }
        /// <summary>
        /// 场景内的物体
        /// </summary>
        private List<GameObject> currentObj = new List<GameObject>();
        private List<AnimatorManager> aniList   = new List<AnimatorManager>();
        // 更新场景内物件
        protected override void OnStart()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.SceneObjManager:
                    {
                        // 物件做动作
                        if (currentObj.Count > 0)
                        {
                            for (int i = 0; i < currentObj.Count; i++)
                            {
                                if (currentObj[i] != null)
                                {
                                    AnimatorManager aniM = currentObj[i].GetComponent<AnimatorManager>();
                                    if (aniM != null)
                                    {
                                        aniM.ActionAni(actionConfig.objAnimation);
                                    }
                                }
                            }
                        }
                        if (aniList.Count > 0)
                        {
                            for (int i = 0; i < aniList.Count; i++)
                            {
                                if (aniList[i] != null)
                                {
                                    aniList[i].ActionAni(actionConfig.objAnimation);
                                }
                            }
                        }
                    }
                    break;
                case StoryActionType.SceneObjRemove:
                    {
                        // 物件显示或隐藏
                        if (currentObj.Count > 0)
                        {
                            for (int i = 0; i < currentObj.Count; i++)
                            {
                                if (currentObj[i] != null)
                                {
                                    currentObj[i].SetActive(actionConfig.objShowOrHide);
                                }
                            }
                        }
                    }
                    break;
            }
            End();
        }
    }
}