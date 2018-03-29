using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public class StoryActionCamera : StoryAction
    {
        private StoryActionCameraConfig actionConfig;
        public override void SetConfig(StoryActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (StoryActionCameraConfig)config;
            if (actionConfig.actionType == StoryActionType.CameraAnimationRotation || actionConfig.actionType == StoryActionType.CameraAnimationPosition)
            {
                endType = StoryActionEndType.Call;
            }
            else
            {
                endType = StoryActionEndType.DurationTime;
                durationTime = actionConfig.duration;
            }
        }

        protected override void OnSkip()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.CameraAnimationPosition:
                    {
                        // [摄像机] 动画位移
                        Game.camera.CameraMg.transform.position = actionConfig.movePath[actionConfig.movePath.Length - 1];
                    }
                    break;
                case StoryActionType.CameraAnimationRotation:
                    {
                        // [摄像机] 动画旋转
                        Game.camera.CameraMg.transform.localEulerAngles = actionConfig.endValue;
                    }
                    break;
                case StoryActionType.CameraFollow:
                    {
                        // [摄像机] 跟随
                        Game.camera.CameraMg.OnFollowSkip();
                    }
                    break;
            }
            End();
        }

        protected override void OnStart()
        {
            switch (actionConfig.actionType)
            {
                case StoryActionType.CameraAnimationPosition:
                    {
                        // [摄像机] 动画位移
                        Game.camera.CameraMg.mCameraStatus = CameraStatus.Story;
                        if (actionConfig.movePath.Length == 1)
                        {
                            Game.camera.CameraMg.transform.position = actionConfig.movePath[0];
                            End();
                        }
                        else
                        {
                            StoryStageMove.Move(Game.camera.CameraMg.transform, actionConfig.movePath, actionConfig.duration, false, End);
                        }
                    }
                    break;
                case StoryActionType.CameraAnimationRotation:
                    {
                        // [摄像机] 动画旋转
                        Game.camera.CameraMg.mCameraStatus = CameraStatus.Story;
                        StoryStageMove.Rotate(Game.camera.CameraMg.transform, actionConfig.endValue, actionConfig.duration, End);
                    }
                    break;
                case StoryActionType.CameraShake:
                    {
                        // [摄像机] 震屏
                        Game.camera.CameraMg.OnShakeCamera(actionConfig.duration, actionConfig.shakeAmplitude, actionConfig.shakeRange, Game.camera.CameraMg.transform.position);
                    }
                    break;
                case StoryActionType.CameraFollow:
                    {
                        // [摄像机] 跟随
                        Transform tf = null;
                        if (actionConfig.followName != null && actionConfig.followName.Length > 0)
                        {
                            StorySceneUnit storySceneUnit = manager.objUnitList.Find(m => m.objName.Equals(actionConfig.followName));
                            if (storySceneUnit != null)
                            {
                                tf = storySceneUnit.tForm;
                            }
                        }
                        Game.camera.CameraMg.OnFollowUnit(tf);
                    }
                    break;
            }
        }
    }
}