using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 5:08:17 PM
*  @Description:    单位数据--锚点
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {
        /// <summary>
        /// 血条，飘血起点位置
        /// </summary>
        public Vector3 BloodStartPos
        {
            get
            {
                if (unitAgent != null && unitAgent.animatorManager != null && unitAgent.animatorManager.BloodPos != null)
                {
                    return unitAgent.animatorManager.BloodPos.position;
                }
                return position;
            }
        }

        /// <summary>
        /// 受击点
        /// </summary>
        public Vector3 AnchorAttackbyPos
        {
            get
            {
                if (unitAgent != null && unitAgent.animatorManager != null && unitAgent.animatorManager.AnchorShotBy != null)
                {
                    return unitAgent.animatorManager.AnchorShotBy.position;
                }
                return position;
            }
        }

        /// <summary>
        /// 受击点
        /// </summary>
        public Transform AnchorAttackbyTform
        {
            get
            {
                if (unitAgent == null) return null;
                if (unitAgent.animatorManager != null && unitAgent.animatorManager.AnchorShotBy != null)
                {
                    return unitAgent.animatorManager.AnchorShotBy;
                }
                return unitAgent.modelTform;
            }
        }


        /// <summary>
        /// 攻击点
        /// </summary>
        public Vector3 AnchorAttackShotPos
        {
            get
            {
                if (unitAgent != null && unitAgent.animatorManager != null && unitAgent.animatorManager.ShotPos != null)
                {
                    return unitAgent.animatorManager.ShotPos.position;
                }
                return position;
            }
        }
    }
}
