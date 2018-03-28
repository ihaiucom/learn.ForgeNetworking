using Rooms.Forge.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMsgItem : MonoBehaviour {

    public Text playerNameText;

    public GameObject textMsg;
    public GameObject audioMsg;

    [Header("Text Msg")]
    public Text textMsgText;

    [Header("Audio Msg")]
    public Text audioMsgText;
    private byte[] audioSamples;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetSystemMsg(string msg)
    {
        playerNameText.gameObject.SetActive(false);
        audioMsg.gameObject.SetActive(false);
        textMsg.gameObject.SetActive(true);
        textMsgText.text = msg;
    }

    public void SetTextMsg(IRoleInfo role, string msg)
    {
        playerNameText.gameObject.SetActive(true);
        audioMsg.gameObject.SetActive(false);
        textMsg.gameObject.SetActive(true);
        textMsgText.text = msg;
        playerNameText.text = role.name + " ("+role.uid+")";
    }

    public void SetAudioMsg(IRoleInfo role, byte[] audioSamples, float time)
    {
        this.audioSamples = audioSamples;
        playerNameText.gameObject.SetActive(true);
        audioMsg.gameObject.SetActive(true);
        textMsg.gameObject.SetActive(false);
        audioMsgText.text = string.Format("语言 {0}秒 {1}", time, audioSamples.Length.Byte2Str());
        playerNameText.text = role.name + " (" + role.uid + ")";
    }

    public void PlayAudio()
    {
        if(audioSamples != null)
        {
            VoipAudio.Install.Add(audioSamples);
        }
    }


    public float Y
    {
        get
        {
            return ((RectTransform)transform).anchoredPosition.y;
        }

        set
        {
            ((RectTransform)transform).anchoredPosition = new Vector2(0, value);
        }
    }
}
