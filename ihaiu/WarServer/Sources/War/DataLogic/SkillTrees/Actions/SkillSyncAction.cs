using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/8/2017 10:07:00 AM
*  @Description:    技能树--同步执行节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 同步执行节点
    /// 帧同步     : 发送指令[Client] -> 广播指令[Server] -> 处理指令[Client] -> 执行结果[Server]
    /// 状态同步   : 发送指令[Client] -> 处理指令[Server] -> 广播结果[Server] -> 执行结果[Client]
    /// </summary>
    public class SkillSyncAction : SkillAction
    {

        protected override SkillNodeTask CreateTask()
        {
            throw new NotImplementedException();
        }

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

        }
    }
}
