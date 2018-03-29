using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class StoryTriggerWave : StoryTrigger
    {
        StoryTriggerWaveConfig tiggerConfig;
        public override void SetConfig(StoryTriggerConfig config)
        {
            base.SetConfig(config);
            tiggerConfig = (StoryTriggerWaveConfig) config;
        }

        public override void Start()
        {
            room.spawnSolider.OnReadyEvent += OnWaveReady;
        }

        private void OnWaveReady()
        {
            if (tiggerConfig.waveIndex == room.spawnSolider.waveIndex + 1)
            {
                OnTirgger();
            }
        }


        public override void End()
        {
            room.spawnSolider.OnReadyEvent -= OnWaveReady;
        }
    }
}