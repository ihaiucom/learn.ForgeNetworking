using UnityEngine;
using System.Collections;
using com.ihaiu;
using System.IO;
using System;

namespace Games
{
	[ignoreAttibute]
	[ConfigCsv("Config/ProtoRegister", true , false)]
	public class ProtoRegisterReader : ConfigReader<ProtoRegisterConfig>
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
            ProtoRegisterConfig config = new ProtoRegisterConfig();
			config.opcode			    = csv.GetInt32(GetHeadIndex("opcode"));
            config.filename             = csv.GetString(GetHeadIndex("filename"));
            config.protoStructAliasName = csv.GetString(GetHeadIndex("protoStructAliasName"));
            config.protoStructName	    = csv.GetString(GetHeadIndex("protoStructName"));
			config.note				    = csv.GetString(GetHeadIndex("note"));


			if (configs.ContainsKey (config.opcode)) 
			{
				Loger.LogErrorFormat ("ProtoRegisterReader 存在相同的opcode config.opcode={0}", config.opcode);
			}
			configs.Add(config.opcode, config);
		}
	}
}
