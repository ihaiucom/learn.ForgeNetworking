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

namespace BeardedManStudios.Forge.Networking.Frame
{
    /// <summary>
    /// 发送ping
    /// </summary>
	public class Ping : FrameStream
	{
        /// <summary>
        /// 连接关闭帧由％x9表示
        /// Connection close frame is denoted by %x9
        /// </summary>
        public const byte CONTROL_BYTE = 137;

		public override byte ControlByte { get { return CONTROL_BYTE; } }

		public Ping() : base() { }
		public Ping(ulong timestep, bool useMask, Receivers receivers, int groupId, bool isStream) : base(timestep, useMask, receivers, groupId, isStream) { }
		public Ping(ulong timestep, bool useMask, byte[] payload, Receivers receivers, int groupId, bool isStream) : base(timestep, useMask, payload, receivers, groupId, isStream) { }
		public Ping(ulong timestep, bool useMask, BMSByte payload, Receivers receivers, int groupId, bool isStream) : base(timestep, useMask, payload, receivers, groupId, isStream) { }
		public Ping(byte[] frame, int payloadStart, int groupId, NetworkingPlayer sender, byte receivers) : base(frame, payloadStart, groupId, sender, receivers) { }

		public override object Clone()
		{
			return BaseClone(new Binary());
		}
	}
}