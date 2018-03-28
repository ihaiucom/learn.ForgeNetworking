using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/SkillValue", true, false)]
    public class SkillValueConfigReader : ConfigReader<SkillValueConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            SkillValueConfig config = new SkillValueConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.valueDic = new Dictionary<int, float>();
            for (int i = 1; i < 20; i++)
            {
                config.valueDic.Add(i, csv.GetSingle(GetHeadIndex("lv" + i)));
            }
            if (configs.ContainsKey(config.id))
            {
                Loger.LogErrorFormat("有重复的ID配置 {0}, {1}", this, config.id);
            }
            else
            {
                configs.Add(config.id, config);
            }
        }

        public float GetConfigs(int unitId,int lvIndex)
        {
            float result = 0;
            if (unitId > 10000)
            {
                if (lvIndex < 1)
                {
                    lvIndex = 1;
                }
                SkillValueConfig config = GetConfig(unitId);
                if (config == null)
                {
                    Loger.LogErrorFormat("不存在配置 ValueConfig unitId={0}",
                        unitId);
                }
                else
                {
                    if (lvIndex < 20)
                    {
                        result = config.valueDic[lvIndex];
                    }
                    else
                    {
                        result = config.valueDic[20];
                    }
                }
            }
            return result;
        }
    }
}
