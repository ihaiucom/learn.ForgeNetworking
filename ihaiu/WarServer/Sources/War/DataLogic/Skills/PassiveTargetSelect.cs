using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class PassiveTargetSelect
    {
        /// <summary>
        /// 以源为中心
        /// </summary>
        public  bool                                    bOrigin                     = false;
        public  bool                                    bOriginSelf                 = true;
        /// <summary>
        /// 以目标为中心
        /// </summary>
        public  bool                                    bTarget                     = false;
        public  bool                                    bTargetSelf                 = true;

        /// <summary>
        /// 最大半径
        /// </summary>
        public  float                                   maxRadius                   = 1;
        /// <summary>
        /// 最小半径
        /// </summary>
        public  float                                   minRadius                   = 0;
    }
}
