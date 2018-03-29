using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games
{
    /// <summary>
    /// 计算Buff,Halo附加属性
    /// <see cref="Games.PropAttachData.App(PropUnit)"/>
    /// </summary>
    public class BuffProp
    {
        // 目标属性ID
        public  int                     aimId;
        // 属性类型
        public  PropType                propType                    = PropType.Base;

        // 参考目标属性ID
        public  int                     refAimId;
        // 参考目标属性类型
        public  PropType                refPropType                 = PropType.Base;

        // 系数 百分比
        public  float                   ratio                       = 1;
        // 读取配置表里的ID  SkillValue.xmlx 里的ID
        public int                     valId                       = 0;

        /// <summary>
        /// 计算出来的结果
        /// <see cref="Games.Wars.HaloBuffManager.AddHalos()">valValue = Game.config.skillValue.GetConfigs(haloBuff.buffPropList[i].valId, skilllv);</see>
        /// </summary>
        public float                   valValue                    = 0;

    }
}