
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 8:25:21 PM
*  @Description:    技能树--执行节点
* ==============================================================================
*/
namespace Games.SkillTrees
{
    /// <summary>
    /// 执行节点
    /// </summary>
    public class SkillAction : SkillNode
    {
        ///// <summary>
        ///// 前摇时间：技能开始，但是技能真正的结算流程还没开始。技能开始以后，技能相关的特效和动作就开始播放。
        ///// </summary>
        //public void Enter()
        //{

        //}

        ///// <summary>
        ///// 前摇时间结束：技能前摇结束时技能开始真正的释放以及结算，等技能前摇结束以后，技能真正的释放并结算。释放包括创建相应的弹道／法术场和buff。
        ///// </summary>
        //public void Exe()
        //{

        //}

        ///// <summary>
        ///// 技能后摇点：技能播放到后摇点时间时，技能真正的结束。这时，技能对应的特效以及人物动作可能还会继续播放，但是技能流程已经正式结束了。也就是说，下一个技能可以执行。
        ///// </summary>
        //public void End()
        //{

        //}

        protected override SkillNodeTask CreateTask()
        {
            throw new NotImplementedException();
        }
    }
}
