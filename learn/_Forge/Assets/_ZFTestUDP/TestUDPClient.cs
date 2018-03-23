using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUDPClient : MonoBehaviour
{
    public UnityEngine.UI.Text msgText;
    public RectTransform msgContent;
    public UnityEngine.UI.ScrollRect msgScroll;
    private float msgContentDefaultHeight = 300;
    public UnityEngine.UI.InputField inputField;

    public string host = "127.0.0.1";
    public ushort port = 13300;

    public UDPClient client;
    void Start ()
    {
        msgContentDefaultHeight = ((RectTransform)msgContent.parent).sizeDelta.y;

        MainThreadManager.Create();
        client = new UDPClient();
        client.Connect(host, port);
        client.textMessageReceived += TextMessageReceived;
        client.binaryMessageReceived += BinaryMessageReceived;
    }
	
	void Update ()
    {
		
	}


    int line = 1;
    // 接收消息
    private void TextMessageReceived(NetworkingPlayer player, Text frame, NetWorker sender)
    {

        MainThreadManager.Run(() =>
        {
            string msg = player.NetworkId + " " + player.Name + " " + frame.ToString();
            Loger.Log(msg);
            msgText.text += msg + "\n";
            line++;
            float height = line * 50;
            if (height < msgContentDefaultHeight)
            {
                height = msgContentDefaultHeight;
            }
            msgContent.sizeDelta = msgText.rectTransform.sizeDelta = new Vector2(msgText.rectTransform.sizeDelta.x, height);
            msgScroll.verticalNormalizedPosition = 1;
        });

       
    }


    private void BinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
    {

        MainThreadManager.Run(() =>
        {
            string identifier = frame.StreamData.GetBasicType<string>();
            string content = frame.StreamData.GetBasicType<string>();
            string msg = player.NetworkId + " " + player.Name + " " + identifier + " " + content;
            Loger.Log(msg);
            msgText.text += msg + "\n";
            line++;
            float height = line * 50;
            if (height < msgContentDefaultHeight)
            {
                height = msgContentDefaultHeight;
            }
            msgContent.sizeDelta = msgText.rectTransform.sizeDelta = new Vector2(msgText.rectTransform.sizeDelta.x, height);
            msgScroll.verticalNormalizedPosition = 1;
        });
    }

    const int GROUPID_TEXT = MessageGroupIds.START_OF_GENERIC_IDS + 1;
    const int GROUPID_BINARY = MessageGroupIds.START_OF_GENERIC_IDS + 2;
    public void SendText()
    {
        Text frame = Text.CreateFromString(client.Time.Timestep, inputField.text, false, Receivers.All, GROUPID_TEXT, false);
        client.Send(frame, true);
    }

    public void SendBinary()
    {
        object[] args = new object[] {Application.identifier, inputField.text };
        BMSByte data = ObjectMapper.BMSByte(args);
        Loger.Log(data.Size);

        Binary frame = new Binary(client.Time.Timestep, false, data, Receivers.All, GROUPID_BINARY, false);
        client.Send(frame, true);
        Loger.Log(frame.StreamData.Size);
    }


    private void OnApplicationQuit()
    {
        client.Disconnect(true);
    }
}
