using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 8:01:24 PM
*  @Description:    接口 -- 单位组件
* ==============================================================================
*/
namespace Games.Wars
{
    public interface IUnitComponent
    {
        /** 单位数据 */
        UnitData unitData { get; set; }


        /** 势力数据 */
        LegionData legionData { get;}



        /** 单位--安装 */
        void OnUnitInstall();

        /** 单位--卸载 */
        void OnUnitUninstall();
    }
}
