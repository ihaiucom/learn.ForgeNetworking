using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Games
{
    public class ProtoRegisterConfig
    {
        public int opcode;
        public string protoStructName;
        public string protoStructAliasName;
        public string note = "";
        public string filename;
        public List<int> opcodeMappingList = new List<int>();
        public bool isS;

        public override string ToString()
        {
            return string.Format("{0}       {1}     {2}     {3}     {4}", filename, opcode,  protoStructAliasName, protoStructName, note);
        }

        public string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4}", opcode, filename, protoStructAliasName, protoStructName, note.Replace(",", ""));
        }


        public static string CsvHeadType()
        {
            return "int,string, string, string, string";
        }

        public static string CsvHeadCN()
        {
            return "协议编号,协议文件名称,协议结构别名,协议结构名称,注释";
        }

        public static string CsvHeadEN()
        {
            return "opcode,filename,protoStructAliasName,protoStructName,note";
        }



        public static int CompareTo(ProtoRegisterConfig a, ProtoRegisterConfig b)
        {
            if (a.filename == b.filename)
            {
                return a.opcode - b.opcode;
            }

            return a.filename.CompareTo(b.filename);
        }
    }
}
