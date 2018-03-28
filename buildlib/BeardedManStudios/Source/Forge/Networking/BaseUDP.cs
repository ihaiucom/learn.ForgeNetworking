/*-----------------------------+-------------------------------\
|                                                              |
|                         !!!NOTICE!!!                         |
|                                                              |
|  These libraries are under heavy development so they are     |
|  subject to make many changes as development continues.      |
|  For this reason, the libraries may not be well commented.   |
|  THANK YOU for supporting forge with all your feedback       |
|  suggestions, bug reports and comments!                      |
|                                                              |
|                              - The Forge Team                |
|                                Bearded Man Studios, Inc.     |
|                                                              |
|  This source code, project files, and associated files are   |
|  copyrighted by Bearded Man Studios, Inc. (2012-2017) and    |
|  may not be redistributed without written permission.        |
|                                                              |
\------------------------------+------------------------------*/

using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;

namespace BeardedManStudios.Forge.Networking
{
	public abstract class BaseUDP : NetWorker
	{
        // 接收完消息的 完整数据包
		public delegate void PacketComplete(BMSByte data, int groupId, byte receivers, bool isReliable);
		public delegate void MessageConfirmedEvent(NetworkingPlayer player, UDPPacket packet);

        // 确认收到消息事件
        public event MessageConfirmedEvent messageConfirmed;

		public CachedUdpClient Client { get; protected set; }

		protected List<UDPPacketComposer> pendingComposers = new List<UDPPacketComposer>();

		public BaseUDP() { }
		public BaseUDP(int maxConnections) : base(maxConnections) { }

		public abstract void Send(FrameStream frame, bool reliable = false);

        /// <summary>
        /// Used to clean up the target composer from memory
        /// </summary>
        /// <param name="composer">The composer that has completed</param>
        /// <summary>
        ///用于从内存中清理目标作曲者
        /// </ summary>
        /// <param name =“composer”>完成</ param>的作曲家
        protected void ComposerCompleted(UDPPacketComposer composer)
		{
#if DEEP_LOGGING
			Logging.BMSLog.Log(string.Format("<<<<<<<<<<<<<<<<<<<<<<<<<<< CONFIRMING: {0}", composer.Frame.UniqueId));
#endif
            //Loger.Log(string.Format("<<<<<<<<<<<<<<<<<<<<<<<<<<< CONFIRMING: {0}", composer.Frame.UniqueId));

            lock (pendingComposers)
			{
				pendingComposers.Remove(composer);
			}
		}

        /// <summary>
        /// 拉取包元数据 reliable = 1 | endPacket = 2 | confirmationPacket = 4 |  4<<receivers
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
		private byte PullPacketMetadata(BMSByte packet)
		{
            //读取最后一个字节的数据
			byte meta = packet.GetBasicType<byte>(packet.Size - sizeof(byte));
			packet.SetSize(packet.Size - sizeof(byte));
			return meta;
		}

        /// <summary>
        /// 提取这个包的 排序号
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
		private int PullPacketOrderId(BMSByte packet)
		{
            //这假设数据包元数据被首先拉出
            // This assumes that packet metadata was pulled first
            int orderId = packet.GetBasicType<int>(packet.Size - sizeof(int));
			packet.SetSize(packet.Size - sizeof(int));
			return orderId;
		}

        /// <summary>
        /// 提取这个包的 组号
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
		private int PullPacketGroupId(BMSByte packet)
		{
			// This assumes that packet order id was pulled first
			int groupId = packet.GetBasicType<int>(packet.Size - sizeof(int));
			packet.SetSize(packet.Size - sizeof(int));
			return groupId;
		}

        /// <summary>
        /// 提取这个包 的编号
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="endPacket"></param>
        /// <returns></returns>
		private ulong PullPacketUniqueId(BMSByte packet, bool endPacket)
		{
			// This assumes that packet group id was pulled first
			ulong uniqueId = packet.GetBasicType<ulong>(packet.Size - sizeof(ulong));

            //不要像其他人那样设置大小，除非是最后的数据包
            //因为帧会消耗这个时间步骤帧期望
            //消息中的最后一个字节是时间步长
            // Don't set the size like in the others unless it is the end packet 
            // because the frame will consume this time step The frame expects
            // the last bytes in the message to be the time step
            if (!endPacket)
				packet.SetSize(packet.Size - sizeof(ulong));

			return uniqueId;
		}

        /// <summary>
        /// 转码 包
        /// </summary>
        /// <param name="sender">发送数据的玩家</param>
        /// <param name="packet">接收发送数据的玩家收到的数据包</param>
        /// <returns></returns>
		protected UDPPacket TranscodePacket(NetworkingPlayer sender, BMSByte packet)
		{
			byte meta = PullPacketMetadata(packet);

            //如果最后一个字节说明它是可靠的
            // If the last byte says it is reliable
            bool reliable = (0x1 & meta) != 0;

            //如果最后一个字节说明它是这个组的最后一个数据包
            // If the last byte says that it is the last packet for this group
            bool endPacket = (0x2 & meta) != 0;

            //如果最后一个字节说明它是一个确认收到包
            // If the last byte says that it is a conformation packet
            bool confirmationPacket = (0x4 & meta) != 0;

            //从前4位获取接收器
            // Get the receivers from the frist 4 bits
            Receivers receivers = (Receivers)(meta >> 4);

            //这个数据包的组和顺序被分配给数据包的尾部
            //头部保留用于框架形成
            // The group and order for this packet are assigned to the trailer of the packet, as
            // the header is reserved for frame formation
            int orderId = PullPacketOrderId(packet);
			int groupId = PullPacketGroupId(packet);

            // 唯一编号
			ulong uniqueId = PullPacketUniqueId(packet, endPacket);

            //检查这个数据包是否已经收到
            // 如果是可靠的包， 并且这个包不是确认包，就反馈已收到。将编号、组好、序号发回去
            // Check to see if this should respond to the sender that this packet has been received
            if (reliable && !confirmationPacket)
			{
#if DEEP_LOGGING
				Logging.BMSLog.Log(string.Format(">>>>>>>>>>>>>>>>>>>>>>>>>>> SEND CONFIRM: {0}", uniqueId));
#endif

				byte[] confirmation = new byte[sizeof(ulong) + sizeof(int) + sizeof(int) + sizeof(byte)];
				Buffer.BlockCopy(BitConverter.GetBytes(uniqueId), 0, confirmation, 0, sizeof(ulong));
				Buffer.BlockCopy(BitConverter.GetBytes(groupId), 0, confirmation, sizeof(ulong), sizeof(int));
				Buffer.BlockCopy(BitConverter.GetBytes(orderId), 0, confirmation, sizeof(ulong) + sizeof(int), sizeof(int));

                //在最后一个字节中注册确认标志
                // Register the confirmation flag in the last byte
                confirmation[confirmation.Length - 1] = (byte)(meta | 0x4);

				Client.Send(confirmation, confirmation.Length, sender.IPEndPointHandle);
			}

            //创建一个包结构的实例，发送给包管理器
            // Create an instance of a packet struct to be sent off to the packet manager
            UDPPacket formattedPacket = new UDPPacket(reliable, endPacket, groupId, orderId, uniqueId, packet.CompressBytes(), confirmationPacket, receivers);

			return formattedPacket;
		}

        /// <summary>
        /// 这个可以调用的messageConfirmed事件调用的包装器
        /// A wrapper for the messageConfirmed event call that children of this can call
        /// </summary>
        protected void OnMessageConfirmed(NetworkingPlayer player, UDPPacket packet)
		{
			if (messageConfirmed != null)
				messageConfirmed(player, packet);
		}
	}
}