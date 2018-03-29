using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 剧情--行为实现
    /// </summary>
    public abstract class StoryAction
    {
        public WarRoom              room;
        public StoryStage           stage;
        public WarRoomStoryManager  manager;

        // 状态
        protected StoryActionStatus status = StoryActionStatus.None;
        // 结束方式
        protected StoryActionEndType endType = StoryActionEndType.None;
        // [结束方]为'持续时间'的值
        protected float             durationTime = 1;

        public static StoryAction CreateAction(StoryActionConfig config, StoryStage stage)
        {
            StoryAction actioner = null;
            switch (config.actionType)
            {
                case StoryActionType.Enter:
                    // "进入剧情";
                    break;
                case StoryActionType.Exit:
                    // "退出剧情";
                    break;
                case StoryActionType.HeroPlayAnimation:
                case StoryActionType.HeroPropSet:
                case StoryActionType.HeroPropAdd:
                case StoryActionType.HeroPosition:
                case StoryActionType.HeroMove:
                    {
                        // "[英雄]操作";
                        actioner = new StoryActionUnit();
                    }
                    break;
                case StoryActionType.ObjAdd:
                case StoryActionType.ObjShow:
                case StoryActionType.ObjHide:
                case StoryActionType.ObjRemove:
                case StoryActionType.ObjPlayAnimation:
                case StoryActionType.ObjMove:
                    {
                        // "[物件]操作";
                        actioner = new StoryActionObj();
                    }
                    break;
                case StoryActionType.UIMask:
                case StoryActionType.UIShowHideComplete:
                    {
                        // "[UI]操作";
                        actioner = new StoryActionUI();
                    }
                    break;
                case StoryActionType.AddBubble:
                    {
                        // "添加文本气泡";
                        actioner = new StoryActionBubble();
                    }
                    break;
                case StoryActionType.CameraAnimationPosition:
                case StoryActionType.CameraAnimationRotation:
                case StoryActionType.CameraShake:
                case StoryActionType.CameraFollow:
                    {
                        // "[摄像机]操作";
                        actioner = new StoryActionCamera();
                    }
                    break;
                case StoryActionType.SceneObjManager:
                case StoryActionType.SceneObjRemove:
                    {
                        // "[场景物件]操作";
                        actioner = new StoryActionSceneObj();
                    }
                    break;
                case StoryActionType.Pause:
                    {
                        actioner = new StoryActionPause();
                    }
                    break;
            }

            actioner.stage = stage;
            actioner.room = stage.room;
            actioner.manager = stage.manager;
            actioner.SetConfig(config);
            return actioner;
        }
        public StoryActionConfig config
        {
            get;
            set;
        }

        virtual public void SetConfig(StoryActionConfig config)
        {
            this.config = config;
        }

        private void Start()
        {
            status = StoryActionStatus.Enter;
            OnStart();
        }

        virtual protected void OnStart()
        {

        }

        virtual protected void OnSkip()
        {

        }

        virtual protected void OnTick()
        {

        }

        virtual protected void OnEnd()
        {

        }

        /// <summary>
        /// 结束
        /// </summary>
        protected void End()
        {
            OnEnd();
            status = StoryActionStatus.Exit;
            stage.OnActionFinish(this);
        }

        public void Skip()
        {
            OnSkip();
        }

        private int         tickFrame = 0;
        private float       tickTime = 0;

        /// <summary>
        /// 检测
        /// </summary>
        public void Tick()
        {
            switch(status)
            {
                case StoryActionStatus.None:
                    if (stage.tickTime >= config.storyStartTime)
                    {
                        Start();
                    }
                    break;
                case StoryActionStatus.Enter:
                    {
                        OnTick();
                        switch (endType)
                        {
                            case StoryActionEndType.NextFrame:
                                if (tickFrame == 1)
                                {
                                    End();
                                }
                                break;
                            case StoryActionEndType.DurationTime:
                                if (tickTime > durationTime)
                                {
                                    End();
                                }
                                break;
                        }

                        tickFrame++;
                        tickTime += room.Time.deltaTime;
                    }
                    break;
            }
        }


        /// <summary>
        /// 暂停
        /// </summary>
        virtual public void OnPause()
        {

        }
        /// <summary>
        /// 结束暂停
        /// </summary>
        virtual public void OnResume()
        {

        }
    }
}
