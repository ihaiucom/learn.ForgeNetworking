using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 11:40:28 AM
*  @Description:    
* ==============================================================================
*/

namespace Games
{
    public partial class PropUnit
    {
        /// <summary>
        /// 是否可以攻击
        /// </summary>
        public bool EnableAttack
        {
            get
            {
                if(StateFreezed || StateVertigo || StateSilence)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// 是否冻结，单位不能动
        /// </summary>
        public bool IsFreezed
        {
            get
            {
                return StateFreezed || StateVertigo;
            }

            set
            {
                StateFreezed = false;
                StateVertigo = false;
            }
        }

        /// <summary>
        /// 当前血量百分比
        /// </summary>
        public float HpRate
        {
            get
            {
                return HpMax == 0 ? 1 : Hp / HpMax;
            }
        }
    }
}
