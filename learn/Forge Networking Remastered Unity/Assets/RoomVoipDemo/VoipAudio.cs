using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Threading;
using Rooms.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoipAudio : MonoBehaviour
{
    private static VoipAudio _install;
    public static VoipAudio Install
    {
        get
        {
            if(_install == null)
            {
                _install = new GameObject("VOIP").AddComponent<VoipAudio>();
            }
            return _install;
        }
    }


    private float audio3D = 0.0f;
    private int channels = 0;
    private int Frequency = 12000;

    public AudioSource source = null;
    private AudioSource audio = null;

    private float READ_FLUSH_TIME = 0.5f;
    private float readFlushTimer = 0.0f;

    private int readUpdateId = 0;
    private int previousReadUpdateId = -1;

    private List<float> readSamples = null;

    private void Awake()
    {
        _install = this;
    }

    void Start ()
    {
        if(source == null)
		    source = gameObject.AddComponent<AudioSource>();

        audio = source;
    }
	
	void Update ()
    {
        if(Microphoneing)
        {
            ReadMic();
        }

        readFlushTimer += Time.deltaTime;
        if (readFlushTimer > READ_FLUSH_TIME)
        {
            if (readUpdateId != previousReadUpdateId && readSamples != null && readSamples.Count > 0)
            {
                previousReadUpdateId = readUpdateId;

                //lock (readSamples)
                {
                    audio.clip = AudioClip.Create("VOIP", readSamples.Count, channels, Frequency, false);
                    audio.spatialBlend = audio3D;

                    audio.clip.SetData(readSamples.ToArray(), 0);
                    if (!audio.isPlaying) audio.Play();

                    readSamples.Clear();
                }
            }

            readFlushTimer = 0.0f;
        }
    }

    public void Add(byte[] data)
    {
        float[] tmp = ToFloatArray(data);
        if (readSamples == null)
            readSamples = new List<float>(tmp);

        //lock (readSamples)
        {
            readSamples.AddRange(tmp);
        }
    }



    private int lastSample = 0;
    private AudioClip mic = null;
    private LobbyClient lobby = null;

    public void StartVOIP(LobbyClient lobby)
    {
        this.lobby = lobby;
        UnityMainThread.Run(() =>
        {
            mic = Microphone.Start(null, true, 100, Frequency);

            if (mic == null)
            {
                Debug.LogError("没有录像设备 A default microphone was not found or plugged into the system");
                return;
            }

            channels = mic.channels;
            Task.Queue(VOIPWorker);
        });
    }

    public static void StopVOIP()
    {
        if(_install != null)
            _install.isRuning = false;
    }


    private bool isRuning = false;
    BMSByte writeBuffer = new BMSByte();
    private float WRITE_FLUSH_TIME = 0.5f;
    private float writeFlushTimer = 0.0f;
    private List<float> writeSamples = new List<float>();
    private float[] samples = null;

    private void VOIPWorker()
    {
        isRuning = true;
        while (lobby.Socket.IsConnected && isRuning)
        {
            if (writeFlushTimer >= WRITE_FLUSH_TIME && writeSamples.Count > 0 && Microphoneing == false)
            {
                //lock (writeSamples)
                {
                    writeBuffer.Clear();

                    ObjectMapper.Instance.MapBytes(writeBuffer, lobby.roleInfo.uid);
                    ObjectMapper.Instance.MapBytes(writeBuffer, writeFlushTimer);
                    ObjectMapper.Instance.MapBytes(writeBuffer, ToByteArray(writeSamples));

                    writeSamples.Clear();
                }
                writeFlushTimer = 0.0f;

                Binary voice = new Binary(lobby.Socket.Time.Timestep, false, writeBuffer, Receivers.All, MessageGroupIds.VOIP, false, VoipRoute.Audio, lobby.roomInfo.roomUid);

                lobby.Send(voice, true);
            }

            UnityMainThread.ThreadSleep(10);
        }
    }


    private void ReadMic()
    {
        writeFlushTimer += Time.deltaTime;
        int pos = Microphone.GetPosition(null);
        int diff = pos - lastSample;

        if (diff > 0)
        {
            samples = new float[diff * channels];
            mic.GetData(samples, lastSample);

            //lock (writeSamples)
            {
                writeSamples.AddRange(samples);
            }
        }

        lastSample = pos;
    }

    public bool Microphoneing { get; private set; }
    public void BeginMicrophoneing()
    {
        Microphoneing = true;
    }

    public void EndMicrophoneing()
    {

        Microphoneing = false;
    }


    private byte[] ToByteArray(List<float> sampleList)
    {
        int len = sampleList.Count * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;

        for (int i = 0; i < sampleList.Count; i++)
        {
            byte[] data = BitConverter.GetBytes(sampleList[i]);
            Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }

        return byteArray;
    }

    private float[] ToFloatArray(byte[] data)
    {
        int len = data.Length / 4;
        float[] floatArray = new float[len];

        for (int i = 0; i < data.Length; i += 4)
            floatArray[i / 4] = BitConverter.ToSingle(data, i);

        return floatArray;
    }
}
