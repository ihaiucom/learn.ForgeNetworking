using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 4:55:14 PM
*  @Description:    单位数据--建筑
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {

        #region 建筑
        /** 建筑格子ID */
        public int buildCellUid = -1;

        public BuildCellData buildCellData
        {
            get
            {
                return room.sceneData.GetBuildCell(buildCellUid);
            }
        }
        #endregion


        #region 建筑费用 和 雇佣 和 士兵死亡收获


        /// <summary>
        /// 建造费用
        /// </summary>
        public int buildCost
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.buildCost;
            }
        }

        /// <summary>
        /// 修理费用
        /// </summary>
        public int rebuildCost
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.rebuildCost;
            }
        }

        /// <summary>
        /// 占领费用
        /// </summary>
        public int occupyCost
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.occupyCost;
            }
        }

        /// <summary>
        /// 雇佣费用
        /// </summary>
        public int employCost
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.employCost;
            }
        }


        /// <summary>
        /// 怪物死亡水晶获得
        /// </summary>
        public int deathCost
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.deathCost;
            }
        }

        /// <summary>
        /// 雇佣结束条1：使用时间2：攻击次数
        /// </summary>
        public UnityEmployType employType
        {
            get
            {
                return unitLevelConfig == null ? UnityEmployType.Time : unitLevelConfig.employType;
            }
        }

        /// <summary>
        /// 雇佣参数
        /// </summary>
        public float employArg
        {
            get
            {
                return unitLevelConfig == null ? 0 : unitLevelConfig.employArg;
            }
        }

        /// <summary>
        /// 是否可以被占领
        /// </summary>
        public bool enableOccupy
        {
            get
            {
                if (unitType == UnitType.Build && buildType != UnitBuildType.Mainbase)
                {
                    return legionData.neutralBuildChangeType == LegionBuildChangType.EnableOccupy;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否可以被雇佣
        /// </summary>
        public bool enableEmploy
        {
            get
            {
                if (unitType == UnitType.Build && buildType != UnitBuildType.Mainbase)
                {
                    return legionData.neutralBuildChangeType == LegionBuildChangType.EnableEmploy;
                }
                return false;
            }
        }


        /// <summary>
        /// 雇佣 -- 势力ID
        /// </summary>
        public int employLegionId = -1;

        /// <summary>
        /// 雇佣 -- CD时间
        /// </summary>
        public float employCD = 0;

        /// <summary>
        /// 雇佣 -- 剩余攻击次数
        /// </summary>
        public int employAttackNum = 0;

        /// <summary>
        /// 使用雇佣攻击
        /// </summary>
        public bool UseEmployAttackNum()
        {
            if (employType == UnityEmployType.AttackNum)
            {
                employAttackNum--;
                if (employAttackNum <= 0)
                {
                    RemoveEmploy();
                }
            }
            return IsEmploying;
        }

        /// <summary>
        /// 是否是雇佣状态
        /// </summary>
        public bool IsEmploying
        {
            get
            {
                if (employLegionId > 0)
                {
                    return true;
                    //if (employType == UnityEmployType.AttackNum)
                    //{
                    //    return employAttackNum > 0;
                    //}
                    //else
                    //{
                    //    return employCD > 0;
                    //}
                }
                return false;
            }
        }


        /// <summary>
        /// 设置雇佣老板
        /// </summary>
        /// <param name="legionId"></param>
        public void SetEmploy(int legionId)
        {
            employLegionId = legionId;
            if (employType == UnityEmployType.AttackNum)
            {
                employAttackNum = (int)employArg;
            }
            else
            {
                employCD = employArg;
            }

            // TODO 临时

            switch (buildType)
            {
                case UnitBuildType.Tower_Switch:
                    buildCellData.SwitchSetEmploy(legionId);
                    break;
            }

            switch (buildType)
            {
                case UnitBuildType.Tower_Switch:
                case UnitBuildType.Tower_Door:
                    //UnitAgent agent = room.clientSceneView.GetUnit(uid);
                    if (unitAgent != null)
                    {
                        unitAgent.ActionOpenDoor();
                    }
                    break;
            }
        }

        /// <summary>
        /// 开始雇佣
        /// </summary>
        /// <param name="legionId"></param>
        /// <returns></returns>
        public bool BeginEmploy(int legionId)
        {
            if (enableEmploy && !IsEmploying)
            {
                SetEmploy(legionId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解除雇佣关系
        /// </summary>
        public void RemoveEmploy()
        {
            if (!IsEmploying) return;

            employLegionId = -1;
            employAttackNum = 0;
            employCD = 0;

            // TODO 临时

            switch (buildType)
            {
                case UnitBuildType.Tower_Switch:
                case UnitBuildType.Tower_Door:
                    //UnitAgent agent = room.clientSceneView.GetUnit(uid);
                    if (unitAgent != null)
                    {
                        unitAgent.ActionCloseDoor();
                    }
                    break;
            }

            switch (buildType)
            {
                case UnitBuildType.Tower_Switch:
                    buildCellData.SwitchRemoveEmploy();
                    break;
            }
            room.clientNetC.EndEmployUnit(uid);
        }
        #endregion

    }
}
