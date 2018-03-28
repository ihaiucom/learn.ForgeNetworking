using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VoipPanelId
{
    ServerPanel,
    RoomPanel,
    ChatPanel,
}

public class VoipClient : MonoBehaviour
{
    public LobbyClient lobby;

    [Header("Panel")]
    public GameObject settingServerPanel;
    public GameObject settingRoomPanel;
    public GameObject chatPanel;
    private Dictionary<VoipPanelId, GameObject> panelDict = new Dictionary<VoipPanelId, GameObject>();

    [Header("ServerPanel")]
    public InputField serverIPInputField;
    public InputField serverPortInputField;

    [Header("RoomPanel")]
    public Dropdown roomIdDropdown;
    public InputField roleIdInputField;
    public InputField roleNameInputField;

    [Header("Chat Player List")]
    public ScrollRect playerListScrollRect;
    public RectTransform playerListContent;
    public GameObject playerItemPrefab;
    private List<PlayerItem> playerItemList = new List<PlayerItem>();

    [Header("Chat Msg List")]
    public ScrollRect chatScrollRect;
    public RectTransform chatContent;
    public GameObject chatItemPrefab;
    private List<ChatMsgItem> chatItemList = new List<ChatMsgItem>();


    [Header("Chat Input")]
    public InputField chatTextInputField;
    public Button chatAudioOpenButton;
    public Button chatAudioCloseButton;



    void Start ()
    {
        panelDict.Add(VoipPanelId.ServerPanel, settingServerPanel);
        panelDict.Add(VoipPanelId.RoomPanel, settingRoomPanel);
        panelDict.Add(VoipPanelId.ChatPanel, chatPanel);
        OpenPanel(VoipPanelId.ServerPanel);

        playerItemPrefab.SetActive(false);
        chatItemPrefab.SetActive(false);


        chatAudioOpenButton.gameObject.SetActive(true);
        chatAudioCloseButton.gameObject.SetActive(false);

        TimeSpan ts = DateTime.Now - new DateTime(2018, 3, 27);
        roleIdInputField.text = ((ulong)ts.TotalMilliseconds) + "";

        string[] names = new string[] { "张三", "李四", "文杰", "向东", "洪竹", "启哲", "长峰", "晨曦", "晓丹", "佳盈", "长秀", "馨", "嬉羽", "珊儿", "嘉喜", "冲", "灵霏", "耀杨", "建浩", "锦霖", "丰瑜", "鑫楠", "凌锋", "祺鑫", "馨丹", "菲柔", "铸", "国堂", "翌轩", "囡", "正耀", "钧轶", "茂祥", "令鸣", "芸莎", "德龙", "飞宇", "雪飞", "丽凤", "印", "爱飞", "艺卿", "鑫鹏", "胜雄", "世荣", "菲", "蕾", "美越", "华森", "梦翔", "洪林", "甜甜", "骐", "彩云", "思润", "雅琪", "均益", "可可", "斌", "灏", "季薇" };
        roleNameInputField.text = names[UnityEngine.Random.Range(0, names.Length)];
    }
	
	void Update ()
    {
		
	}

    public void OpenPanel(VoipPanelId panelId)
    {
        UnityMainThread.Run(() =>
        {
            foreach (var kvp in panelDict)
            {
                kvp.Value.SetActive(false);
            }
            panelDict[panelId].SetActive(true);
        });
        
    }

    public void Connect()
    {
        
        string ip = serverIPInputField.text;
        ushort port =(ushort) Convert.ToUInt32(serverPortInputField.text);

        lobby = new LobbyClient();
        lobby.Socket.serverAccepted += OnServerAccepted;
        lobby.Socket.disconnected += OnDisconnected;
        lobby.playerJoinRoom += OnJoinRoom;
        lobby.playerLeftRoom += OnLeftRoom;
        lobby.Connect(ip, port);

    }

    public void CreateAndJoinRoom()
    {
        lobby.roleInfo = new NetRoleInfo();
        lobby.roomInfo = new NetRoomInfo();

        lobby.roomInfo.roomUid = Convert.ToUInt64(roomIdDropdown.itemText);

        lobby.roleInfo.uid = Convert.ToUInt64(roleIdInputField.text);
        lobby.roleInfo.name = roleNameInputField.text;

        lobby.CreateAndJoinRoom();
    }


    // 服务器接收连接了
    private void OnServerAccepted(NetWorker sender)
    {
        Loger.LogFormat("LobbyClient OnServerAccepted {0}", sender);

        OpenPanel(VoipPanelId.RoomPanel);
    }

    // 服务器断开连接
    private void OnDisconnected(NetWorker sender)
    {
        Loger.LogFormat("LobbyClient OnDisconnected {0}", sender);

        OpenPanel(VoipPanelId.ServerPanel);
    }

    // 加入到房间
    private void OnJoinRoom(ulong roomUid, NetworkingPlayer player, NetJoinRoomResult ret)
    {
        Loger.LogFormat("LobbyClient OnPlayerJoinRoom {0}", roomUid);

        lobby.room.playerJoinRoom += OnPlayerJoinRoom;
        lobby.room.playerLeftRoom += OnPlayerLeftRoom;
        lobby.room.playerListEvent += OnPlayerList;
        lobby.room.binaryMessageReceived += OnBinaryMessageReceived;
        OpenPanel(VoipPanelId.ChatPanel);
        lobby.GetRoomPlayerList();

        UnityMainThread.Run(()=> {
            VoipAudio.Install.StartVOIP(lobby);
            RefreshPlayerList();
        });
    }

    // 离开房间
    private void OnLeftRoom(ulong roomUid, ulong roleUid, NetworkingPlayer player, NetLeftRoomResult ret)
    {
        if(ret != NetLeftRoomResult.Failed_NoRoom)
            OpenPanel(VoipPanelId.RoomPanel);
    }

    // 玩家离开房间
    private void OnPlayerLeftRoom(ulong roleUid, NetworkingPlayer player)
    {

        UnityMainThread.Run(() =>
        {
            AddSystemMsg(string.Format("{0} 离开房间", roleUid));
            RefreshPlayerList();
        });
    }


    // 玩家加入到房间
    private void OnPlayerJoinRoom(IRoleInfo roleInfo, NetworkingPlayer player)
    {
        UnityMainThread.Run(() =>
        {
            AddSystemMsg(string.Format("{0} {1} 加入到房间", roleInfo.uid, roleInfo.name));
            RefreshPlayerList();
        });
    }

    // 获取到 玩家列表
    private void OnPlayerList(List<IRoleInfo> players)
    {
        UnityMainThread.Run(() =>
        {
            AddSystemMsg(string.Format("获取到 玩家列表 人数{0}", players.Count));
            RefreshPlayerList();
        });
    }

    /// <summary>
    /// 刷新玩家列表
    /// </summary>
    private void RefreshPlayerList()
    {
        List<IRoleInfo> PlayerList = lobby.room.PlayerList;

        int y = 0;
        int gap = 10;
        int height = 100;
        int playerItemCount = playerItemList.Count;
        for (int i = 0; i < PlayerList.Count; i ++)
        {
            PlayerItem item = null;
            if(i < playerItemCount)
            {
                item = playerItemList[i];
            }
            else
            {
                GameObject go = GameObject.Instantiate<GameObject>(playerItemPrefab);
                go.transform.SetParent(playerListContent, false);
                item = go.GetComponent<PlayerItem>();
                playerItemList.Add(item);
            }

            item.SetData(PlayerList[i]);
            item.Y = -1 * y;
            item.gameObject.SetActive(true);
            y += height + gap;
        }

        int defaultHeight = (int) ((RectTransform)playerListContent.parent).sizeDelta.y;
        y = Mathf.Max(y, defaultHeight);

        playerListContent.sizeDelta = new Vector2(playerListContent.sizeDelta.x, y);
        playerListScrollRect.normalizedPosition = new Vector2(0, 0);

        for(int i = PlayerList.Count; i < playerItemCount; i ++)
        {
            PlayerItem item = playerItemList[i];
            item.gameObject.SetActive(false);

        }
    }


    private ChatMsgItem AddMsg()
    {
        GameObject go = GameObject.Instantiate<GameObject>(chatItemPrefab);
        go.transform.SetParent(chatContent, false);
        ChatMsgItem item = go.GetComponent<ChatMsgItem>();
        item.gameObject.SetActive(true);
        chatItemList.Add(item);

        float y = chatItemList.Count * (120 + 10);
        item.Y = -1 * y;
        float defaultHeight = ((RectTransform)chatContent.parent).sizeDelta.y;
        y = Mathf.Max(y + 300, defaultHeight);


        chatContent.sizeDelta = new Vector2(chatContent.sizeDelta.x, y);
        chatScrollRect.normalizedPosition = new Vector2(0, 0);

        return item;
    }

    private void AddSystemMsg(string  msg)
    {
        ChatMsgItem item = AddMsg();
        item.SetSystemMsg(msg);
    }

    private void AddAudioMsg(IRoleInfo role, byte[] data, float time )
    {
        ChatMsgItem item = AddMsg();
        item.SetAudioMsg(role, data, time);
    }

    private void AddTextMsg(IRoleInfo role, string msg)
    {
        ChatMsgItem item = AddMsg();
        item.SetTextMsg(role, msg);
    }


    private void OnBinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
    {
        Loger.Log("OnBinaryMessageReceived");
        if (frame.GroupId != MessageGroupIds.VOIP)
            return;
        UnityMainThread.Run(()=> {
            ulong roleId = frame.StreamData.GetBasicType<ulong>();
            IRoleInfo role = lobby.room.GetPlayer(roleId);
            switch (frame.RouterId)
            {
                case VoipRoute.Audio:
                    float time = frame.StreamData.GetBasicType<float>();
                    byte[] bytes = ObjectMapper.Instance.Map<byte[]>(frame.StreamData);
                    AddAudioMsg(role, bytes, time);
                    break;

                case VoipRoute.Text:
                    string msg = frame.StreamData.GetBasicType<string>();
                    AddTextMsg(role, msg);
                    break;
            }
        });

        
    }

    BMSByte writeBuffer = new BMSByte();
    public void SetText()
    {
        writeBuffer.Clear();
        ObjectMapper.Instance.MapBytes(writeBuffer, lobby.roleInfo.uid);
        ObjectMapper.Instance.MapBytes(writeBuffer, chatTextInputField.text);

        Binary voice = new Binary(lobby.Socket.Time.Timestep, false, writeBuffer, Receivers.All, MessageGroupIds.VOIP, false, VoipRoute.Text, lobby.roomInfo.roomUid);

        lobby.Send(voice, true);
        chatTextInputField.text = "";
    }

    public void BeginMicrophoneing()
    {
        AddSystemMsg("已开启语音");

        chatAudioOpenButton.gameObject.SetActive(false);
        chatAudioCloseButton.gameObject.SetActive(true);
        VoipAudio.Install.BeginMicrophoneing();
    }

    public void EndMicrophoneing()
    {
        AddSystemMsg("已关闭语音");
        chatAudioOpenButton.gameObject.SetActive(true);
        chatAudioCloseButton.gameObject.SetActive(false);
        VoipAudio.Install.EndMicrophoneing();
    }


    public void CloseChat()
    {
        lobby.LeftRoom();
    }






    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        VoipAudio.StopVOIP();
        if(lobby != null)
            lobby.Dispose();
        lobby = null;
    }

}
