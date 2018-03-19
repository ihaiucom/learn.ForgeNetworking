using Games.PB;
using System;
using System.Collections.Generic;
using static GameFramework.Utility;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/16/2018 4:12:00 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public class ProtoMaster
    {
        public delegate void LoginEvent();
        public delegate void LoginConnectGateEvent(string ip, uint port);
        public delegate void AuthFailEvent(AuthCode code);

        // 连接成功
        public event LoginEvent connectSucceed;

        // 验证成功
        public event LoginEvent authing;

        // 验证成功
        public event LoginEvent authSucceed;

        // 验证失败
        public event AuthFailEvent authFail;

        // 其他地方登陆了
        public event LoginEvent otherLogin;

        // 重新连接网关
        public event LoginConnectGateEvent connectGate;


        private MasterClient    client;
        private HTcpClient      tcpClient;
        private AuthObj         authObj;

        private SocketState     state;
        private bool            isShortLink;


        public ProtoMaster(MasterClient client)
        {
            this.client = client;
            this.tcpClient = client.tcpClient;
            this.authObj = client.authObj;

            Register();
            AddListener();
        }


        /// <summary>
        /// 注册
        /// </summary>
        public void Register()
        {
            Action< IProtoItem > AddItem= tcpClient.protoClient.protoListListener.AddItem;

            // ====================
            // authorization Listener
            // -- --------------------


            // 客户端登录认证结果
            AddItem(new ProtoItem<S_Ping_1>() { opcode = 1, protoStructType = typeof(S_Ping_1), protoStructName = "S_Ping_1", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端接收Ping" });


            // 客户端登录认证结果
            AddItem(new ProtoItem<S_FirstAuthorization_10002>() { opcode = 10002, protoStructType = typeof(S_FirstAuthorization_10002), protoStructName = "S_FirstAuthorization_10002", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端登录认证结果" });

            // 通知客户端连接新网关
            AddItem(new ProtoItem<S_ConnectToNewGateway_10003>() { opcode = 10003, protoStructType = typeof(S_ConnectToNewGateway_10003),  protoStructName = "S_ConnectToNewGateway_10003",  protoFilename = "authorization", opcodeMapping = new int[] { }, note = "通知客户端连接新网关" });

            // 通知客户端账号重复登录
            AddItem(new ProtoItem<S_AccountDuplicateLogin_10004>() { opcode = 10004, protoStructType = typeof(S_AccountDuplicateLogin_10004),  protoStructName = "S_AccountDuplicateLogin_10004",  protoFilename = "authorization", opcodeMapping = new int[] { }, note = "通知客户端账号重复登录" });

            // 同步服务器时间(utc)
            AddItem(new ProtoItem<S_SyncServerTimestamp_15001>() { opcode = 15001, protoStructType = typeof(S_SyncServerTimestamp_15001),  protoStructName = "S_SyncServerTimestamp_15001", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "同步服务器时间(utc)" });

            // 同步随机因子
            AddItem(new ProtoItem<S_SyncRandomFactor_15002>() { opcode = 15002, protoStructType = typeof(S_SyncRandomFactor_15002),  protoStructName = "S_SyncRandomFactor_15002",  protoFilename = "authorization", opcodeMapping = new int[] { }, note = "同步随机因子" });





            AddItem = tcpClient.protoClient.protoListSender.AddItem;

            // ====================
            // authorization Sender
            // -- --------------------

            // Ping
            AddItem(new ProtoItem<C_Ping_1>() { opcode = 1, protoStructType = typeof(C_Ping_1), protoStructName = "C_Ping_1", protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端发送Ping" });

            // 客户端首次登录认证
            AddItem(new ProtoItem<C_FirstAuthorization_10000>() { opcode = 10000, protoStructType = typeof(C_FirstAuthorization_10000),  protoStructName = "C_FirstAuthorization_10000",  protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端首次登录认证" });

            // 客户端断线重连认证
            AddItem(new ProtoItem<C_ReconnectOnLossandAuthorization_10001>() { opcode = 10001, protoStructType = typeof(C_ReconnectOnLossandAuthorization_10001),  protoStructName = "C_ReconnectOnLossandAuthorization_10001",  protoFilename = "authorization", opcodeMapping = new int[] { }, note = "客户端断线重连认证" });


        }



        /** 添加监听 */
        public void AddCallback<T>(Action<T> callback) where T : new()
        {
            tcpClient.protoClient.AddCallback<T>(callback);
        }

        /** 发送消息 */
        public void SendMessage<T>(T msg)
        {
            tcpClient.protoClient.SendMessage<T>(msg);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        private void AddListener()
        {
            /**  服务器：Ping */
            AddCallback<S_Ping_1>(S_Ping_1);

            /** 服务器：响应客户端登录验证结果 */
            AddCallback<S_FirstAuthorization_10002>(S_FirstAuthorization_10002);


            /** 服务器：通知客户端重新连接新网关 */
            AddCallback<S_ConnectToNewGateway_10003>(S_ConnectToNewGateway_10003);


            /** 服务器：通知客户端账号在别处登录 */
            AddCallback<S_AccountDuplicateLogin_10004>(S_AccountDuplicateLogin_10004);


            /** 服务器：同步随机因子到客户端 */
            AddCallback<S_SyncRandomFactor_15002>(S_SyncRandomFactor_15002);

            /** 服务器：同步服务器时间到客户端 */
            AddCallback<S_SyncServerTimestamp_15001>(S_SyncServerTimestamp_15001);
        }


        /** 服务器：Ping */
        public void C_Ping_1()
        {
            C_Ping_1 msg = new C_Ping_1();
            SendMessage<C_Ping_1>(msg);
        }

        /** 服务器：Ping */
        private void S_Ping_1(S_Ping_1 msg)
        {
            Loger.LogTagFormat("ProtoLogin", "S_Ping_1");
        }


        /** 服务器：响应客户端登录验证结果 */
        private void S_FirstAuthorization_10002(S_FirstAuthorization_10002 msg)
        {

            AuthCode code = (AuthCode) msg.error_code;
            Loger.LogTagFormat("ProtoLogin", "S_FirstAuthorization_10002 服务器：响应客户端登录验证结果 error_code={0}" , code);
            if (code == AuthCode.enum_auth_succeed)
            {
                /** 认证成功 */
                string auth_string = msg.authorization_str;
                if (authObj.first_auth_string == "")
                {
                    authObj.first_auth_string = auth_string;
                }

                authObj.prev_auth_string = auth_string;

                /**
				 * 注：短链接成功包含短链接认证成功、短链接准备完毕两个阶段，
				 * 两个阶段完毕表示短链接认证成功
				 * */
                if (!isShortLink)
                {
                    /** 首次认证成功 */
                }

                if (authSucceed != null) authSucceed();
                state = SocketState.Authsucceed;
            }
            else
            {
                /** 认证失败 */
                if (authFail != null) authFail(code);
            }
            
        }


        /** 服务器：通知客户端重新连接新网关 */
        private void S_ConnectToNewGateway_10003(S_ConnectToNewGateway_10003 msg)
        {

            Loger.LogTagFormat("ProtoLogin", "S_ConnectToNewGateway_10003 服务器：通知客户端重新连接新网关 ip={0}, port={1}", msg.ip, msg.port);
            if (connectGate != null)
                connectGate(msg.ip, msg.port);
        }


        /** 服务器：通知客户端账号在别处登录 */
        private void S_AccountDuplicateLogin_10004(S_AccountDuplicateLogin_10004 msg)
        {
            Loger.LogTagFormat("ProtoLogin", "S_AccountDuplicateLogin_10004 服务器：通知客户端账号在别处登录");

            if (otherLogin != null) otherLogin();
        }


        /** 服务器：同步随机因子到客户端 */
        private void S_SyncRandomFactor_15002(S_SyncRandomFactor_15002 msg)
        {
            Loger.LogTagFormat("ProtoLogin", "S_SyncRandomFactor_15002 服务器：同步随机因子到客户端 msg.random_factor={0}", msg.random_factor);


            // 变更状态为连接上
            state = SocketState.Connected;

            // 发送连接成功事件
            if (connectSucceed != null) connectSucceed();


            // 记录服务器随机因子
            authObj.prev_server_S = authObj.now_server_S;
            authObj.now_server_S = msg.random_factor;


            // 状态变为认证中
            state = SocketState.Authing;

            bool needShortLink = (authObj.first_auth_string != "");
            if (needShortLink)
            {
                /** 请求短链接认证 */
                C_ReconnectOnLossandAuthorization_10001();
            }
            else
            {
                /* 请求首次认证 */
                C_FirstAuthorization_10000();
            }

            if (authing != null) authing();
        }



        /** 服务器：同步服务器时间到客户端 */
        private void S_SyncServerTimestamp_15001(S_SyncServerTimestamp_15001 msg)
        {
            Loger.LogTagFormat("ProtoLogin", "S_SyncServerTimestamp_15001 服务器：同步服务器时间到客户端 timestamp={0}", (Int64)msg.timestamp);
        }


        // 请求进行首次认证
        private void C_FirstAuthorization_10000()
        {
            /** 计算session */
            string session = Verifier.Md5Sum(authObj.from_web_M + authObj.now_server_S);


            C_FirstAuthorization_10000 msg = new C_FirstAuthorization_10000();
            msg.account_id = client.parameter.accountId;
            msg.account_name = client.parameter.username;
            msg.token = session;
            msg.version = "phone version";
            msg.channel = "default_self";


            Loger.LogTagFormat("ProtoLogin", "C_FirstAuthorization_10000 请求进行首次认证 session={0}", session);

            isShortLink = false;
            SendMessage<C_FirstAuthorization_10000>(msg);

        }

        // 请求进行短链接认证
        private void C_ReconnectOnLossandAuthorization_10001()
        {
            C_ReconnectOnLossandAuthorization_10001 msg = new C_ReconnectOnLossandAuthorization_10001();

            Loger.LogTagFormat("ProtoLogin", "C_ReconnectOnLossandAuthorization_10001 请求进行短链接认证");
            isShortLink = true;
            SendMessage<C_ReconnectOnLossandAuthorization_10001>(msg);

        }


    }

    public enum SocketState
    {
        // 正在连接
        Connecting,

        // 连接成功
        Connected,

        // 正在认证
        Authing,

        // 认证成功
        Authsucceed,

    }

    public enum AuthCode
    {
        /// 认证成功
        enum_auth_succeed = 0,
        /// 认证失败
        enum_auth_failed = 1,
        /// 版本不匹配
        enum_auth_version_not_eq = 2,
        /// 系统繁忙
        enum_auth_system_busy = 3,
        /// 请重新进行登录流程(短链接认证无法进行)
        enum_auth_need_restart_login_process = 4,
        /// 渠道错误(短链接认证无法进行)
        enum_auth_channel_error = 5,
    }

    public class AuthObj
    {

        /** 客户端发往web的随机因子 */
        public string to_web_random_string = "";

        /** 登陆web成功后，web回馈的值M */
        public string from_web_M = "";

        /** 首次认证字符串 */
        public string first_auth_string = "";

        /** 上次认证字符串 */
        public string prev_auth_string = "";

        /** 上次服务器随机因子S */
        public string prev_server_S = "";

        /** 当前服务器随机因子S */
        public string now_server_S = "";


        public void reset()
        {
            to_web_random_string = "";
            from_web_M = "";
            first_auth_string = "";
            prev_auth_string = "";
            prev_server_S = "";
            now_server_S = "";
        }
    }
}
