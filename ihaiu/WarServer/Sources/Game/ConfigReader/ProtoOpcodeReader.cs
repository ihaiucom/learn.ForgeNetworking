using UnityEngine;
using System.Collections;
using com.ihaiu;
using System.IO;
using System;

namespace Games
{
	[ignoreAttibute]
	[ConfigCsv("Config/ProtoOpcode", true , false)]
	public class ProtoOpcodeReader : ConfigReader<ProtoOpcodeConfig>
	{
		#if UNITY_EDITOR
		public void Load(string path)
		{
			Type t = this.GetType();
			arrtr = t.GetCustomAttributes(typeof(ConfigCsvAttribute), false)[0] as ConfigCsvAttribute;

			string txt = File.ReadAllText (path);
			ParseAsset (path, txt);
		}
		#endif

		public override void ParseCsv (string[] csv)
		{
			ProtoOpcodeConfig config = new ProtoOpcodeConfig();
			config.opcode			= csv.GetInt32(GetHeadIndex("opcode"));
			config.protoStructName	= csv.GetString(GetHeadIndex("protoStructName"));
			config.filename			= csv.GetString(GetHeadIndex("filename"));
			config.opcodeMapping	= csv.GetString(GetHeadIndex("opcodeMapping"));
			config.note				= csv.GetString(GetHeadIndex("note"));

			if (!string.IsNullOrEmpty (config.opcodeMapping)) 
			{
				config.opcodeMappingList = config.opcodeMapping.ToInt32List();
			}

			if (configs.ContainsKey (config.opcode)) 
			{
				Loger.LogErrorFormat ("ProtoOpcodeReader 存在相同的opcode config.opcode={0}", config.opcode);
			}
			configs.Add(config.opcode, config);
		}
	}
}
