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

using System.Collections.Generic;

namespace BeardedManStudios.Forge.Networking
{
	public class UDPPacketGroup
	{
        // 完整数据包，缓存时间 30秒
		private const int MAX_ACCEPT_TIME_WINDOW = 30000;

        /// <summary>
        /// 按时间戳索引的序列列表
        /// 当数据包接收完整后，会缓存时间MAX_ACCEPT_TIME_WINDOW 30秒
        /// key = UDPPacket.uniqueId
        /// The list of sequences which are indexed by their timestamp
        /// </summary>
        private Dictionary<ulong, UDPPacketSequence> packets = new Dictionary<ulong, UDPPacketSequence>();

        // Id跟踪器
        private struct IdTracker
		{
            // 接收完 完整包的时间
			public ulong storeTime;
            // 数据包都ID UDPPacket.uniqueId
            public ulong id;
		}

        /// <summary>
        /// 这是正在等待删除的包ID的列表
        /// This is a list of packet ids that are pending removal
        /// </summary>
        private List<IdTracker> trackers = new List<IdTracker>();

		private TimeManager time = new TimeManager();

        /// <summary>
        /// GroupId = UDPPacket.groupId
        /// </summary>
		public int GroupId { get; private set; }
		public UDPPacketGroup(int groupId)
		{
			GroupId = groupId;
		}

        /// <summary>
        /// Add a packet to a sequence based on the server timestamp
        /// </summary>
        /// <param name="packet">The packet to be added</param>
        /// <param name="packetCompleteHandle">The method to call and pass the data to when a sequence is complete</param>
        /// <summary>
        ///根据服务器时间戳添加一个数据包到一个序列
        /// </ summary>
        /// <param name =“packet”>要添加的数据包</ param>
        /// <param name =“packetCompleteHandle”>在序列完成时调用和传递数据的方法</ param>
        public void AddPacket(UDPPacket packet, BaseUDP.PacketComplete packetCompleteHandle, NetWorker networker)
		{
            //不要处理具有指定范围内时间步长的数据包
            // Don't process packets that have a timestep within a specified range
            //if (Time.Milliseconds - packet.timeStep > MAX_ACCEPT_TIME_WINDOW)
            //{
            // TODO:  Send an event for old message received or packet rejected
            //	return;
            //}

            //从这个查询中删除数据包是在一个单独的线程上完成的
            // Removal of packets from this lookup is done on a separate thread
            lock (packets)
			{
                // 检查，看看我们是否已经开始这个序列
                // Check to see if we have already started this sequence
                if (!packets.ContainsKey(packet.uniqueId))
					packets.Add(packet.uniqueId, new UDPPacketSequence());
			}

            //缓存序列，所以我们不要反复查找它
            // Cache the sequence so we don't repeatedly look it up
            UDPPacketSequence sequence = packets[packet.uniqueId];

            //如果序列已经完成，则不要继续添加数据包
            // Do not continue to add the packet if the sequence is already complete
            if (sequence.Done)
				return;

            // 返回true是完整的数据包
			if (sequence.AddPacket(packet))
			{
                //数据包序列完整
                // The packet sequence is complete
                CompleteSequence(packet.uniqueId, sequence, packetCompleteHandle, networker);
			}
		}

        /// <summary>
        /// Calls the supplied completion handler and passes the complete packet, then removes
        /// the sequence from the pending list 
        /// </summary>
        /// <param name="id">The timestamp for the packet to be used to lookup in packets dictionary</param>
        /// <param name="sequence">The actual sequence reference to skip another lookup</param>
        /// <param name="packetCompleteHandle">The method to call and pass this sequence into</param>
        /// <summary>
        ///调用提供的完成处理程序并传递完整的数据包，然后删除
        ///待处理列表中的序列
        /// </ summary>
        /// <param name =“id”>数据包用于在数据包字典中查找的时间戳</ param>
        /// <param name =“sequence”>跳过另一个查找的实际序列引用</ param>
        /// <param name =“packetCompleteHandle”>调用该方法并将其传递给</ param>的方法
        private void CompleteSequence(ulong id, UDPPacketSequence sequence, BaseUDP.PacketComplete packetCompleteHandle, NetWorker networker)
		{
			packetCompleteHandle(sequence.GetData(networker), GroupId, (byte)sequence.Receivers, sequence.Reliable);

			lock (packets)
			{
				trackers.Add(new IdTracker() { storeTime = time.Timestep, id = id });
			}

			lock (packets)
			{
				for (int i = 0; i < trackers.Count; i++)
				{
					if (trackers[i].storeTime + MAX_ACCEPT_TIME_WINDOW <= time.Timestep)
					{
						packets.Remove(trackers[i].id);
						trackers.RemoveAt(i--);
					}
				}
			}
		}
	}
}