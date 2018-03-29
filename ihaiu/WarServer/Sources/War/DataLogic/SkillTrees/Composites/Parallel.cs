using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 10:12:41 AM
*  @Description:    技能树--并行节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public class Parallel : SkillNode
    {
        /** 失败策略 */
        private FailurePolicy       failurePolicy             = FailurePolicy.FAIL_ON_ALL;
        /** 子节点结束还是继续策略 */
        private ChildFinishPolicy   childFinishPolicy     = ChildFinishPolicy.CHILDFINISH_ONCE;

        /** 获取,失败策略 */
        public FailurePolicy GetFailurePolicy()
        {
            return failurePolicy;
        }

        public Parallel SetFailurePolicy(FailurePolicy val)
        {
            failurePolicy = val;
            return this;
        }

        /** 获取,子节点结束还是继续策略 */
        public ChildFinishPolicy GetChildFinishPolicy()
        {
            return childFinishPolicy;
        }

        public Parallel SetChildFinishPolicy(ChildFinishPolicy val)
        {
            childFinishPolicy = val;
            return this;
        }


        /** 创建节点任务 */
        protected override SkillNodeTask CreateTask()
        {
            return new ParallelTask();
        }
    }
}
