using com.ihaiu;
using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 7:59:43 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ConfigCsv("Config/Unit", true, false)]
    public class UnitConfigReader : ConfigReader<UnitConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            UnitConfig config               =   new UnitConfig();
            config.unitId                   =   csv.GetInt32(GetHeadIndex("unit_id"));
            config.name                     =   csv.GetString(GetHeadIndex("name"));
            config.unitType                 =   (UnitType)csv.GetInt32(GetHeadIndex("unit_type"));
            config.buildType                =   (UnitBuildType)csv.GetInt32(GetHeadIndex("build_type"));
            config.soliderType              =   (UnitSoliderType)csv.GetInt32(GetHeadIndex("solider_type"));
            config.professionType           =   (UnitProfessionType)csv.GetInt32(GetHeadIndex("professional"));
            config.spaceType                =   (UnitSpaceType)csv.GetInt32(GetHeadIndex("space_type"));
            config.flyHeight                =   csv.GetSingle(GetHeadIndex("fly_height"));
            config.radius                   =   csv.GetSingle(GetHeadIndex("radius"));
            config.rvoRadius                =   csv.GetSingle(GetHeadIndex("RVO_radius"));
            config.enableRotation           =   csv.GetInt32(GetHeadIndex("enable_rotation")) == 1;
            config.isManualAttack             =   csv.GetInt32(GetHeadIndex("manual_attack")) == 1;
            config.avatarId                 =   csv.GetInt32(GetHeadIndex("avatar_id"));
            config.skillList                =   new Dictionary<int, int>();
            int skill0                      =   csv.GetInt32(GetHeadIndex("skill_" + 0));
            config.skillList.Add(0, skill0);
            List<int> ids = csv.GetInt32List(GetHeadIndex("skill_list"), ';');
            for(int i = 1; i <= ids.Count; i ++)
            {
                config.skillList.Add(i, ids[i - 1]);
            }
            config.weaponDefaultId          = csv.GetInt32(GetHeadIndex("weapon_start"));

            config.deathEffect              = new Dictionary<int, string>();
            for (int i = 1; i < 3; i++)
            {
                config.deathEffect.Add(i, csv.GetString(GetHeadIndex("death_" + i)));
            }

            configs.Add(config.unitId, config);
        }
    }
}
