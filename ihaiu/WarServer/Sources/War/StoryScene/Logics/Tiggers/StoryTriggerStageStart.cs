using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class StoryTriggerStageStart : StoryTrigger
    {
        public override void Start()
        {
            room.EventStarted += OnGameStarted;
        }

        private void OnGameStarted()
        {
            OnTirgger();
        }

        public override void End()
        {
            room.EventStarted -= OnGameStarted;
        }
    }
}