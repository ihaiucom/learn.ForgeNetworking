using System;
using System.Collections.Generic;
using Games.Wars;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/11/2017 10:41:56 AM
*  @Description:    技能节点任务
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /** 技能节点任务 */
    public class SkillNodeTask
    {
        /** 节点索引 */
        private int                 index;
        /** 流程 */
        protected SNProcess         process;
        /** 状态 */
        protected SNStatues         status;
        /** 节点ID */
        protected int               id;
        /** 节点配置 */
        protected SkillNode         node;
        /** 父任务节点 */
        protected SkillNodeTask     parentTask;
        /** 根任务节点 */
        protected SkillTreeTask     rootTask;
        /** 技能控制器 */
        protected SkillController   skill;

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }



        public int GetId()
        {
            return id;
        }

        public SNStatues GetStatus()
        {
            return status;
        }

        public void SetStatus(SNStatues status)
        {
            this.status = status;
        }

        public SkillNode Node
        {
            get
            {
                return node;
            }
        }


        public SkillNodeTask SkillParentTask
        {
            get
            {
                return parentTask;
            }

            set
            {
                parentTask = value;
            }
        }

        public SkillNodeTask()
        {
            this.status     = SNStatues.INVALID;
            this.parentTask = null;
            this.node       = null;
        }

        /** 初始化 */
        public virtual void Init(SkillNode node, SkillTreeTask root, SkillController skill)
        {
            this.id         = node.Id;
            this.node       = node;
            this.skill      = skill;
            this.rootTask   = root;
            this.index      = root.INDEX_ADD++;
        }


        /** 清理 */
        public virtual void Clear()
        {
            this.index          = -1;
            this.id             = -1;
            this.parentTask     = null;
            this.rootTask       = null;
            this.skill          = null;
            this.status         = SNStatues.INVALID;
            this.process        = SNProcess.NONE;
        }


        /** 执行 */
        public virtual SNStatues Tick()
        {
            // 检测前提条件
            if(process == SNProcess.NONE)
            {
                status = SNStatues.RUNNING;
                process = SNProcess.PRECONDITION;
                bool bValid = CheckPrecondition();

                // 前提条件验证未通过，直接跳过该执行
                if(!bValid)
                {
                    status = SNStatues.SUCCESS;
                    return status;
                }
                process = SNProcess.ENTER;
            }
            // 前摇
            if(process == SNProcess.ENTER)
            {
                status = OnEnter();
                if(status == SNStatues.RUNNING)
                {
                    return status;
                }

                if(status == SNStatues.FAILURE)
                {
                    process = SNProcess.END;
                    SNStatues result = status;
                    OnExit();
                    return result;
                }

                process = SNProcess.EXECUTE;
            }

            // 结算
            if(process == SNProcess.EXECUTE)
            {
                status = OnExecute();
                if(status != SNStatues.RUNNING)
                {
                    process = SNProcess.END;
                    SNStatues result = status;
                    OnExit();
                    return result;
                }
            }


            // 后摇
            return status;
        }

        /** 检测前提条件 */
        public virtual bool CheckPrecondition()
        {
            if(node.PrecoditionCount == 0)
            {
                return true;
            }

            bool result = true;
            for(int i = 0; i < node.PrecoditionCount; i ++)
            {
                Precondition precondition = node.GetPrecodition(i);
                precondition.Evaluate(this, ref result);
            }
            return result;
        }

        /** 前摇 */
        public virtual SNStatues OnEnter()
        {
            return SNStatues.SUCCESS;
        }

        /** 结算 */
        public virtual SNStatues OnExecute()
        {
            return SNStatues.SUCCESS;
        }


        /** 后摇 */
        public virtual void OnExit()
        {
            process = SNProcess.NONE;
            status  = SNStatues.INVALID;
        }


        #region Sync
        /// <summary>
        /// 发送指令
        /// [Client]
        /// </summary>
        public void SendCmd()
        {

        }


        /// <summary>
        /// 处理指令
        /// [Server]
        /// </summary>
        public void ExeCmd()
        {

        }

        /// <summary>
        /// 广播结果
        /// [Server]
        /// </summary>
        public void BroadcastResult()
        {

        }

        /// <summary>
        /// 执行结果
        /// [Client]
        /// </summary>
        public void ExeResult()
        {
            process = SNProcess.EXECUTE;
        }
        #endregion


    }
}
