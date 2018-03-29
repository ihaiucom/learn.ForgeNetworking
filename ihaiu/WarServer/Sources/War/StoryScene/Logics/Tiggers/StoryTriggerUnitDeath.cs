using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class StoryTriggerUnitDeath : StoryTrigger
    {
        private int deathNum = 0;
        private StoryTriggerUnitDeathConfig tiggerConfig;
        public override void SetConfig(StoryTriggerConfig config)
        {
            base.SetConfig(config);
            tiggerConfig = (StoryTriggerUnitDeathConfig)config;
        }

        public override void Start()
        {
            deathNum = 0;
            UIHandler.OnUnitDeath += UIHandler_OnUnitDeath;
        }

        private void UIHandler_OnUnitDeath(int deathUnitId)
        {
            if (tiggerConfig.unitAvatarId == deathUnitId)
            {
                deathNum++;
                if (tiggerConfig.unitDeathNum == deathNum)
                {
                    OnTirgger();
                }
            }
        }
        
        public override void End()
        {
            UIHandler.OnUnitDeath -= UIHandler_OnUnitDeath;
        }
    }
}