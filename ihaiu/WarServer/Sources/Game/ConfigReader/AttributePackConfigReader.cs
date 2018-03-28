using com.ihaiu;
using System;
using System.Collections.Generic;
namespace Games
{
    [ConfigCsv("Config/AttributePack", true, false)]
    public class AttributePackConfigReader : ConfigReader<AttributePackConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AttributePackConfig config = new AttributePackConfig();
            config.id = csv.GetInt32(GetHeadIndex("ID"));
            config.hp = csv.GetInt32(GetHeadIndex("hp"));
            config.hpMax = csv.GetInt32(GetHeadIndex("hp_max"));
            config.damage = csv.GetInt32(GetHeadIndex("damage"));
            config.physicalDefence = csv.GetInt32(GetHeadIndex("physical_defence"));
            config.magicDefence = csv.GetInt32(GetHeadIndex("magic_defence"));
            config.physicalAttack = csv.GetInt32(GetHeadIndex("physical_attack"));
            config.magicAttack = csv.GetInt32(GetHeadIndex("magic_attack"));
            config.hpRecover = csv.GetSingle(GetHeadIndex("hp_recover"));
            config.energyRecover = csv.GetSingle(GetHeadIndex("energy_recover"));
            config.radarRadius = csv.GetInt32(GetHeadIndex("radar_radius"));
            config.attackRadius = csv.GetInt32(GetHeadIndex("attack_radius"));
            config.attackSpeed = csv.GetSingle(GetHeadIndex("attack_speed"));
            config.movementSpeed = csv.GetInt32(GetHeadIndex("movement_speed"));

            if (configs.ContainsKey(config.id))
            {
                Loger.LogErrorFormat("有重复的ID配置 {0}, {1}", this, config.id);
            }
            else
            {
                configs.Add(config.id, config);
            }
        }

        public AttributePackConfig GetConfigs(int id)
        {
            AttributePackConfig config = GetConfig(id);
            if (config == null)
            {
                Loger.LogErrorFormat("不存在配置 SkillLevelConfig levelId={0}",
                    id);
            }
            return config;
        }

    }
}
