using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public class StoryActionObj : StoryAction
    {
        private StoryActionObjConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionObjConfig)config;

            endType = StoryActionEndType.Call;

            objParent = room.clientOperationUnit.GetUnitAgent().transform.parent;
        }

        protected override void OnSkip()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.ObjAdd:
                    {
                        // 添加物件
                        StorySceneUnit storySceneUnit = new StorySceneUnit();
                        storySceneUnit.gObject = room.clientRes.GetGameObjectInstall(actionConfig.objPath);
                        storySceneUnit.tForm = storySceneUnit.gObject.transform;
                        storySceneUnit.tForm.SetParent(objParent);
                        storySceneUnit.tForm.position = actionConfig.objPos;
                        storySceneUnit.tForm.localEulerAngles = actionConfig.objAngle;
                        storySceneUnit.tForm.localScale = actionConfig.objScale;
                        storySceneUnit.gObject.SetActive(true);
                        storySceneUnit.animatorManager = storySceneUnit.gObject.GetComponent<AnimatorManager>();
                        storySceneUnit.objName = actionConfig.objName;
                        manager.objUnitList.Add(storySceneUnit);
                    }
                    break;
                case StoryActionType.ObjShow:
                    {
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.gObject.SetActive(true);
                        }
                    }
                    break;
                case StoryActionType.ObjHide:
                    {
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.gObject.SetActive(false);
                        }
                    }
                    break;
                case StoryActionType.ObjPlayAnimation:
                    break;
                case StoryActionType.ObjMove:
                    {
                        // 物件移动
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            int length = actionConfig.objMovePath.Length;
                            storySceneUnit.tForm.position = actionConfig.objMovePath[length - 1];
                            if (length > 1)
                            {
                                Vector3 nearestDir = actionConfig.objMovePath[length - 1] - actionConfig.objMovePath[length - 2];
                                storySceneUnit.tForm.rotation = Quaternion.LookRotation(nearestDir);
                            }
                        }
                    }
                    break;
                case StoryActionType.ObjRemove:
                    {
                        // 物件移除
                        int index = manager.objUnitList.FindIndex(m => m.objName.Equals(actionConfig.currentObjName));
                        if (index != -1)
                        {
                            manager.objUnitList[index].gObject.SetActive(false);
                        }
                    }
                    break;
            }
            End();
        }

        private Transform objParent;

        // 更新物件
        protected override void OnStart()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.ObjAdd:
                    {
                        // 添加物件
                        StorySceneUnit storySceneUnit = new StorySceneUnit();
                        storySceneUnit.gObject = room.clientRes.GetGameObjectInstall(actionConfig.objPath);
                        storySceneUnit.tForm = storySceneUnit.gObject.transform;
                        storySceneUnit.tForm.SetParent(objParent);
                        storySceneUnit.tForm.position = actionConfig.objPos;
                        storySceneUnit.tForm.localEulerAngles = actionConfig.objAngle;
                        storySceneUnit.tForm.localScale = actionConfig.objScale;
                        storySceneUnit.gObject.SetActive(true);
                        storySceneUnit.animatorManager = storySceneUnit.gObject.GetComponent<AnimatorManager>();
                        storySceneUnit.objName = actionConfig.objName;
                        manager.objUnitList.Add(storySceneUnit);
                        End();
                    }
                    break;
                case StoryActionType.ObjShow:
                    {
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.gObject.SetActive(true);
                        }
                        End();
                    }
                    break;
                case StoryActionType.ObjHide:
                    {
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.gObject.SetActive(false);
                        }
                        End();
                    }
                    break;
                case StoryActionType.ObjPlayAnimation:
                    {
                        // 物件做动作
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.animatorManager.ActionAni(actionConfig.objAnimation);
                        }
                        End();
                    }
                    break;
                case StoryActionType.ObjMove:
                    {
                        // 物件移动
                        StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.currentObjName));
                        if (storySceneUnit != null)
                        {
                            storySceneUnit.animatorManager.ActionAni(AnimatorState.Run);
                            StoryStageMove.Move(storySceneUnit.tForm, actionConfig.objMovePath, actionConfig.objMoveDuration, true, End);
                        }
                    }
                    break;
                case StoryActionType.ObjRemove:
                    {
                        // 物件移除
                        int index = manager.objUnitList.FindIndex(m => m.objName.Equals(actionConfig.currentObjName));
                        if (index != -1)
                        {
                            manager.objUnitList[index].gObject.SetActive(false);
                        }
                        End();
                    }
                    break;
            }
        }
    }
}