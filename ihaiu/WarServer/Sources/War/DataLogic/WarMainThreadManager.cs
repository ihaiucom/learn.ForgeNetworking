using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 9:40:00 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class WarMainThreadManager : AbstractRoomObject
    {
        public WarMainThreadManager(WarRoom room)
        {
            this.room = room;
        }

        public delegate void UpdateEvent();
        public event UpdateEvent updateEvent;

        private List<Action> onceList = new List<Action>();


        public void AddOnceUpdate(Action action)
        {
            onceList.Add(action);
        }


        private List<Action> copiedActions = new List<Action>();
        public void Update()
        {
            if(updateEvent != null)
            {
                updateEvent();
            }

            if(onceList.Count > 0)
            {
                copiedActions.AddRange(onceList.ToArray());
                onceList.Clear();

                for(int i = 0; i < copiedActions.Count; i ++)
                {
                    copiedActions[i]();
                }
                copiedActions.Clear();
            }
        }

    }
}
