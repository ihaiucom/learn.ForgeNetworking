using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 5:52:29 PM
*  @Description:    技能树--节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 技能树--节点
    /// </summary>
    public abstract class SkillNode
    {
        /** 节点ID */
        private int                         id;
        /** 节点名称 */
        private string                      name;
        /** 父亲 */
        private SkillNode                        parentNode;
        /** 子节点 */
        private List<SkillNode>                  childNodes;
        /** 前提条件节点 */
        private List<Precondition>     preconditionNodes;

       

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }



        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }


        public SkillNode ParentNode
        {
            get
            {
                return parentNode;
            }

            set
            {
                parentNode = value;
            }
        }


        public List<SkillNode> ChildNodes
        {
            get
            {
                return childNodes;
            }

            set
            {
                childNodes = value;
            }
        }

        public List<Precondition> PreconditionNodes
        {
            get
            {
                return preconditionNodes;
            }

            set
            {
                preconditionNodes = value;
            }
        }


        /** 添加子节点 */
        virtual public SkillNode AddChild(SkillNode child)
        {
            child.parentNode = this;
            if (childNodes == null)
            {
                childNodes = new List<SkillNode>();
            }

            childNodes.Add(child);
            return this;
        }

        /** 附加前提条件 */
        virtual public SkillNode AttachPrecondition(Precondition precodition)
        {
            precodition.parentNode = this;
            if (preconditionNodes == null)
            {
                preconditionNodes = new List<Precondition>();
            }

            preconditionNodes.Add(precodition);
            return this;
        }

        /** 获取子节点数量 */
        public int ChildCount
        {
            get
            {
                if (childNodes == null)
                    return 0;

                return childNodes.Count;
            }
        }

        /** 获取前提条件数量 */
        public int PrecoditionCount
        {
            get
            {
                if (preconditionNodes == null)
                    return 0;

                return preconditionNodes.Count;
            }
        }

        /** 获取前提节点 */
        public Precondition GetPrecodition(int index)
        {
            if (preconditionNodes != null && index < preconditionNodes.Count)
            {
                return preconditionNodes[index];
            }

            return null;
        }



        /** 获取子节点 */
        public SkillNode GetChild(int index)
        {
            if (childNodes != null && index < childNodes.Count)
            {
                return childNodes[index];
            }

            return null;
        }

        /** 获取子节点, 根据节点ID */
        public SkillNode GetChildById(int nodeId)
        {
            if (childNodes != null && childNodes.Count > 0)
            {
                for (int i = 0; i < childNodes.Count; ++i)
                {
                    SkillNode c = childNodes[i];

                    if (c.Id == nodeId)
                    {
                        return c;
                    }
                }
            }

            return null;
        }


        /** 清空 */
        public void Clear()
        {
            if(childNodes != null)
            {
                childNodes.Clear();
            }

            if(preconditionNodes != null)
            {
                preconditionNodes.Clear();
            }
        }


        /** 创建节点任务并初始化 */
        public SkillNodeTask CreateAndInitTask(SkillTreeTask rootTask, SkillController skill)
        {
            SkillNodeTask task = this.CreateTask();

            Debug.Check(task != null);
            task.Init(this, rootTask, skill);

            return task;
        }


        /** 创建节点任务 */
        protected abstract SkillNodeTask CreateTask();

    }
}
