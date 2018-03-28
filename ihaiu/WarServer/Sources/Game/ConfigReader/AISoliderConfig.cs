using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/23/2017 4:49:29 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// 士兵技能AI
    /// </summary>
    public class AISoliderConfig
    {
        public int id;
        public string name;
        public string tip;
        public AIPreconditionType precoditionType;
        public float    minHP = 0;
        public float    maxHP = 1;
        public int      enableUseCount = 0;
        public int      skillId;
        public float    weight = 100;
    }
}
