using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/7/2017 9:52:19 AM
*  @Description:    接口 -- 单位安装
* ==============================================================================
*/
namespace Games
{
    public interface IUnitInstall
    {
        /** 单位--安装 */
        void UnitInstall();

        /** 单位--卸载 */
        void UnitUninstall();
    }
}
