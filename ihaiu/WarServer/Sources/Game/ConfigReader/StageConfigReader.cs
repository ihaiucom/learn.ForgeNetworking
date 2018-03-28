using com.ihaiu;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 3:44:12 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ignoreAttibute]
    public class StageConfigReader : IConfigReader
    {

        public Dictionary<int, StageConfig> configs = new Dictionary<int, StageConfig>();

        public void Load()
        {
        }

        public void OnGameConfigLoaded()
        {
        }

        public void Reload()
        {
        }



        public StageConfig GetConfig(int id)
        {
            if (configs.ContainsKey(id))
            {
                return configs[id];
            }

            string josn = Game.asset.LoadConfig("Config/Stages/Stage_" + id);
            StageConfig config = HJsonUtility.FromJson<StageConfig>(josn);
            configs.Add(id, config);
            return config;
        }

    }
}
