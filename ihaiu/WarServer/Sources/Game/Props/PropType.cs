using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 10:02:18 AM
*  @Description:    属性类型
* ==============================================================================
*/
namespace Games
{
    public enum PropType
    {
        /** 基础属性 */
        Base,

        /** 附加具体值, 相对基础值 */
        Basesadd,

        /** 附加百分比, 相对基础值 */
        Baseper,

        /** 附加百分比，相对上限值，比如血量 */
        Maxper,

        /** 附加具体值 */
        Add,

        /** 附加百分比 */
        Per,

        /** 终于属性 */
        Final,
    }
}
