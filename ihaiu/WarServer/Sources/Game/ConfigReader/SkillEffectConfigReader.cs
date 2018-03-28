using com.ihaiu;
using Games.Wars;
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
    public class SkillEffectConfigReader : IConfigReader
    {

        public Dictionary<int, SkillBass> bassconfigs = new Dictionary<int, SkillBass>();
        //public Dictionary<int, SkillInfo> configs = new Dictionary<int, SkillInfo>();
        //public Dictionary<int, BuffInfo> buffConfigs = new Dictionary<int, BuffInfo>();
        public Dictionary<int, HaloBuff> haloBuffConfigs = new Dictionary<int, HaloBuff>();

        public void Load()
        {
        }

        public void OnGameConfigLoaded()
        {
        }

        public void Reload()
        {
        }



        //public SkillInfo GetSkillInfoConfig(int id)
        //{
        //    if (configs.ContainsKey(id))
        //    {
        //        return configs[id];
        //    }

        //    string josn = Game.asset.LoadConfig("Config/Skilljson/skill_" + id);
        //    SkillInfo config = HJsonUtility.FromJson<SkillInfo>(josn);
        //    configs.Add(id, config);
        //    return config;
        //}

        public SkillBass GetSkillBassConfig(int id)
        {
            if (bassconfigs.ContainsKey(id))
            {
                return bassconfigs[id];
            }

            string josn = Game.asset.LoadConfig("Config/NewSkill/skill_" + id);
            SkillBass config = HJsonUtility.FromJson<SkillBass>(josn);
            bassconfigs.Add(id, config);
            return config;
        }


        //public BuffInfo GetBuffInfoConfig(int id)
        //{
        //    if (buffConfigs.ContainsKey(id))
        //    {
        //        return buffConfigs[id];
        //    }

        //    string josn = Game.asset.LoadConfig("Config/Buffjson/buff_" + id);
        //    BuffInfo config = HJsonUtility.FromJson<BuffInfo>(josn);
        //    buffConfigs.Add(id, config);
        //    return config;
        //}

        public HaloBuff GetHaloBuffConfig(int id)
        {
            if (haloBuffConfigs.ContainsKey(id))
            {
                return haloBuffConfigs[id];
            }

            string josn = Game.asset.LoadConfig("Config/NewBuffHalo/buff_" + id);
            HaloBuff config = HJsonUtility.FromJsonType<HaloBuff>(josn);
            haloBuffConfigs.Add(id, config);
            return config;
        }

    }
}
