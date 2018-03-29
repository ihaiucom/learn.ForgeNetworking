using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarAudioMusic
{
    private static WarAudioMusic _Install;
    public static WarAudioMusic Install
    {
        get
        {
            if(_Install == null)
            {
                _Install = new WarAudioMusic();
                _Install.Init();
            }
            return _Install;
        }
    }



    public EventInstance eventInstance;

    public void Init()
    {
        eventInstance = Game.audio.CreateSoundInstance(SoundKeys.Music_Battle_01);
    }

    public void Start()
    {
        if(Game.audio.setting.enableMusic)
        {
            eventInstance.start();
        }
    }

    public void Stop()
    {
        eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public float[] readys   = new float[] { 0.0f, 0.2f, 0.3f, 0.5f, 0.6f };
    public float[] waves    = new float[] { 0.1f, 0.1f, 0.4f, 0.4f, 0.7f };

    public void WaveReady(int waveIndex)
    {
        int index = Mathf.Clamp(waveIndex, 0, readys.Length);
        float val = readys[index];
        eventInstance.setParameterValue("Parameter 1", val);
    }

    public void WaveStart(int waveIndex)
    {
        int index = Mathf.Clamp(waveIndex, 0, waves.Length);
        float val = waves[index];
        eventInstance.setParameterValue("Parameter 1", val);
    }

}
