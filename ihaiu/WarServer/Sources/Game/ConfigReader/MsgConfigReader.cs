using UnityEngine;
using System.Collections;
using com.ihaiu;

namespace Games
{
    [ignoreAttibute]
    [Sort(-1)]
	[ConfigCsv("Config/Msg", true , false)]
	public class MsgConfigReader : ConfigReader<MsgConfig>
	{
		public override void ParseCsv (string[] csv)
		{
			MsgConfig config = new MsgConfig();
			config.id	= csv.GetInt32(GetHeadIndex("id"));
			config.content	= csv.GetString(GetHeadIndex("content"));
			config.type	= csv.GetInt32(GetHeadIndex("type"));

			configs.Add(config.id, config);
		}
	}
}
