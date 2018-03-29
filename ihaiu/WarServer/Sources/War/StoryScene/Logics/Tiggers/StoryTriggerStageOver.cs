using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class StoryTriggerStageOver : StoryTrigger
    {
        public WarOverType overType;

        public StoryTriggerStageOver(WarOverType overType)
        {
            this.overType = overType;
        }

        public override void Start()
        {
            room.EventOver += OnGameOver;
        }
        
        private void OnGameOver(WarOverType overType)
        {
            if(overType == this.overType)
            {
                OnTirgger();
            }
        }
        
        public override void End()
        {
            room.EventOver -= OnGameOver;
        }
    }
}