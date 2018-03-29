using System;
using System.Collections.Generic;
using Games.Wars;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 5:47:57 PM
*  @Description:    技能树
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 技能树
    /// 
    /// 技能树参考传统行为树的设计，使用树形结构控制技能的执行流程。
    /// 技能树和行为树在结构上比较类似，但是在运行逻辑上有很大的不同。
    /// 首先，技能树的重点并不是根据上下文选择一个合适的节点执行，而是以一定的策略将技能树从头到尾遍历执行一遍。
    /// 
    /// 其次，技能树没有tick的概念，而是基于回调的，比如一个顺序节点，顺序节点中一个子节点执行完毕后，马上通知顺序节点，
    /// 顺序节点执行下一个子节点，直至顺序节点的最后一个子节点执行完毕，顺序节点就会通知父节点（如果有）它已经执行完毕。
    /// 
    /// 此外，为了完成技能的一些需求，控制节点往往存储更多的控制信息来控制子节点的执行流程。
    /// 具体的信息根据策划需求设置，比如顺序结点包括原子属性和循环属性。
    /// 如果一个顺序节点具有原子属性，则这个顺树节点在执行的过程中并不会被end，只有全部子节点执行结束才可以end。
    /// </summary>
    public class SkillTree : SkillNode
    {
        /** 创建任务 */
        protected override SkillNodeTask CreateTask()
        {
            return new SkillTreeTask();
        }


        public void SettingIndex()
        {

        }

        /** 生成树任务 */
        public SkillTreeTask GenerateTreeTask(SkillController skill)
        {
            SkillTreeTask rootTask = (SkillTreeTask) this.CreateTask();
            rootTask.Init(this, rootTask, skill);

            for (int i = 0; i < this.ChildCount; i++)
            {
                SkillNode childNode = this.GetChild(i);
                GenerateTreeTask(childNode, rootTask, rootTask, skill);
            }

            return rootTask;
        }

        private SkillNodeTask GenerateTreeTask(SkillNode node, ParentTask parentTask, SkillTreeTask rootTask, SkillController skill)
        {
            SkillNodeTask task = node.CreateAndInitTask(rootTask, skill);
            if (parentTask != null)
            {
                parentTask.AddChild(task);
            }



            if (node.ChildCount > 0)
            {
                parentTask = (ParentTask) task;
                Debug.Check(parentTask != null, "改节点的任务需要继承SkillParentTask， {0}", node);
            }


            for(int i = 0; i < node.ChildCount; i ++)
            {
                SkillNode childNode = node.GetChild(i);
                GenerateTreeTask(childNode, parentTask, rootTask, skill);
            }

            return task;
        }


    }
}
