using System;
using System.Collections.Generic;
using Games.SkillTrees;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 5:43:08 PM
*  @Description:    技能调用接口
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 技能调用接口
    /// - 技能开始：开始执行技能，若技能不循环进行，则技能可以自动结束。
    /// - 技能结束：有的技能不能自己结束，比如普通攻击，当玩家松开按钮，调用技能结束接口，告诉当前技能使其结束，此时技能到达后摇点时，技能不再继续执行。
    /// - 技能停止：当技能被强制打断时，如被攻击、晕眩等，技能会被强制停止。（此功能没有实现，感觉优先级较低）
    /// - 技能真正的结束后，提供回调接口。
    /// 此外，技能控制器负责** 目标查找**，技能在技能树执行过程中，从技能控制器中获得目标。
    /// 管理技能动态目标选择
    /// </summary>
    public class SkillController : IRoomObject
    {

        #region IRoomObject
        /** 房间--门面 */
        public WarRoom room { get; set; }
        /** 单位 */
        public UnitData unit;

        public SkillController(WarRoom room, UnitData unit)
        {
            this.room = room;
            this.unit = unit;
        }

        /** 房间--场景数据 */
        public WarSceneData sceneData
        {
            get
            {
                return room.sceneData;
            }
        }



        /** 房间--时间 */
        public WarTime Time
        {
            get
            {
                return room.Time;
            }
        }

        /** 房间--逻辑时间 */
        public WarLTime LTime
        {
            get
            {
                return room.LTime;
            }
        }
        #endregion

        public int skillUid;
        public int skillId;
        public int skillLevel;
        public SkillConfig skillConfig;
        public SkillLevelConfig skillLevelConfig;
        public float cd = 0;
        public bool HasConfig
        {
            get
            {
                return skillConfig != null;
            }
        }

        //private SkillBass _skillBass = null;
        //public SkillBass skillBass
        //{
        //    get
        //    {
        //        if (skillId > 0)
        //        {
        //            _skillBass = Game.config.skillEffect.GetSkillBassConfig(skillId);
        //            _skillBass.skillLv = skillLevel;
        //            //UnitAgent unitAgent = room.clientSceneView.GetUnit(unit.uid);
        //            //if (unitAgent != null)
        //            //    _skillBass = unitAgent.warUnitcontrol.skillBass(skillId);
        //        }
        //        return _skillBass;
        //    }
        //}

        private SkillInfoConfig _skillInfoConfig = null;
        public SkillInfoConfig skillInfoConfig
        {
            get
            {
                if (skillId > 0)
                {
                    _skillInfoConfig = room.skillManager.GetSkillInfoConfig(skillId);
                }
                return _skillInfoConfig;
            }
        }
        public void ChangeSkillConfig(int _skillid)
        {
            skillId = _skillid;
            _skillInfoConfig = room.skillManager.GetSkillInfoConfig(_skillid);
        }

        public float attackRadius
        {
            get
            {
                float radius = 0;
                //if (skillBass != null && skillBass.skillActivation.activation == Activation.User)
                //{
                //    switch (skillBass.aimDirectionList[0].targetLocation)
                //    {
                //        case TargetLocation.LinearWave:
                //        case TargetLocation.FanWave:
                //        case TargetLocation.CircularShockwave:
                //            {
                //                radius = skillBass.aimDirectionList[0].targetFanRadius;
                //            }
                //            break;
                //        case TargetLocation.TargetCircleArea:
                //            {
                //                radius = skillBass.aimDirectionList[0].targetCircleAreaDis;
                //            }
                //            break;
                //    }
                //}
                if (skillInfoConfig != null && skillInfoConfig.activation == Activation.User)
                {
                    SkillTriggerUse skillTriggerUse = (SkillTriggerUse)skillInfoConfig.skillTriggerConfig;
                    switch (skillTriggerUse.targetLocation)
                    {
                        case TargetLocation.FanWave:
                            {
                                radius = ((SkillTriggerUseFanWave)skillTriggerUse.skillTriggerLocation).targetFanRadius;
                            }
                            break;
                        case TargetLocation.LinearWave:
                            {
                                radius = ((SkillTriggerUseLinearWave)skillTriggerUse.skillTriggerLocation).targetFanRadius;
                            }
                            break;
                        case TargetLocation.TargetCircleArea:
                            {
                                radius = ((SkillTriggerUseTargetCircleArea)skillTriggerUse.skillTriggerLocation).targetCircleAreaDis;
                            }
                            break;
                    }

                }

                return radius;
            }
        }

        /** 技能树配置 */
        public SkillTree        tree;
        /** 技能树任务 */
        public SkillTreeTask    task;

        public void Update()
        {
            if (cd > 0)
            {
                cd -= LTime.deltaTime;
                if (cd < 0)
                    cd = 0;
            }
        }

        /// <summary>
        /// 使用
        /// </summary>
        public void Use(UnitData unit)
        {
            if (skillConfig != null)
            {
                //UnitAgent unitAgent = room.clientSceneView.GetUnit(unit.uid);
                //if (unitAgent != null)
                //{
                //    //room.clientOperationUnit.OnSendAttackOtherBySkillId(unitAgent, skillId);
                //    unitAgent.unitControl.Use(unitAgent, skillId);
                //}
                room.skillManager.Init(unit.unitAgent, skillId);
            }
        }


        /// <summary>
        /// 技能开始
        /// 开始执行技能，若技能不循环进行，则技能可以自动结束。
        /// </summary>
        public void Enter()
        {
            task = (SkillTreeTask)tree.GenerateTreeTask(this);
            room.skillTreeManager.AddTask(task);
        }

        /// <summary>
        /// 技能结束
        /// 有的技能不能自己结束，比如普通攻击，当玩家松开按钮，调用技能结束接口，告诉当前技能使其结束，此时技能到达后摇点时，技能不再继续执行。
        /// </summary>
        public void Exit()
        {
            room.skillTreeManager.RemoveTask(task);
        }

        /// <summary>
        /// 技能停止
        /// 当技能被强制打断时，如被攻击、晕眩等，技能会被强制停止。（此功能没有实现，感觉优先级较低）
        /// </summary>
        public void Kill()
        {

        }




        #region Sync
        /// <summary>
        /// 发送指令
        /// [Client]
        /// </summary>
        public void SendCmd(int index, NodeArg arg)
        {
            // 广播节点前摇
            BroadcastEnter(index);

            // 处理指令
            ExeCmd(index, arg);

        }


        /// <summary>
        /// 处理指令
        /// [Server]
        /// </summary>
        public void ExeCmd(int index, NodeArg arg)
        {
            // 广播结果
            BroadcastResult();
        }

        /// <summary>
        /// 广播节点前摇
        /// [Server]
        /// </summary>
        public void BroadcastEnter(int index)
        {


        }

        /// <summary>
        /// 执行前摇
        /// [Client]
        /// </summary>
        public void ExeEnter()
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
        #endregion
    }
}
