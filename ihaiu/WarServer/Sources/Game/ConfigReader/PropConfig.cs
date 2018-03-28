using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/5/2017 3:43:20 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class PropConfig
    {

        /** ID */
        public int          id;
        /** 常量名称 */
        public string       constName;
        /** 字段名称 */
        public string       field;
        /** 英文名称 */
        public string       enName;
        /** 中文名称 */
        public string       cnName;
        /** 图标 */
        public string       icon;
        /** 描述 */
        public string       tip;
        /** 属性分组类型 */
        public PropGroupType groupType;


        public override string ToString()
        {
            return string.Format("PropConfig {0},   {1},    {2},    {3},    {4}", id, constName, cnName, field, groupType);
        }

    }
}
