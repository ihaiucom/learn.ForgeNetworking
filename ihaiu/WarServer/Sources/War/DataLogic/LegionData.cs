using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 4:25:18 PM
*  @Description:    势力数据
* ==============================================================================
*/
namespace Games.Wars
{
    /** 势力数据 */
    public class LegionData : PooledClassObject
    {
        public WarRoom room;

        public void SetLegionData(WarRoom room)
        {
            this.room = room;
        }

        /** 势力ID */
        public int                      legionId;

        /** 势力类型 */
        public LegionType               type;

        /** 中立势力建筑关系类型 */
        public LegionBuildChangType     neutralBuildChangeType;

        /** 是否是机器人 */
        public bool                     isRobot = false;

        /** 机器人ID */
        public int                      robotId;

        /** 势力组 */
        public LegionGroupData          group;

        /** 是否在线 */
        public bool                     isOnline = true;

        /** 主基地单位 */
        public UnitData                 mainbaseUnit;

        /** 主单位,玩家默认是英雄 */
        public UnitData                 mainUnit;

        /** 复活时间 */
        public int                      reliveSecond = 0;

        public bool IsLive
        {
            get
            {
                return HasMainUnit ? mainUnit.isLive : true;
            }
        }

        /// <summary>
        /// 是否有主单位
        /// </summary>
        public bool HasMainUnit
        {
            get
            {
                return mainUnit != null;
            }
        }


        /// <summary>
        /// 是否有主基地单位
        /// </summary>
        public bool HasMainbaseUnit
        {
            get
            {
                return mainbaseUnit != null;
            }
        }



        /// <summary>
        /// 当前能量
        /// </summary>
        public float Energy
        {
            get
            { 
                return HasMainUnit ? mainUnit.prop.Energy : 0;
            }

            set
            {
                if(HasMainUnit)
                {
                    if (value < 0) value = 0;
                    if (value > EnergyMax) value = EnergyMax;
                    mainUnit.prop.Energy = value;
                }
            }
        }


        /// <summary>
        /// 能量上限
        /// </summary>
        public float EnergyMax
        {
            get
            {
                return HasMainUnit ? mainUnit.prop.EnergyMax : 1;
            }

            set
            {
                if (HasMainUnit)
                    mainUnit.prop.EnergyMax = value;
            }
        }

        /// <summary>
        /// 能量恢复速度
        /// </summary>
        public float EnergyRecover
        {
            get
            {
                return HasMainbaseUnit ? mainbaseUnit.prop.EnergyRecover : 0;
            }
        }


        public void AddEnergy(float val)
        {
            Energy += val;
        }


        /** 角色信息 */
        public RoleInfo roleInfo = new RoleInfo();
        ///** 角色名称 */
        public string roleName
        {
            get
            {
                return roleInfo.roleName;
            }
        }

        /** 角色ID */
        public int roleId
        {
            get
            {
                return roleInfo.roleId;
            }
        }




        /** 是否是中立 */
        public bool IsNeutral
        {
            get
            {
                return type == LegionType.Neutral;
            }
        }


        /** 是否是怪物 */
        public bool IsMonster
        {
            get
            {
                return type == LegionType.Monster;
            }
        }


        /** 获取关系类型  */
        public RelationType GetRelation(int targetLegionId)
        {
            if(legionId == targetLegionId)
            {
                return RelationType.Own;
            }
            else if(group != null)
            {
                return group.IsOnceGroup(targetLegionId) ? RelationType.Friendly : RelationType.Enemy;
            }
            return RelationType.Enemy;
        }

        #region Pool
        public override void OnRelease()
        {
            roleInfo    = null;
            legionId    = 0;
            type        = LegionType.None;
            group       = null;
            isRobot     = false;
            robotId          = 0;
            base.OnRelease();
        }
        #endregion


        public void OnSecond()
        {
            // 能量恢复
            Energy += EnergyRecover;
            if(Energy > EnergyMax)
            {
                Energy = EnergyMax;
            }

            if(room.stageType != StageType.Dungeon)
            {
                if (!IsLive)
                {
                    if (reliveSecond > 0)
                        reliveSecond--;

                    if (reliveSecond <= 0)
                    {
                        room.clientNetS.SetUnitIsLive(mainUnit.uid, true);
                    }
                }
            }
        }
    }
}
