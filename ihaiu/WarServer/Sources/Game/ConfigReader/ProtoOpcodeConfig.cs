using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Games
{
	public class ProtoOpcodeConfig
	{
		public int 		opcode;
		public string 	protoStructName;
		public string 	filename;
		public string 	opcodeMapping;
		public string 	note;

		public List<int> opcodeMappingList = new List<int>();

		public string protoStructAliasNameC
		{
			get 
			{
				return string.Format ("C_{0}_{1}", protoStructName.Replace("CMSG_", "").Replace("_Req", ""), opcode);
			}
		}

		public string protoStructAliasNameS
		{
			get 
			{
				return string.Format ("S_{0}_{1}", protoStructName.Replace("SMSG_", "").Replace("_Resp", "").Replace("_Ntf", ""), opcode);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[ProtoOpcodeConfig]{{opcode={0}, protoStructName={1}, filename={2}, opcodeMapping={3}, note={4}}}", 
				opcode, protoStructName, filename, opcodeMapping, note);
		}
	}
}
