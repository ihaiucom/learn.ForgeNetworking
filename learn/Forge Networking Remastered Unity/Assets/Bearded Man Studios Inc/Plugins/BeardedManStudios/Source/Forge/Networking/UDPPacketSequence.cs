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
	public class UDPPacketSequence
	{
        /// <summary>
        /// 确定这个数据包序列是否可靠
        /// Determines if this sequence of packets are reliable
        /// </summary>
        public bool Reliable { get; private set; }

        /// <summary>
        /// 找到这个序列中最后一个数据包的id
        /// The id of the last packet in this sequence when found
        /// </summary>
        public int End { get; private set; }


		public Receivers Receivers { get; private set; }

        /// <summary>
        /// 组成这个序列的数据包列表
        /// The list of packets that make up this sequence
        /// </summary>
        Dictionary<int, UDPPacket> packets = new Dictionary<int, UDPPacket>();

        /// <summary>
        /// 用于确定这个序列是否完整
        /// Used to determine if this sequence is complete
        /// </summary>
        public bool Done { get; private set; }

        /// <summary>
        /// Adds the packet to the sequence
        /// </summary>
        /// <returns><c>true</c>, if packet copmelted the sequence, <c>false</c> otherwise.</returns>
        /// <param name="packet">The packet to be added</param>
        /// <summary>
        ///将数据包添加到序列中
        /// </ summary>
        /// <returns> <c> true </ c>，如果数据包混合了序列，否则为<c> false </ c>。</ returns>
        /// <param name =“packet”>要添加的数据包</ param>
        public bool AddPacket(UDPPacket packet)
		{
            //如果没有数据包，那么这是第一个数据包，所以我们需要
            //在这里进行与进入的数据包有关的任何初始化
            // If there are no packets then this is the first packet, so we need
            // to do any initialization here related to the packets coming in
            if (packets.Count == 0)
				Reliable = packet.reliable;

            //检查数据包是否已被读取
            // Check to see if the packet has already been read
            if (packets.ContainsKey(packet.orderId))
				return false;

            //检查这是否是列表中的最后一个数据包，如果存储的话
            //当其他数据包进入时，我们可以确定何时完成
            // Check to see if this is the last packet in the list, if so store it
            // so that when other packets come in we can determine when to complete
            if (packet.endPacket)
			{
				End = packet.orderId + 1;
				Receivers = packet.receivers;
			}

			packets.Add(packet.orderId, packet);

            // 检查是否收到最终数据包
            // Check to see if the end packet was received
            if (End != 0)
			{
                // 如果这个字典与最终数据包顺序号相同，我们就完成了
                // If this dictionary has the same amount of packets as the end packet order id we are done
                if (packets.Count == End)
				{
					Done = true;
					return true;
				}
			}

            //这个数据包序列仍然不完整
            // This packet sequence is still incomplete
            return false;
		}

        /// <summary>
        /// Collect all of the data from all of the packets in this sequence and return it
        /// </summary>
        /// <returns>The complete packet sequence data</returns>
        /// <summary>
        ///收集所有数据包中的所有数据，然后返回
        /// </ summary>
        /// <returns>完整的数据包序列数据</ returns>
        public BMSByte GetData(NetWorker networker)
		{
			networker.PacketSequenceData.Clear();

			for (int i = 0; i < End; i++)
				networker.PacketSequenceData.BlockCopy(packets[i].rawBytes, 0, packets[i].rawBytes.Length);

			return networker.PacketSequenceData;
		}
	}
}