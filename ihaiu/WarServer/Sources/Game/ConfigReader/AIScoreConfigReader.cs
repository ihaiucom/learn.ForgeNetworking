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
    [ignoreAttibute]
    public class AIScoreConfigReader : ConfigReader<AIScoreConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AIScoreConfig config = new AIScoreConfig();
            config.id                   = csv.GetInt32(GetHeadIndex("id"));
            config.name                 = csv.GetString(GetHeadIndex("name"));
            config.hero                 = csv.GetSingle(GetHeadIndex("hero"));
            config.buildMainbase        = csv.GetSingle(GetHeadIndex("build_mainbase"));
            config.buildTowerAttack     = csv.GetSingle(GetHeadIndex("build_tower_attack"));
            config.buildTowerDefense    = csv.GetSingle(GetHeadIndex("build_tower_defense"));
            config.buildTowerAuxiliary  = csv.GetSingle(GetHeadIndex("build_tower_auxiliary"));
            config.buildTowerDoor       = csv.GetSingle(GetHeadIndex("build_tower_door"));
            config.soliderGeneral       = csv.GetSingle(GetHeadIndex("solider_general"));
            config.soliderElite         = csv.GetSingle(GetHeadIndex("solider_elite"));
            config.soliderBoss          = csv.GetSingle(GetHeadIndex("solider_boss"));
            configs.Add(config.id, config);
        }
    }
}
