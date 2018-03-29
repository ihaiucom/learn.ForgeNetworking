using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 9:28:49 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public class LogAction : SkillAction
    {
        public string msg;

        public LogAction SetMsg(string msg)
        {
            this.msg = msg;
            return this;
        }

        protected override SkillNodeTask CreateTask()
        {
            return new LogTask();
        }

    }


    public class LogTask : SkillNodeTask
    {
        public string msg
        {
            get
            {
                return ((LogAction)node).msg;
            }
        }
        public override SNStatues OnExecute()
        {
            Loger.Log(msg);
            return SNStatues.SUCCESS;
        }
    }



}
