using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 战斗 房间剧情， 管理本场战斗所有剧情
    /// </summary>
    public class WarRoomStoryManager : AbstractRoomObject
    {
        public WarRoomStoryManager(WarRoom room)
        {
            this.room = room;
        }
        public void Stop()
        {

        }

        public List<StoryStage>     stageList   = new List<StoryStage>();
        public StoryStage           current           = null;
        public void Init()
        {
            stageList = new List<StoryStage>();
            // 从配置中读取所有本关卡的剧情配置
            DungeonConfig dung = Game.config.dungeon.GetDungeonConfig(room.stageConfig.stageId);
            for (int i = 0; i < dung.storyId.Count; i++)
            {
                StoryStage stageStory = new StoryStage(GetStorySceneConfig(dung.storyId[i]),this);
                stageList.Add(stageStory);
            }
            // 如果是已经触发过的，是否再次运行
            Start();
        }

        /// <summary>
        /// 启动管理器
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < stageList.Count; i++)
            {
                stageList[i].StartTigger();
            }

        }

        public void OnTigger(StoryStage stage)
        {
            StoryStageMove.Init();
            current = stage;
            // 这里要做进入剧情模式处理
            room.displayModel = WarDisplayModel.Story;
            WarUI.Instance.storySkipObj.SetActive(true);
        }

        public  void   OnSkip()
        {
            if (current != null)
            {
                current.Skip();
            }
        }

        public void OnFinish(StoryStage stage)
        {
            if (current == stage)
                current = null;

            // 这里要做退出剧情模式处理
            room.displayModel = WarDisplayModel.Normal;
            WarUI.Instance.storySkipObj.SetActive(false);
        }

        public void Tick()
        {
            if (current != null)
            {
                current.TickAction();
            }
            else
            {
                TickTiggers();
            }
        }

        private void TickTiggers()
        {
            for (int i = 0 ; i < stageList.Count; i++)
            {
                stageList[i].TickTigger();
            }
        }

        /// <summary>
        /// 生成的obj
        /// </summary>
        public List<StorySceneUnit>                    objUnitList                 = new List<StorySceneUnit>();
        /// <summary>
        /// 场景内物件列表
        /// </summary>
        public SceneObjList sceneObjList = null;
        #region 配置文件读取
        public Dictionary<int, StoryStageConfig> configs = new Dictionary<int, StoryStageConfig>();
        public StoryStageConfig GetStorySceneConfig(int id)
        {
            if (configs.ContainsKey(id))
            {
                return configs[id];
            }

            string josn = Game.asset.LoadConfig("Config/Storyjson/story_" + id);
            StoryStageConfig config = HJsonUtility.FromJsonType<StoryStageConfig>(josn);
            configs.Add(id, config);
            return config;
        }
        #endregion
    }
}