using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/18/2017 5:26:16 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 建筑格子数据
    /// </summary>
    public class BuildCellData
    {
        public WarRoom room;
        public BuildCellData(WarRoom room)
        {
            this.room = room;
        }
        /** 格子Uid */
        public int  uid;
        /** 航线ID */
        public int  routeId = -1;
        /** 坐标 */
        public Vector3 position = Vector3.zero;
        /** 方向 */
        public Vector3 rotation = Vector3.zero;
        /** 大小 */
        public Vector2 size = new Vector2(6, 6);

        /** 关联建筑格子 */
        public List<int> switchBuildCellList = new List<int>();

        /** 当前格子上的建筑单位 */
        public UnitData unit = null;

        /** 是否已经被使用 */
        public bool hasUnit
        {
            get
            {
                return unit != null;
            }
        }

        /** 是否是自己的建筑 */
        public bool clientIsOwn
        {
            get
            {
                return unit.clientIsOwn;
            }
        }

        /// <summary>
        /// 死亡建造，可修理
        /// </summary>
        public bool hasDeath
        {
            get
            {
                return unit.prop.Hp <= 0;
            }
        }

        /** 设置单位uid */
        public void SetUnit(UnitData unit)
        {
            this.unit = unit;
        }

        /** 设置格子上没有单位 */
        public void SetUnitEmpty()
        {
            this.unit = null;
        }
        
        /// <summary>
        /// 开关，关联其他建筑格子的单位
        /// </summary>
        public void SwitchSetEmploy(int legionId)
        {
            foreach(int cellId in switchBuildCellList)
            {
                BuildCellData cell = room.sceneData.GetBuildCell(cellId);
                if(cell != null && cell.hasUnit)
                {
                    cell.unit.SetEmploy(legionId);
                }
            }
        }


        /// <summary>
        /// 开关，关联其他建筑格子的单位
        /// </summary>
        public void SwitchRemoveEmploy()
        {
            foreach (int cellId in switchBuildCellList)
            {
                BuildCellData cell = room.sceneData.GetBuildCell(cellId);
                if (cell != null && cell.hasUnit)
                {
                    cell.unit.RemoveEmploy();
                }
            }
        }

    }
}
