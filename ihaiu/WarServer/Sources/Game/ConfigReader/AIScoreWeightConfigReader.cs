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
    [ConfigCsv("Config/AIScoreWeight", true, false)]
    public class AIScoreWeightConfigReader : ConfigReader<AIScoreWeightConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AIScoreWeightConfig config = new AIScoreWeightConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.weightHatred = csv.GetSingle(GetHeadIndex("weight_hatred"));
            config.weightType = csv.GetSingle(GetHeadIndex("weight_type"));
            config.weightDistance = csv.GetSingle(GetHeadIndex("weight_distance"));
            configs.Add(config.id, config);
        }
    }
}
