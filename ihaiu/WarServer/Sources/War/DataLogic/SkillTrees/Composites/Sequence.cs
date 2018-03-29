using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/8/2017 1:37:50 PM
*  @Description:    技能树--序列节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public class Sequence : SkillNode
    {
        /** 失败策略 */
        private FailurePolicy failurePolicy = FailurePolicy.FAIL_ON_ONE;


        /** 获取,失败策略 */
        public FailurePolicy GetFailurePolicy()
        {
            return failurePolicy;
        }

        public Sequence SetFailurePolicy(FailurePolicy val)
        {
            failurePolicy = val;
            return this;
        }

        /** 创建任务 */
        protected override SkillNodeTask CreateTask()
        {
            return new SequenceTask();
        }
    }
}
