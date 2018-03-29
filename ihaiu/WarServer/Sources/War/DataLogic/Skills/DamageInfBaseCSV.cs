using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class DamageInfBaseCSV
    {
        public  DamageType                              damageType;
        /// <summary>
        /// 参考属性类型
        /// </summary>
        public  SelectTarget                            selectTarget;
        /// <summary>
        /// 基础表id
        /// </summary>
        public  int                                     valid;
        /// <summary>
        /// 参考属性id
        /// </summary>
        public  int                                     refId;
        /// <summary>
        /// 参考类型
        /// </summary>
        public  PropType                                refRpopType;
        /// <summary>
        /// 加成系数
        /// </summary>
        public  float                                   refRatio;
        /// <summary>
        /// 具体结果
        /// </summary>
        public  float                                   resultVal;
        /// <summary>
        /// 技能等级
        /// </summary>
        public  int                                     skillLv;
    }
}