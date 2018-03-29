using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 5:48:33 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public class SequenceTask : ParentTask
    {
        /** 当前子节点索引 */
        private int             currentChildIndex = 0;
        /** 执行状态 */
        private SNStatues       executionStatus = SNStatues.INVALID;

        /** 获取当前执行的子任务索引 */
        public int CurrentChildIndex()
        {
            return currentChildIndex;
        }

        /** 是否还能执行 */
        public bool CanExecute()
        {
            return currentChildIndex < ChildTasks.Count;
        }


        /** 子任务执行完成 */
        public void OnChildEnded(int childIndex)
        {
            currentChildIndex ++;
        }


        /** 失败策略 */
        private FailurePolicy failurePolicy
        {
            get
            {
                return ((Sequence)node).GetFailurePolicy();
            }
        }

        /** 执行 */
        public override SNStatues OnExecute()
        {
            status = SNStatues.RUNNING;
            for(int i = 0; i < ChildCount; i ++)
            {
                if (!CanExecute())
                {
                    status = SNStatues.SUCCESS;
                    break;
                }

                SkillNodeTask childTask = GetChild(CurrentChildIndex());
                if (childTask == null)
                {
                    status = SNStatues.SUCCESS;
                    break;
                }


                executionStatus = childTask.Tick();
                if(executionStatus == SNStatues.RUNNING)
                {
                    status = SNStatues.RUNNING;
                    break;
                }
                else
                {
                    
                    if (executionStatus == SNStatues.FAILURE)
                    {
                        if(failurePolicy == FailurePolicy.FAIL_ON_ONE)
                        {
                            status = SNStatues.FAILURE;
                            break;
                        }
                    }


                    if (executionStatus != SNStatues.RUNNING)
                    {
                        OnChildEnded(CurrentChildIndex());
                    }


                    if (CurrentChildIndex() >= ChildCount)
                    {
                        status = SNStatues.SUCCESS;
                    }
                }

            }
            

            if(status != SNStatues.RUNNING)
            {
                currentChildIndex   = 0;
                executionStatus     = SNStatues.INVALID;
            }

            return status;
        }

        public override void OnExit()
        {
            currentChildIndex   = 0;
            executionStatus     = SNStatues.INVALID;
            base.OnExit();
        }
    }
}
