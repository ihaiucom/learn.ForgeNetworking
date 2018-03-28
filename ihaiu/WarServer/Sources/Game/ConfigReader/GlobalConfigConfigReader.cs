using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/Global", true, false)]
    public class GlobalConfigConfigReader : ConfigReader<GlobalConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            GlobalConfig config = new GlobalConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.valueGlobal = csv.GetInt32(GetHeadIndex("value"));

            if (configs.ContainsKey(config.id))
            {
                Loger.LogErrorFormat("有重复的ID配置 {0}, {1}", this, config.id);
            }
            else
            {
                configs.Add(config.id, config);
            }
        }

        public int GetGlobalConfigs(int id)
        {
            int result = 0;
            if (id > 0)
            {
                GlobalConfig config = GetConfig(id);
                if (config == null)
                {
                    Loger.LogErrorFormat("不存在配置 ValueConfig unitId={0}", id);
                }
                else
                {
                    result = config.valueGlobal;
                }
            }
            return result;
        }
    }
}
