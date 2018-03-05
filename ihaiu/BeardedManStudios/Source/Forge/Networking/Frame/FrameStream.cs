﻿/*-----------------------------+-------------------------------\
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

using System;

namespace BeardedManStudios.Forge.Networking.Frame
{
    /// <summary>
    /// 帧数据流
    /// </summary>
	public abstract class FrameStream : ICloneable
	{
        /// <summary>
        /// 获取这个帧的控制字节
        /// Get the control byte for this frame
        /// </summary>
        public abstract byte ControlByte { get; }

        /// <summary>
        /// 目前的原始数据
        /// The current raw data for this
        /// </summary>
        public BMSByte StreamData { get; protected set; }

		/// <summary>
		/// The mask that is used to mask this frame
		/// </summary>
		protected byte[] mask = new byte[0];

		/// <summary>
		/// Used to track where the payload starts in this frame
		/// </summary>
		private int payloadStart = 0;

        /// <summary>
        /// 房间ID
        /// </summary>
        public ulong RoomId { get; protected set; }

        /// <summary>
        /// 这是毫秒内这个帧的时间步长
        /// This is the time step for this frame in milliseconds
        /// </summary>
        public ulong TimeStep { get; protected set; }

        /// <summary>
        /// 此帧的唯一ID
        /// The Unique id for this frame
        /// </summary>
        public ulong UniqueId { get; protected set; }

        /// <summary>
        /// 是否是可靠的
        /// </summary>
		public bool IsReliable { get; private set; }

        // 可靠数据 克隆
		private BMSByte reliableCloneData = new BMSByte();

        /// <summary>
        /// 这个帧数据的唯一可靠的ID
        /// 用来以正确的发送顺序执行消息
        /// The unique reliable id for this frame
        /// </summary>
        public ulong UniqueReliableId { get; protected set; }

        /// <summary>
        /// 这是一个用户控制的字节，允许二进制帧由用户路由
        /// This is a user controlled byte that allows binary frames to be routed by the user
        /// </summary>
        public byte RouterId { get; protected set; }

        /// <summary>
        /// 一个id指向代码中这个消息来自哪里（唯一的）
        /// An id to point to where in the code that this message is coming from (unique)
        /// </summary>
        public int GroupId { get; private set; }

		/// <summary>
		/// The type of receivers for this frame
		/// </summary>
		public Receivers Receivers { get; private set; }

		/// <summary>
		/// The player that sent the message on the network
		/// </summary>
		public NetworkingPlayer Sender { get; private set; }

        /// <summary>
        /// 这是此消息的唯一消息ID
        /// This is the unique message id for this message
        /// </summary>
        public static ulong UniqueMessageIdCounter { get; private set; }

		/// <summary>
		/// A method to assign the sender (only should be done on server)
		/// </summary>
		/// <param name="sender">The sender to be assigned</param>
		public void SetSender(NetworkingPlayer sender)
		{
			Sender = sender;
		}

		public FrameStream() { }

		/// <summary>
		/// This constructor will take a payload and create a new frame with the appropriate structure
		/// </summary>
		/// <param name="timestep">The timestep that this frame is created within</param>
		/// <param name="useMask">If set to <c>true</c> a mask will be used to encode the payload.</param>
		/// <param name="routerId">A byte that can be used to route data within a user program</param>
		public FrameStream(ulong timestep, bool useMask, Receivers receivers, int groupId, bool isStream
			, byte routerId = 0, ulong roomId = 0)
		{
			CreateFrame(useMask, timestep, new byte[0], receivers, groupId, routerId, isStream, roomId);
		}

		/// <summary>
		/// This constructor will take a payload and create a new frame with the appropriate structure
		/// </summary>
		/// <param name="timestep">The timestep that this frame is created within</param>
		/// <param name="useMask">If set to <c>true</c> a mask will be used to encode the payload.</param>
		/// <param name="payload">The new frame initial data data</param>
		/// <param name="routerId">A byte that can be used to route data within a user program</param>
		public FrameStream(ulong timestep, bool useMask, byte[] payload, Receivers receivers, int groupId, bool isStream, byte routerId = 0, ulong roomId = 0)
		{
			if (payload == null)
				// TODO:  Throw frame exception
				throw new BaseNetworkException("The payload for the frame is not allowed to be null, otherwise use other constructor");

			CreateFrame(useMask, timestep, payload, receivers, groupId, routerId, isStream, roomId);
		}

		/// <summary>
		/// This constructor will take a payload and create a new frame with the appropriate structure
		/// </summary>
		/// <param name="timestep">The timestep that this frame is created within</param>
		/// <param name="useMask">If set to <c>true</c> a mask will be used to encode the payload.</param>
		/// <param name="payload">The new frame initial data data</param>
		/// <param name="routerId">A byte that can be used to route data within a user program</param>
		public FrameStream(ulong timestep, bool useMask, BMSByte payload, Receivers receivers, int groupId, bool isStream, byte routerId = 0, ulong roomId = 0)
		{
			if (payload == null)
				// TODO:  Throw frame exception
				throw new BaseNetworkException("The payload for the frame is not allowed to be null, otherwise use other constructor");

			CreateFrame(useMask, timestep, payload.byteArr, receivers, groupId, routerId, isStream, roomId);
		}

        /// <summary>
        /// 获取一个现有的byte []帧并将其映射到这个数据类型
        /// Take an existing byte[] frame and map it to this data type
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="payloadStart"></param>
        public FrameStream(byte[] frame, int payloadStart, int groupId, NetworkingPlayer sender, byte receivers = 255)
		{
			GroupId = groupId;
			Sender = sender;
			StreamData = new BMSByte();
			ReadFrame(frame, payloadStart, receivers);
		}

        /// <summary>
        /// Take an existing byte[] frame and map it to this data type
        /// </summary>
        /// <param name="frame">The existing frame data</param>
        /// <param name="payloadStart">The index that the payload starts at in the frame byte[]</param>
        /// <summary>
        ///获取一个现有的byte []帧并将其映射到这个数据类型
        /// </ summary>
        /// <param name =“frame”>现有的帧数据</ param>
        /// <param name =“payloadStart”>有效载荷在帧中开始的索引byte [] </ param>
        protected virtual void ReadFrame(byte[] frame, int payloadStart, byte receivers)
		{
            //帧有效载荷的末尾就在唯一ID之前
            // data,, RoomId TimeStep, UniqueId 
            // data,Receivers, RoomId, TimeStep, UniqueId ; receivers==255
            // The end of the frame payload is just before the unique id
            int end = frame.Length - (sizeof(ulong) * 3);

            //如果接收器无效，请将其从数据中拉出
            // If the receivers is invalid, pull it from the data
            if (receivers == 255)
			{
				Receivers = (Receivers)frame[end - 1];
				//end--;
			}
			else
				Receivers = (Receivers)receivers;

            //如果发送了空帧，请不要将其复制到数据，因为数据已经是空的
            // If an empty frame was sent, do not copy it to data as data is already empty
            if (frame.Length - payloadStart > end)
				StreamData.BlockCopy(frame, payloadStart, end - payloadStart);


            // 房间ID
            RoomId = BitConverter.ToUInt64(frame, end);

            // 拉这个框架的时间步骤
            // Pull the time step for this frame
            TimeStep = BitConverter.ToUInt64(frame, end + sizeof(ulong));

            // 拉这个框架的唯一ID
            // Pull the unique id for this frame
            UniqueId = BitConverter.ToUInt64(frame, end + sizeof(ulong) * 2);
        }

        /// <summary>
        /// 使用传入的有效载荷创建帧数据
        /// Creates the frame data using the passed in payload
        /// </summary>
        private void CreateFrame(bool useMask, ulong timestep, byte[] payload, Receivers receivers, int groupId, byte routerId, bool isStream, ulong roomId)
		{
            //如果我们要使用一个蒙版然后生成一个随机蒙版
            // If we are to use a mask then generate a random mask
            if (useMask)
			{
				mask = new byte[4];
				new Random().NextBytes(mask);
			}

			StreamData = new BMSByte();

            RoomId = roomId;
            TimeStep = timestep;
            GroupId = groupId;
			RouterId = routerId;
			Receivers = receivers;
			UniqueId = UniqueMessageIdCounter++;
            //生成frame标识
            // Generate the frame identity
            byte[] frame = new byte[10];

            //数据的第一个字节总是控制字节，它决定了消息的类型
            // The first byte of the data is always the control byte, which dictates the message type
            frame[0] = ControlByte;

			int length = payload.Length;

			if (isStream)
				length += 21;  // Group id (4), receivers (1), time step (8), unique id (8)

			if (frame[0] == Binary.CONTROL_BYTE)
				length += 1;

			// Determine the length of the payload
			int dataStartIndex = 0;
			if (length <= 125)
			{
				frame[1] = (byte)length;
				dataStartIndex = 2;
			}
			else if (length >= 126 && length <= 65535)
			{
				dataStartIndex = 4;
				frame[1] = 126;
			}
			else
			{
				dataStartIndex = 10;
				frame[1] = 127;
			}

			// If the payload is greater than a byte (255) then set the order of the bytes for the length
			if (dataStartIndex > 2)
			{
				int i = 0, j = 2, largestBitIndex = (dataStartIndex - 3) * 8;

				// Little endian / Big endian reversal based on mask
				if (mask.Length == 0)
				{
					for (i = largestBitIndex; i >= 0; i -= 8)
						frame[j++] = (byte)((payload.Length >> i) & 255);
				}
				else
				{
					for (i = 0; i <= largestBitIndex; i += 8)
						frame[j++] = (byte)((payload.Length >> i) & 255);
				}
			}

			// Prepare the stream data with the size so that it doesn't have to keep resizing
			StreamData.SetSize(dataStartIndex + mask.Length + payload.Length);
			StreamData.Clear();

			// Add the frame bytes
			StreamData.BlockCopy(frame, 0, dataStartIndex);

			// Add the mask bytes
			StreamData.BlockCopy(mask, 0, mask.Length);

			// Setup the int that tracks where the payload begins
			payloadStart = dataStartIndex + mask.Length;

			// If we are on a stream then use groupId
			if (isStream)
			{
				StreamData.BlockCopy(BitConverter.GetBytes(groupId), 0, sizeof(int));
				payloadStart += sizeof(int);
			}

			// Copy the routerId if this is a binary frame
			if (frame[0] == Binary.CONTROL_BYTE)
			{
				StreamData.BlockCopy(new byte[1] { routerId }, 0, sizeof(byte));
				payloadStart += 1;
			}

			// Add the initial payload bytes
			StreamData.BlockCopy(payload, 0, payload.Length);

			if (isStream)
				StreamData.Append(new byte[] { (byte)Receivers });

            // 房间ID
            StreamData.BlockCopy<ulong>(RoomId, sizeof(ulong));

            // Add the time step to the end of the frame
            StreamData.BlockCopy<ulong>(TimeStep, sizeof(ulong));

			// Add the unique message id for this frame just before the timestep frame
			StreamData.BlockCopy<ulong>(UniqueId, sizeof(ulong));

            if (mask.Length > 0)
			{
				for (int i = dataStartIndex + mask.Length, j = 0; i < StreamData.Size; i++, j++)
					StreamData.byteArr[i] = (byte)(StreamData.byteArr[i] ^ mask[j % 4]);
			}
		}

        /// <summary>
        /// 设置为可靠的
        /// 从玩家那里生成一个唯一编号
        /// 设置为可靠的
        /// </summary>
        /// <param name="player"></param>
		private void MakeReliable(NetworkingPlayer player)
		{
			UniqueReliableId = player.GetNextReliableId();
			IsReliable = true;
		}

        /// <summary>
        /// Gets the raw data for this frame
        /// </summary>
        /// <returns>The raw byte data prepared by this frame</returns>
        /// <summary>
        ///获取这个帧的原始数据
        /// </ summary>
        /// <returns>由此框架准备的原始字节数据</ returns>
        public byte[] GetData(bool makeReliable = false, NetworkingPlayer player = null)
		{
			if (makeReliable)
			{
				MakeReliable(player);
				reliableCloneData.Clone(StreamData);

				reliableCloneData.InsertRange(StreamData.Size - (sizeof(ulong) * 3), BitConverter.GetBytes(UniqueReliableId));
				return reliableCloneData.CompressBytes();
			}

			return StreamData.CompressBytes();
		}

		protected FrameStream BaseClone(FrameStream target)
		{
			target.StreamData = new BMSByte();
			target.StreamData.Clone(StreamData);
			target.mask = mask;
			target.payloadStart = payloadStart;
			target.TimeStep = TimeStep;
			target.UniqueId = UniqueId;
            target.RoomId = RoomId;
            target.RouterId = RouterId;
			target.GroupId = GroupId;
			target.Receivers = Receivers;
			target.Sender = Sender;

			return target;
		}

        /// <summary>
        /// 提取可靠的ID
        /// </summary>
		public void ExtractReliableId()
		{
            //我们已经使这个frame可靠
            // We've already made this frame reliable
            if (IsReliable)
				return;

			int currentIndex = StreamData.StartIndex();
			StreamData.MoveStartIndex((StreamData.Size - currentIndex - sizeof(ulong)));
			UniqueReliableId = ObjectMapper.Instance.Map<ulong>(StreamData);
			StreamData.MoveStartIndex(-StreamData.StartIndex() + currentIndex);
			IsReliable = true;
		}

		public abstract object Clone();
	}
}