using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 剧情触发
    /// </summary>
    public partial class StoryStage : AbstractRoomObject
    {
        public void Init(WarRoomStoryManager manager)
        {
            this.manager = manager;
            this.room = manager.room;
        }
        public WarRoomStoryManager manager;
        public StoryTriggerConfig storyConfig;
        public StoryStageConfig storySceneConfig;
        public StoryTrigger tigger;
        public List<StoryAction> actionList = new List<StoryAction>();

        public StoryStageStatus status = StoryStageStatus.None;
        public float            tickTime        = 0;
        public bool             pause           = false;

        public StoryStage(StoryStageConfig config, WarRoomStoryManager manager)
        {
            actionList = new List<StoryAction>();
            Init(manager);
            storySceneConfig = config;
            storyConfig = config.storyTrigger;
            foreach (var child in storySceneConfig.storyActionConfigDic)
            {
                List<StoryActionConfig> list = child.Value;
                for (int i = 1; i < list.Count; i++)
                {
                    StoryAction  item = StoryAction.CreateAction(list[i], this);
                    actionList.Add(item);
                }
            }
            tigger = StoryTrigger.CreateTirgger(storyConfig, this);
        }

        //================================
        // 触发器
        //--------------------------------

        // 启动触发器检查
        public void StartTigger()
        {
            status = StoryStageStatus.Tiggering;
            tigger.Start();
        }

        internal void TickTigger()
        {
            if (status == StoryStageStatus.Tiggering)
            {
                tigger.Tick();
            }
        }

        // 触发剧情
        internal void OnTigger()
        {
            status = StoryStageStatus.Actioning;
            tickTime = 0;
            tigger.End();
            manager.OnTigger(this);
        }



        //================================
        // 行为
        //--------------------------------
        public void TickAction()
        {
            if (pause) return;
            tickTime += Time.deltaTime;

            for (int i = actionList.Count - 1; i >= 0; i--)
            {
                actionList[i].Tick();
            }
        }

        /// <summary>
        /// 跳过
        /// </summary>
        public void Skip()
        {
            Puase(true);
            StoryStageMove.skip();
            List<StoryAction> actionListSort = new List<StoryAction>();
            for (int i = 0; i < actionList.Count; i++)
            {
                actionListSort.Add(actionList[i]);
            }
            sortActionList(actionListSort);
            for (int i = 0; i < actionListSort.Count; i++)
            {
                actionListSort[i].Skip();
            }
        }

        private void sortActionList(List<StoryAction> list)
        {
            list.Sort((StoryAction a, StoryAction b) =>
            {
                if (a.config.storyStartTime > b.config.storyStartTime)
                {
                    return 1;
                }
                else if (a.config.storyStartTime < b.config.storyStartTime)
                {
                    return -1;
                }
                return 0;
            });
        }


        /// <summary>
        /// 暂停
        /// </summary>
        public void Puase(bool pause)
        {
            this.pause = pause;
        }


        // 完成
        public void OnActionFinish(StoryAction action)
        {
            if (actionList.Contains(action))
            {
                actionList.Remove(action);
            }

            if (actionList.Count <= 0)
            {
                manager.OnFinish(this);
            }
        }

    }
}