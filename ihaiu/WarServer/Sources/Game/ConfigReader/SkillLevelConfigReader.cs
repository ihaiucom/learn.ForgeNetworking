using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/Skilllevel", true, false)]
    public class SkillLevelConfigReader : ConfigReader<SkillLevelConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            SkillLevelConfig config = new SkillLevelConfig();
            config.levelId = csv.GetInt32(GetHeadIndex("level_id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.cd = csv.GetSingle(GetHeadIndex("CD"));
            config.maxCharge = csv.GetInt32(GetHeadIndex("Max_Charge"));
            config.bulletCD = csv.GetSingle(GetHeadIndex("Bullet_CD"));
            config.bulletMax = csv.GetInt32(GetHeadIndex("Max_Bullet"));
            config.energyCost = csv.GetInt32(GetHeadIndex("energy_cost"));
            config.level = csv.GetInt32(GetHeadIndex("level"));
            config.unlockUnitId = csv.GetInt32(GetHeadIndex("unlock_unit_id"));
            config.unlockUnitLevel = csv.GetInt32(GetHeadIndex("unlock_unit_level"));
            config.effect = csv.GetInt32(GetHeadIndex("effect"));
            config.effectValue = csv.GetInt32(GetHeadIndex("effect_value"));
            config.buff = csv.GetInt32(GetHeadIndex("buff"));
            config.buffLife = csv.GetInt32(GetHeadIndex("butt_duration"));
            config.attributePack = csv.GetInt32(GetHeadIndex("AttributePack"));
            config.tip = csv.GetString(GetHeadIndex("tip"));


            if (configs.ContainsKey(config.levelId))
            {
                Loger.LogErrorFormat("有重复的ID配置 {0}, {1}", this, config.levelId);
            }
            else
            {
                configs.Add(config.levelId, config);
            }
        }

        public SkillLevelConfig GetConfigs(int skillId,int skillLv)
        {
            int id = skillId * 100 + skillLv;
            SkillLevelConfig config = GetConfig(id);
            if (config == null)
            {
                Loger.LogErrorFormat("不存在配置 SkillLevelConfig levelId={0}",
                    id);
            }
            return config;
        }
    }
}
