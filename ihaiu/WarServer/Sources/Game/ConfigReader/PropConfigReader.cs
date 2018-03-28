using com.ihaiu;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/5/2017 3:43:51 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ConfigCsv("Config/Property", true, false)]
    public class PropConfigReader : ConfigReader<PropConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            PropConfig config = new PropConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.constName = csv.GetString(GetHeadIndex("const"));
            config.field = csv.GetString(GetHeadIndex("field"));
            config.enName = csv.GetString(GetHeadIndex("enName"));
            config.cnName = csv.GetString(GetHeadIndex("cnName"));
            config.icon = csv.GetString(GetHeadIndex("icon"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.groupType = (PropGroupType) csv.GetInt32(GetHeadIndex("groupType"));

            configs.Add(config.id, config);
        }
    }
}
