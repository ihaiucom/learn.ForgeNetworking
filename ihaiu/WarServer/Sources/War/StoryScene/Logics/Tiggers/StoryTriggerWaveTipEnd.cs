using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class StoryTriggerWaveTipEnd : StoryTrigger
    {
        private StoryTriggerWaveConfig tiggerConfig;
        public override void SetConfig(StoryTriggerConfig config)
        {
            base.SetConfig(config);
            tiggerConfig = (StoryTriggerWaveConfig)config;
        }

        public override void Start()
        {
            room.spawnSolider.OnSpawnEvent += OnWaveSpawn;
        }

        private void OnWaveSpawn()
        {
            if (tiggerConfig.waveIndex == room.spawnSolider.waveIndex + 1)
            {
                OnTirgger();
            }
        }

        public override void End()
        {
            room.spawnSolider.OnSpawnEvent -= OnWaveSpawn;
        }
    }
}