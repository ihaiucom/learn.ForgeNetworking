using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为(功能)
    /// </summary>
    [Serializable]
    public class StoryActionConfig
    {
        /// <summary>
        /// 行为类型
        /// </summary>
        public StoryActionType              actionType;
        /// <summary>
        /// id
        /// </summary>
        public int                          actionId;
        /// <summary>
        /// 父id
        /// </summary>
        public int                          proActionId;
        /// <summary>
        /// 子id自增长
        /// </summary>
        public int                          childAutoId;
        /// <summary>
        /// 编辑器中的位置
        /// </summary>
        public Rect                         rectPos                 = new Rect(50,250,220,200);
        /// <summary>
        /// 开始时间
        /// </summary>
        public float                        storyStartTime          = 0;

        public StoryActionConfig()
        {

        }
        public StoryActionConfig(int actionId)
        {
            this.actionId = actionId;
            childAutoId = actionId * 100;
        }
        public static StoryActionConfig CreateTirggerConfig(StoryActionType actionType, int actionId)
        {
            StoryActionConfig config = null;
            switch (actionType)
            {
                case StoryActionType.UIMask:
                case StoryActionType.UIShowHideComplete:
                    config = new StoryActionMaskConfig();
                    break;
                case StoryActionType.AddBubble:
                    config = new StoryActionBubbleConfig();
                    break;
                case StoryActionType.ObjAdd:
                case StoryActionType.ObjShow:
                case StoryActionType.ObjHide:
                case StoryActionType.ObjRemove:
                case StoryActionType.ObjPlayAnimation:
                case StoryActionType.ObjMove:
                    config = new StoryActionObjConfig();
                    break;
                case StoryActionType.CameraAnimationPosition:
                case StoryActionType.CameraAnimationRotation:
                case StoryActionType.CameraShake:
                case StoryActionType.CameraFollow:
                    config = new StoryActionCameraConfig();
                    break;
                //case StoryActionType.UnitAdd:
                //case StoryActionType.UnitRemove:
                case StoryActionType.HeroPlayAnimation:
                case StoryActionType.HeroPropSet:
                case StoryActionType.HeroPropAdd:
                case StoryActionType.HeroPosition:
                case StoryActionType.HeroMove:
                    config = new StoryActionUnitConfig();
                    break;
                case StoryActionType.SceneObjManager:
                case StoryActionType.SceneObjRemove:
                    config = new StoryActionSceneObjConfig();
                    break;
                default:
                    config = new StoryActionConfig();
                    break;
            }
            config.actionType = actionType;
            config.actionId = actionId;
            config.childAutoId = actionId * 100;
            return config;
        }
    }
}