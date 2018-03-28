using UnityEngine;
using System.Collections;
using com.ihaiu;

namespace Games
{
	[ignoreAttibute]
	[ConfigCsv("Config/Loader", true, false)]
    public class LoadConfigReader : ConfigReader<LoadConfig>
	{
		public override void ParseCsv (string[] csv)
		{
            LoadConfig config = new LoadConfig();
			config.id				= csv.GetInt32(GetHeadIndex("id"));
            config.name				= csv.GetString(GetHeadIndex("name"));
			config.path 			= csv.GetString(GetHeadIndex("path"));
			config.isShowCircle 	= csv.GetBoolean(GetHeadIndex("isShowCircle"));

			configs.Add(config.id, config);
		}
	}
}
