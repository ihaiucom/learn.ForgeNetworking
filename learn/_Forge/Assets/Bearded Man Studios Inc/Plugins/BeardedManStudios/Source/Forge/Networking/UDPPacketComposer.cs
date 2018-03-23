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
using System.Linq;
using System.Threading;

namespace BeardedManStudios.Forge.Networking
{
	public class UDPPacketComposer
	{
        /// <summary>
        /// A base for any composer based events
        /// </summary>
        /// <param name="composer">The composer that fired off the event</param>
        /// <summary>
        ///任何基于作曲者的事件的基础
        /// </ summary>
        /// <param name =“composer”>发起事件的作曲家</ param>
        public delegate void ComposerEvent(UDPPacketComposer composer);

        /// <summary>
        /// 在这个作曲家完成了所有消息传递任务时发生
        /// Occurs when this composer has completed all of its messaging tasks
        /// </summary>
        public event ComposerEvent completed;

        /// <summary>
        /// 每个数据包允许的最大大小
        /// The maximum size allowed for each packet
        /// </summary>
        public const ushort PACKET_SIZE = 1200;

        /// <summary>
        /// 这个作曲家所属的客户工作者的引用
        /// A reference to the client worker that this composer belongs to
        /// </summary>
        public BaseUDP ClientWorker { get; private set; }

        /// <summary>
        /// 将会收到这个数据的目标玩家
        /// The target player in question that will be receiving this data
        /// </summary>
        public NetworkingPlayer Player { get; private set; }

        /// <summary>
        /// 要发送给用户的帧
        /// The frame that is to be sent to the user
        /// </summary>
        public FrameStream Frame { get; private set; }

        /// <summary>
        /// 如果这个消息是可靠的，以便对象知道是否需要尝试重新发送数据包
        /// If this message is reliable so that the object knows if it needs to attempt to resend packets
        /// </summary>
        public bool Reliable { get; private set; }

        /// <summary>
        /// The list of packets that are to be resent if it is reliable, otherwise it is just the
        /// list of packets that is to be sent and forgotten about
        /// </summary>
        /// <summary>
        ///如果可靠的话将被重新发送的数据包的列表，否则就是
        ///要发送和忘记的数据包列表
        /// </ summary>
        public Dictionary<int, UDPPacket> PendingPackets { get; private set; }

		public UDPPacketComposer() { }

		public UDPPacketComposer(BaseUDP clientWorker, NetworkingPlayer player, FrameStream frame, bool reliable = false)
		{
#if DEEP_LOGGING
			Logging.BMSLog.Log("---------------------------\n" + (new System.Diagnostics.StackTrace()).ToString() + "\nUNIQUE ID: " + frame.UniqueId.ToString() + "\n---------------------------");
#endif

			Init(clientWorker, player, frame, reliable);
		}

		public void Init(BaseUDP clientWorker, NetworkingPlayer player, FrameStream frame, bool reliable = false)
		{
			ClientWorker = clientWorker;
			Player = player;
			Frame = frame;
			Reliable = reliable;

			Initialize();
		}

        /// <summary>
        /// Send the packet off to the recipient
        /// </summary>
        /// <param name="data">The packet data that is to be sent</param>
        /// <summary>
        ///将数据包发送给收件人
        /// </ summary>
        /// <param name =“data”>要发送的数据包</ param>
        private void Send(byte[] data)
		{
			ClientWorker.Client.Send(data, data.Length, Player.IPEndPointHandle);
		}

		private void Initialize()
		{
			CreatePackets();

            //如果这是一个可靠的消息，那么我们需要确保尝试并重新发送消息
            //以给定的时间间隔，稍后可以在玩家最后一次ping +时间缓冲区发送
            // If this is a reliable message then we need to make sure to try and resend the message
            // at a given interval, later on this could be sent at the players last ping + time buffer
            if (Reliable)
			{
                // 确保注册，这个作曲家是听完整的数据包知道
                //当每个数据包已经被接收者确认时
                // Make sure to register that this composer is to listen for completed packets to know
                // when each of the packets have been confirmed by the recipient
                ClientWorker.messageConfirmed += MessageConfirmed;

				Player.QueueComposer(this);
			}
			else
			{
                // TODO：可能应该从主线程中运行
                //查看所有创建的数据包，并立即发送出去
                // TODO:  Probably should run this off the main thread
                // Go through all of the packets that were created and send them out immediately
                foreach (KeyValuePair<int, UDPPacket> kv in PendingPackets)
				{
					Send(kv.Value.rawBytes);

					ClientWorker.BandwidthOut += (ulong)kv.Value.rawBytes.Length;

                     // 传播数据包1毫秒，以防止任何可能发生的破坏
                    //在套接字层发送太多的数据
                    // Spread the packets apart by 1 ms to prevent any clobbering that may happen
                    // on the socket layer for sending too much data
                    Thread.Sleep(1);
				}

				Cleanup();
			}
		}

        /// <summary>
        /// 清理线程，挂起的数据包，并触发任何完成事件
        /// Cleans up the thread, pending packets, and fires off any completion events
        /// </summary>
        private void Cleanup()
		{
			lock (PendingPackets)
			{
				PendingPackets.Clear();
			}

			if (completed != null)
				completed(this);
		}

        /// <summary>
        /// 遍历所有数据，并根据PACKET_SIZE将其编译为独立的数据包
        /// Go through all of the data and compile it into separated packets based on the PACKET_SIZE
        /// </summary>
        private void CreatePackets()
		{
			PendingPackets = new Dictionary<int, UDPPacket>();

            //获取可用于该帧的所有数据
            // Get all of the data that is available for this frame
            byte[] data = Frame.GetData(Reliable, Player);

			int byteIndex = 0, orderId = 0;

            // int groupId 
            // int orderId
            // trailer[8] = Reliable (0x1) | endPacket(0x2) | Receivers <<4
            byte[] trailer = new byte[9];


			Buffer.BlockCopy(BitConverter.GetBytes(Frame.GroupId), 0, trailer, 0, sizeof(int));

			if (Reliable)
				trailer[trailer.Length - 1] |= 0x1;

			do
			{
				int remainingPacketSize = data.Length - byteIndex + trailer.Length;
				bool endPacket = remainingPacketSize <= PACKET_SIZE;
				int length = 0;

                //如果不是结束的话，我们需要把这个时间步添加到这个包中
                // We need to add the time step to this packet if it is not the end
                if (!endPacket)
				{
                    //我们需要回溯添加的时间戳的长度
                    // We need to backtrack the length of the added timestamp
                    length -= sizeof(ulong);
					remainingPacketSize += -length;
				}

                //在内存中创建数据包空间并将其分配给正确的长度
                // dataPartEnd, trailer
                // dataPart,UniqueId, trailer
                // Create the packet space in memory and assign it to the correct length
                byte[] packet = new byte[Math.Min(PACKET_SIZE, remainingPacketSize)];

				length += packet.Length - trailer.Length;

                //将来自源的字节复制到新的数据包中
                // Copy the bytes from the source into the new packet
                Buffer.BlockCopy(data, byteIndex, packet, 0, length);

                 // 确保我们计数每个字节，所以我们正确地结束循环，所以我们知道
                 //如果这是序列中的最后一个数据包
                 // Make sure we count every byte so we end the loop correctly and also so we know
                 // if this is the last packet in the sequence
                 byteIndex += length;

				if (endPacket)
				{
					trailer[trailer.Length - 1] |= 0x2;

                    //将接收器添加到结束标头字节
                    // Add the receivers to the end header byte
                    trailer[trailer.Length - 1] |= (byte)(((int)Frame.Receivers) << 4);
				}
				else
                    //我们需要将唯一的ID复制到这个消息中
                    // We need to copy the unique id into this message
                    Buffer.BlockCopy(BitConverter.GetBytes(Frame.UniqueId), 0, packet, length, sizeof(ulong));

                //在预告片中设置这个数据包的顺序号
                // Set the order id for this packet in the trailer
                Buffer.BlockCopy(BitConverter.GetBytes(orderId), 0, trailer, sizeof(int), sizeof(int));

                //将预告片复制到数据包的末尾
                // Copy the trailer to the end of the packet
                Buffer.BlockCopy(trailer, 0, packet, packet.Length - trailer.Length, trailer.Length);

                //创建新的数据包并将其添加到挂起的数据包中，以便将其发送出去
                // Create and add the new packet to pending packets so that it can be sent out
                PendingPackets.Add(orderId, new UDPPacket(Reliable, endPacket, Frame.GroupId, orderId, Frame.UniqueId, packet, false, Frame.Receivers));
				orderId++;
			} while (byteIndex < data.Length);
		}

        /// <summary>
        /// 遍历所有未决的数据包并重新发送
        /// Go through all of the pending packets and resend them
        /// </summary>
        public void ResendPackets(ulong timestep, ref int counter)
		{
			lock (PendingPackets)
			{
				foreach (var key in PendingPackets.Keys.ToArray())
				{
					if (PendingPackets[key].LastSentTimestep + (ulong)Player.RoundTripLatency > timestep)
						continue;

					if (counter <= 0)
					{
						PendingPackets[key] = PendingPackets[key].UpdateTimestep(timestep);
						continue;
					}

					counter -= PendingPackets[key].rawBytes.Length;

					PendingPackets[key] = PendingPackets[key].DoingRetry(timestep);
					Send(PendingPackets[key].rawBytes);
					ClientWorker.BandwidthOut += (ulong)PendingPackets[key].rawBytes.Length;
				}
			}
		}

        /// <summary>
        /// This method is called when a packet is received and is a confirmation packet
        /// </summary>
        /// <param name="packet">The packet that was received</param>
        /// <summary>
        ///接收数据包时调用此方法，并且是确认数据包
        /// </ summary>
        /// <param name =“packet”>收到的数据包</ param>
        private void MessageConfirmed(NetworkingPlayer player, UDPPacket packet)
		{
            //检查以确保这个数据包是从这个组发送的
            // Check to make sure that this packet was sent from this group
            if (packet.groupId != Frame.GroupId)
				return;

            //检查以确保数据包是从这个作曲家发送的
            // Check to make sure that the packet was sent from this composer
            if (packet.uniqueId != Frame.UniqueId)
				return;

			if (player != Player)
				return;

			lock (PendingPackets)
			{
				UDPPacket foundPacket;

                //检查我们是否已经收到了这个包的确认
                // Check to see if we already received a confirmation for this packet
                if (!PendingPackets.TryGetValue(packet.orderId, out foundPacket))
					return;

                // 往返延迟
                player.RoundTripLatency = (int)(player.Networker.Time.Timestep - foundPacket.LastSentTimestep);

				// Remove the packet from pending so that it isn't sent again
				PendingPackets.Remove(packet.orderId);

				// All of the messages have been successfully confirmed, so we can remove the event listener
				if (PendingPackets.Count == 0)
				{
					ClientWorker.messageConfirmed -= MessageConfirmed;

					Cleanup();
					Player.EnqueueComposerToRemove(packet.uniqueId);
				}
			}
		}
	}
}