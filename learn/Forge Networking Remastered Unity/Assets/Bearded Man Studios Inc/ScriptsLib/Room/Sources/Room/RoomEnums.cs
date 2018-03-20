using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/28/2018 7:30:40 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    /// <summary>
    /// 加入房间结果
    /// </summary>
    public enum NetJoinRoomResult
    {
        // 成功
        Successed,
        // 已存在
        Existed,
        // 失败, 不存在房间
        Failed_NoRoom
    }

    /// <summary>
    /// 离开房间结果
    /// </summary>
    public enum NetLeftRoomResult
    {
        // 成功
        Successed,
        // 失败, 房间不存在该玩家
        Failed_RoomNoPlayer,
        // 失败, 不是同一个终端
        Failed_NoSameSocket,
        // 失败, 不存在房间
        Failed_NoRoom,
    }
}
