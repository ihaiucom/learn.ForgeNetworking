using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 4:31:40 PM
*  @Description:    技能树根节点任务
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /** 技能树根节点任务 */
    public class SkillTreeTask : SequenceTask
    {
        public int INDEX_ADD = 0;

        /** 技能树管理器 */
        public SkillTreeManager                         manager;
        /** 节点 */
        public List<SkillNodeTask>                      taskList = new List<SkillNodeTask>();
        public Dictionary<int, SkillNodeTask>           taskDict = new Dictionary<int, SkillNodeTask>();
        protected Dictionary<SkillNodeTask, SNStatues>  taskStatus = new Dictionary<SkillNodeTask, SNStatues>();

        /** 添加任务 */
        private void AddTask(SkillNodeTask task)
        {
            taskList.Add(task);
            taskDict.Add(task.Index, task);
        }
        

        /** 生成任务列表 */
        public void GenerateTaskList()
        {
            GenerateTaskList(this);
        }

        private void GenerateTaskList(ParentTask parent)
        {
            if (parent == null) return;
            AddTask(parent);

            if (parent == null || parent.ChildTasks.Count == 0) return;

            foreach(SkillNodeTask task in parent.ChildTasks)
            {
                if (task == null) continue;
                if(task is ParallelTask)
                {
                    GenerateTaskList((ParentTask) task);
                }
                else
                {
                    AddTask(task);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if(manager != null)
            {
                manager.RemoveTask(this);
            }
        }



    }
}
