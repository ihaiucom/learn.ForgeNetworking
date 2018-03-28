using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/Dungeon", true, false)]
    public class DungeonConfigReader : ConfigReader<DungeonConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            DungeonConfig config = new DungeonConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.storyId = new List<int>();
            List<int> storyIdList = csv.GetInt32List(GetHeadIndex("cut_scene"), ',');
            foreach (int storyId in storyIdList)
            {
                if (storyId > 0)
                {
                    config.storyId.Add(storyId);
                }
            }
            configs.Add(config.id, config);
        }
        public DungeonConfig GetDungeonConfig(int id)
        {
            DungeonConfig config = GetConfig(id);
            if (config == null)
            {
                Loger.LogErrorFormat("不存在配置 Dungeon Id={0}", id);
            }
            return config;
        }
    }
}
