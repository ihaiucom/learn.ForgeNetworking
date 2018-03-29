using System;
using System.Collections.Generic;

/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 8:04:36 PM
*  @Description:    接口 -- 房间对象
* ==============================================================================
*/
namespace Games.Wars
{
    public interface IRoomObject
    {
        /** 房间--门面 */
        WarRoom         room {get; set;}

        /** 房间--场景数据 */
        WarSceneData    sceneData { get; }

        /** 房间--时间 */
        WarTime         Time { get; }

        /** 房间--逻辑时间 */
        WarLTime LTime { get; }
    }
}
