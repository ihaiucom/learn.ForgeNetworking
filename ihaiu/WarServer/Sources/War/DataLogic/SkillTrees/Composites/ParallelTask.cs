using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 5:48:15 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 并行节点
    /// 子节点会都会被Tick
    /// 所有子节点都返回SNStatues.SUCCESS时结束
    /// 如果有一个子节点返回SNStatues.FAILURE时结束，那么所有的自己点都停止
    /// 下一次Tick只有是SNStatues.RUNNING的会被运行
    /// </summary>
    public class ParallelTask : ParentTask
    {
        /** 执行状态 */
        private SNStatues[] executionStatus;

        /** 失败策略 */
        private FailurePolicy failurePolicy
        {
            get
            {
                return ((Parallel)node).GetFailurePolicy();
            }
        }

        /** 子节点结束还是继续策略 */
        private ChildFinishPolicy childFinishPolicy
        {
            get
            {
                return ((Parallel)node).GetChildFinishPolicy();
            }
        }



        /** 执行 */
        public override SNStatues OnExecute()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                // 下一次Tick只有是SNStatues.RUNNING的会被运行
                if (childFinishPolicy == ChildFinishPolicy.CHILDFINISH_ONCE)
                {
                    if (executionStatus[i] == SNStatues.INVALID || executionStatus[i] == SNStatues.RUNNING)
                    {
                        SkillNodeTask childTask = GetChild(i);
                        executionStatus[i] = childTask.Tick();
                    }
                }
                else
                {
                    SkillNodeTask childTask = GetChild(i);
                    executionStatus[i] = childTask.Tick();
                }
            }


            status = SNStatues.SUCCESS;
            int failureCount = 0;
            for (int i = 0; i < ChildCount; i++)
            {
                if(executionStatus[i] == SNStatues.RUNNING)
                {
                    status = SNStatues.RUNNING;
                }
                else if (executionStatus[i] == SNStatues.FAILURE)
                {
                    failureCount++;
                    if (failurePolicy == FailurePolicy.FAIL_ON_ONE)
                    {
                        status = SNStatues.FAILURE;
                        break;
                    }
                }
            }

            if(status != SNStatues.FAILURE)
            {
                if(failurePolicy == FailurePolicy.FAIL_ON_ALL)
                {
                    if (failureCount == ChildCount)
                    {
                        status = SNStatues.FAILURE;
                    }
                }
            }


            if (status != SNStatues.RUNNING)
            {
                Rest();
            }

            return status;
        }

        public override SNStatues OnEnter()
        {
            executionStatus = new SNStatues[ChildCount];
            return base.OnEnter();
        }

        public override void OnExit()
        {
            Rest();
            base.OnExit();
        }

        private void Rest()
        {
            for (int i = 0; i < executionStatus.Length; i++)
            {
                executionStatus[i] = SNStatues.INVALID;
            }
        }
    }
}
