﻿using BeardedManStudios;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/26/2018 3:10:55 PM
*  @Description:    
* ==============================================================================
*/
namespace Rooms.Forge.Networking
{
    public interface IRoomInfo
    {
        int ClassId { get; }

        void MapBytes(BMSByte data);
        void ReadBytes(BMSByte StreamData);


        ulong roomUid { get; set; }
        int stageClassId { get; set; }


    }
}