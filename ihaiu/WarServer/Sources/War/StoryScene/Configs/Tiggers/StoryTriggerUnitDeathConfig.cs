using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class StoryTriggerUnitDeathConfig : StoryTriggerConfig
    {
        /// <summary>
        /// 死亡单位AvatarId
        /// </summary>
        public  int             unitAvatarId;
        /// <summary>
        /// 第几个死亡的单位
        /// </summary>
        public  int             unitDeathNum            = 1;
        public StoryTriggerUnitDeathConfig() { }
        public StoryTriggerUnitDeathConfig(int unitAvatarId, int unitDeathNum)
        {
            this.unitAvatarId = unitAvatarId;
            this.unitDeathNum = unitDeathNum;
        }
    }
}