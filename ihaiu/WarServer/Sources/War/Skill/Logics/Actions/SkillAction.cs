using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能 -- 行为实现
    /// </summary>
    public abstract class SkillAction
    {
        public WarRoom              room;
        public WarSkill             warSkill;
        public WarRoomSkillManager  manager;

        // 状态
        public StoryActionStatus status = StoryActionStatus.None;
        // 结束方式
        protected StoryActionEndType endType = StoryActionEndType.None;
        // [结束方式]为'持续时间'的值
        protected float             durationTime = 1;

        public static SkillAction CreateAction(SkillActionConfig config, WarSkill warSkill)
        {
            SkillAction actioner = null;
            switch (config.sKillActionType)
            {
                case SKillActionType.PlayAnimator:
                    // "播放动作";
                    actioner = new SkillActionPlayAnimator();
                    break;
                case SKillActionType.PlayEffect:
                    // 生成特效
                    actioner = new SkillActionPlayEffect();
                    break;
                case SKillActionType.PlayMusic:
                    // 音效
                    actioner = new SkillActionSound();
                    break;
                case SKillActionType.TriggerDamage:
                    // 伤害
                    actioner = new SkillActionDamage();
                    break;
                case SKillActionType.ShotBullet:
                    // 子弹
                    actioner = new SkillActionShotBullet();
                    break;
                case SKillActionType.MoveTowardCurrDir:
                    // 位移
                    actioner = new SkillActionMoveToward();
                    break;
                case SKillActionType.CreateMirror:
                    // 召唤分身
                    actioner = new SkillActionCreateMirror();
                    break;
                case SKillActionType.RemoveBuff:
                    // 移除buff
                    actioner = new SkillActionRemoveBuff();
                    break;
                case SKillActionType.BuffEffective:
                    // 主体携带buff
                    actioner = new SkillActionBuffEffective();
                    break;
                case SKillActionType.AfterShaking:
                    // 后摇
                    actioner = new SkillActionAfterShaking();
                    break;
                case SKillActionType.SkillEnd:
                    // 技能结束
                    actioner = new SkillActionSkillEnd();
                    break;
                case SKillActionType.RayAttack:
                    actioner = new SkillActionRayAttack();
                    break;
            }

            actioner.warSkill = warSkill;
            actioner.room = warSkill.room;
            actioner.manager = warSkill.manager;
            actioner.SetConfig(config);
            return actioner;
        }
        public SkillActionConfig config
        {
            get;
            set;
        }

        virtual public void SetConfig(SkillActionConfig config)
        {
            this.config = config;
        }

        private void Start()
        {
            status = StoryActionStatus.Enter;
            if (warSkill.actionUnitAgent != null)
            {
                OnStart();
            }
            else
            {
                End();
            }
        }

        virtual protected void OnStart()
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
            warSkill.OnActionFinish(this);
        }

        private int         tickFrame = 0;
        private float       tickTime = 0;

        /// <summary>
        /// 检测,update
        /// </summary>
        public void Tick()
        {
            switch (status)
            {
                case StoryActionStatus.None:
                    if (warSkill.tickTime >= config.triggerTime)
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
    }
}
