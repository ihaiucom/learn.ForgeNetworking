using System;
using System.Collections.Generic;
namespace Games
{
    public class SkillValueConfig
    {
        /** 编号 */
        public int      id;
        /** 名称 */
        public string   name;
        /** 等级值 */
        public  Dictionary<int,float> valueDic       = new Dictionary<int, float>();

        public float GetVal(int level)
        {
            return valueDic[level];
        }

    }
}
