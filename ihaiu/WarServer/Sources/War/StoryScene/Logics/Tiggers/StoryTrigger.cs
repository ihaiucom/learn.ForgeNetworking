using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 剧情--触发器基类触发
    /// </summary>
    [Serializable]
    public abstract class StoryTrigger
    {
        public WarRoom          room;
        public  StoryStage      stage;
        public static StoryTrigger CreateTirgger(StoryTriggerConfig config, StoryStage stage)
        {
            StoryTrigger trigger = null;
            switch (config.tirggerType)
            {
                case TirggerType.WaveStart:
                    trigger = new StoryTriggerWave();
                    break;
                case TirggerType.WaveTipEnd:
                    trigger = new StoryTriggerWaveTipEnd();
                    break;
                case TirggerType.UnitDeath:
                    trigger = new StoryTriggerUnitDeath();
                    break;
                case TirggerType.StageWin:
                    trigger = new StoryTriggerStageOver(WarOverType.Win);
                    break;
                case TirggerType.StageFail:
                    trigger = new StoryTriggerStageOver(WarOverType.Fail);
                    break;
                case TirggerType.StageStart:
                    trigger = new StoryTriggerStageStart();
                    break;
            }

            trigger.stage = stage;
            trigger.room = stage.room;
            trigger.SetConfig(config);
            return trigger;
        }

        public StoryTriggerConfig config
        {
            set;
            get;
        }

        virtual public void SetConfig(StoryTriggerConfig config)
        {
            this.config = config;
        }

        virtual public void Start()
        {

        }


        /// <summary>
        /// 检测
        /// </summary>
        virtual public void Tick()
        {

        }

        /// <summary>
        /// 结束
        /// </summary>
        virtual public void End()
        {

        }

        /// <summary>
        /// 触发
        /// </summary>
        virtual public void OnTirgger()
        {
            stage.OnTigger();
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
