using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 4:00:21 PM
*  @Description:    抽象 房间对象
* ==============================================================================
*/
namespace Games.Wars
{
    /** 抽象 房间对象 */
    abstract public class AbstractRoomObject : IRoomObject
    {
        /** 房间--门面 */
        public WarRoom room                 { get; set; }

        /** 房间--场景数据 */
        public WarSceneData sceneData
        {
            get
            {
                return room.sceneData;
            }
        }


        /** 房间--时间 */
        public WarTime Time
        {
            get
            {
                return room.Time;
            }
        }


        /** 房间--逻辑时间 */
        public WarLTime LTime
        {
            get
            {
                return room.LTime;
            }
        }
    }
}
