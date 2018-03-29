using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/8/2017 2:51:30 PM
*  @Description:    技能树--附加节点 之 前置条件节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 前置条件节点
    /// </summary>
    public class Precondition : AttachAction
    {
        public bool opAnd = true;


        /** 条件判断 */
        public virtual bool Execute(SkillNodeTask task)
        {
            return true;
        }

        /** 复合判断 */
        public virtual bool Evaluate(SkillNodeTask task, ref bool result)
        {
            bool val = Execute(task);
            if(opAnd)
            {
                result = val && result;
            }
            else
            {
                result = val || result;
            }
            return result;
        }
    }
}
