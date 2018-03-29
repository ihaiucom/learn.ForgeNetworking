using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 战斗常量配置
    /// </summary>
    public class WarConstConfig : AbstractRoomObject
    {
        public WarConstConfig(WarRoom room) 
        {
            this.room = room;
        }

        /** AI参数--积分权重--仇恨值 */
        public float AIWeightHatred
        {
            get
            {
                return 1;
            }
        }

        /** AI参数--积分权重--单位类型 */
        public float AIWeightType
        {
            get
            {
                return 1;
            }
        }



        /** AI参数--积分权重--距离 */
        public float AIWeightDistance
        {
            get
            {
                return 1;
            }
        }



        /// <summary>
        /// AI参数--获取单位类型积分
        /// </summary>
        /// <param name="src">元单位类型</param>
        /// <param name="target">目标单位类型</param>
        /// <returns></returns>
        public float GetAIUnitTypeScore(UnitType src, UnitType target)
        {
            return 1;
        }

   
        /// <summary>
        /// 获取回收能量
        /// </summary>
        /// <param name="unit">机关单位</param>
        /// <returns></returns>
        public int GetRecoverEngery(UnitData unit)
        {
            //回收资源=int(建造资源×0.5×当前血量百分比)
            int engery = (int)(unit.buildCost * unit.prop.HpRate * 0.5f);
            if (engery < 1)
            {
                engery = 1;
            }
            return engery;
        }

        public  bool    IsEnoughEngery(int cost)
        {
            return room.clientOperationUnit.Energy > cost;
        }
    }

}