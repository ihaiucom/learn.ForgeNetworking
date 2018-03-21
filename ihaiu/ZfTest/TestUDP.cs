using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/23/2018 5:41:20 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class UnitBehaviour
    {

    }

    public class UnitClipBehaviour
    {

    }

    /// <summary>
    /// 单位实体代理
    /// </summary>
    public partial class UnitAgent
    {

        public void Init()
        {
        }
    }


    /// <summary>
    /// 技能树
    /// </summary>
    public class SkillTree
    {

    }

    public class Node
    {
        public void Enter()
        {
            // 客户端挂起
        }

        public void EnterServer()
        {
            // 服务器等待其他客户端，进入该阶段
            // 如果都进入该阶段，服务器将计算结果通知所有客户端，并执行EnterExe()
            
        }

        public void EnterExe(object result)
        {
            // 真正进入执行逻辑
        }

        public void Loop()
        {

        }

        public void End()
        {

        }
    }



    /// <summary>
    /// 技能实体代理
    /// </summary>
    public class SkillAgent
    {
        
        /// <summary>
        /// 施法者单位
        /// </summary>
        public UnitAgent casterUnit { get; set; }

        /// <summary>
        /// 技能树
        /// </summary>
        public SkillTree skillTree { get; set; }

        public SkillAgent(SkillTree skillTree, UnitAgent casterUnit)
        {
            this.casterUnit = casterUnit;
            this.skillTree = skillTree;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// 打断
        /// </summary>
        public void Kill()
        {

        }



        /// <summary>
        /// 结束
        /// </summary>
        protected void OnEnd()
        {

        }
    }

    public class TestUDP
    {
        private const int PING_INTERVAL = 10000;
        private UDPServer server;
        public bool IsRunning { get; private set; }

        public  TestUDP(string host, ushort port)
        {
            int maxConnections = 100;
            server = new UDPServer(maxConnections);
            server.Connect(host, port);

            server.textMessageReceived += TextMessageReceived;
            server.binaryMessageReceived += BinaryMessageReceived;
            server.playerConnected += PlayerConnected;
            server.playerAccepted += PlayerAccepted;
            server.playerDisconnected += PlayerDisconnected;


            IsRunning = true;
            // 停服
            server.disconnected += (sender) =>
            {
                IsRunning = false;
            };

            server.playerTimeout += (player, sender) =>
            {
                Loger.Log("Player " + player.NetworkId + " timed out");
            };


            Task.Queue(() =>
            {
                while (server.IsBound)
                {
                    Thread.Sleep(PING_INTERVAL);
                }
            }, PING_INTERVAL);
        }


        public void Dispose()
        {
            server.Disconnect(true);
            IsRunning = false;
        }


        private void PlayerConnected(NetworkingPlayer player, NetWorker sender)
        {
            Loger.Log("PlayerConnected" + " " + player.NetworkId + " " + player.Name);
        }


        private void PlayerAccepted(NetworkingPlayer player, NetWorker sender)
        {
            Loger.Log("PlayerAccepted" + " " + player.NetworkId + " " + player.Name);
        }

        

        private void PlayerDisconnected(NetworkingPlayer player, NetWorker sender)
        {
            Loger.Log("PlayerDisconnected" + " " + player.NetworkId + " " + player.Name);
        }

        const int GROUPID_TEXT = MessageGroupIds.START_OF_GENERIC_IDS + 1;
        const int GROUPID_BINARY = MessageGroupIds.START_OF_GENERIC_IDS + 2;

        // 接收消息
        private void TextMessageReceived(NetworkingPlayer player, Text frame, NetWorker sender)
        {
            string message = "Receivers:" + frame.Receivers.ToString() + " RouterId:" + frame.RouterId + " UniqueId:" + frame.UniqueId + " UniqueReliableId:" + frame.UniqueReliableId + " GroupId:" + frame.GroupId + " NetworkId:" +  player.NetworkId + " " + player.Name + " " + frame.ToString();
            Loger.Log(message);



            Text temp = Text.CreateFromString(server.Time.Timestep, message, false, Receivers.AllBuffered, GROUPID_TEXT, false);

            server.Send(temp, true);
        }

        private void BinaryMessageReceived(NetworkingPlayer player, Binary frame, NetWorker sender)
        {
            Loger.Log(frame.StreamData.Size + " " + frame.StreamData.StartPointer);
            string identifier = frame.StreamData.GetBasicType<string>();
            string content = frame.StreamData.GetBasicType<string>();
            string message =   "NetworkId:" + player.NetworkId + " " + player.Name + " " + identifier + " " + content;
            Loger.Log(message);


            object[] args = new object[] { identifier, content };
            BMSByte data = ObjectMapper.BMSByte(args);

            Binary subFrame = new Binary(server.Time.Timestep, false, data, Receivers.All, GROUPID_BINARY, false);


            server.Send(subFrame, true);
        }
    }
}
