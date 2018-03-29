using System;
using System.Collections.Generic;
using Games.Wars;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 7:48:06 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 关卡--区域 */
    [Serializable]
    public class RegionData
    {
        /** 区域ID */
        public int                  regionId;
        /** 区域名称 */
        public string               name;
        /** 区域功能类型 */
        [SerializeField]
        public RegionFuncType       funcType;
        /** 区域形状类型 */
        [SerializeField]
        public RegionShapeType      shapeType;
        /** 坐标 */
        [SerializeField]
        public Vector3                position = Vector3.zero;
        /** 方向 */
        [SerializeField]
        public Vector3                rotation = Vector3.zero;

        /** [圆形] 半径 */
        [SerializeField]
        public float radius = 5f;
        /** [矩形] 半径 */
        [SerializeField]
        public Rect rect   = new Rect(-5, -5, 5, 5);
        

        public RegionData Clone()
        {
            RegionData item = new RegionData();
            item.regionId = regionId;
            item.name = name;
            item.funcType = funcType;
            item.shapeType = shapeType;
            item.position = position;
            item.rotation = rotation;
            item.radius = radius;
            item.rect = rect;
            return item;
        }


        /** 生成玩家出生点位置 */
        virtual public Vector3 GenerateSpawnPlayerPosition()
        {
            return position;
        }


        /** 生成玩家出生点位置 */
        virtual public Vector3 GenerateSpawnPlayerRotation()
        {
            return rotation;
        }


        
    }
}
