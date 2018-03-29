using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/12/2017 7:16:25 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 战斗驱动接口
    /// </summary>
    public interface IWarDrive 
    {

        /** 设置房间 */
        void SetRoom(WarRoom room);

        /** 销毁 */
        void DestoryDirve();
    }
}
