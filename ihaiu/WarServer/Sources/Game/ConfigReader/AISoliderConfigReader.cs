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
    [ConfigCsv("Config/AISolider", true, false)]
    public class AISoliderConfigReader : ConfigReader<AISoliderConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AISoliderConfig config = new AISoliderConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.precoditionType = (AIPreconditionType)csv.GetInt32(GetHeadIndex("precondition"));
            if (config.precoditionType == AIPreconditionType.HP)
            {
                float[] arr = csv.GetFloatArray(GetHeadIndex("args"));
                if(arr.Length == 2)
                {
                    config.minHP = arr[0];
                    config.maxHP = arr[1];
                }
            }
            config.enableUseCount = csv.GetInt32(GetHeadIndex("number"));
            config.skillId = csv.GetInt32(GetHeadIndex("skill_id"));
            config.weight = csv.GetSingle(GetHeadIndex("skill_weight"));
            configs.Add(config.id, config);
        }
    }
}
