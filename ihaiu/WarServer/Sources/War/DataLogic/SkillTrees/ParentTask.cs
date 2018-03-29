using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 5:38:41 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.SkillTrees
{
    public class ParentTask : SkillNodeTask
    {
        /** 子任务列表 */
        private List<SkillNodeTask> childTasks;

        public List<SkillNodeTask> ChildTasks
        {
            get
            {
                return childTasks;
            }

            set
            {
                childTasks = value;
            }
        }

        /** 获取子任务数量 */
        public int ChildCount
        {
            get
            {
                if (childTasks == null || childTasks.Count == 0)
                    return 0;

                return childTasks.Count;
            }
        }

        /** 添加子任务 */
        public void AddChild(SkillNodeTask task)
        {
            if(childTasks == null)
            {
                childTasks = new List<SkillNodeTask>();
            }

            task.SkillParentTask = this;
            childTasks.Add(task);
        }

        /** 获取子任务 */
        public SkillNodeTask GetChild(int i)
        {
            if (childTasks != null && i < childTasks.Count)
            {
                return childTasks[i];
            }

            return null;
        }




    }
}
