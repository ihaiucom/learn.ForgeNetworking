using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/27/2017 9:25:17 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 抽象 房间 MonoBehaviour */
    public class AbstractRoomMonoBehaviour : MonoBehaviour, IRoomObject
    {

        private WarRoom _room;
        /** 房间--门面 */
        public WarRoom room
        {
            get
            {
                if (_room == null)
                    return War.currentRoom;
                return _room;
            }
            set
            {
                _room = value;
            }
        }

        /** 房间--场景数据 */
        public WarSceneData sceneData
        {
            get
            {
                return room.sceneData;
            }
        }


        /** 房间--视图代理 */
        public WarUnityViewAgent clientViewAgent
        {
            get
            {
                return room.clientViewAgent;
            }
        }

        /** 房间--场景视图 */
        public WarUnitySceneView clientSceneView
        {
            get
            {
                if (room == null) return null;
                return room.clientSceneView;
            }
        }

        /** 房间--场景视图 */
        public WarUnityRes clientRes
        {
            get
            {
                return room.clientRes;
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


        /** 设置房间 */
        virtual public void SetRoom(WarRoom room)
        {
            this.room = room;
        }
    }
}
