using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 4:34:00 PM
*  @Description:    技能树管理器 
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /** 技能树管理器 */
    public class SkillTreeManager
    {
        /** 技能树任务列表 */
        public List<SkillTreeTask> taskList = new List<SkillTreeTask>();

        

        /** 启动技能树任务 */
        public void AddTask(SkillTreeTask treeTask)
        {
            treeTask.manager = this;
            taskList.Add(treeTask);
        }

        /** 技能树任务结束 */
        public void RemoveTask(SkillTreeTask treeTask)
        {
            if (taskList.Contains(treeTask) )
            {
                taskList.Remove(treeTask);
                treeTask.manager = null;
            }
        }


        private List<SkillTreeTask> runTaskList = new List<SkillTreeTask>();
        /** Tick技能树列表 */
        public void Tick()
        {
            int count = taskList.Count;
            for(int i = 0; i < count; i ++)
            {
                runTaskList.Add(taskList[i]);
            }

            for (int i = 0; i < count; i++)
            {
                runTaskList[i].Tick();
            }
            runTaskList.Clear();
        }

    }
}
