﻿using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/1/2018 10:20:22 AM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Ihaiu.Forge.Networking
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class NetRoleInfo
    {
        public ulong uid;
        public string name;

        // 玩家加入
        internal void OnPlayerJoined()
        {

        }

        // 玩家离开
        internal void OnPlayerLeft()
        {

        }
    }
}
