//
// System.Net.Sockets.UdpClient.cs
//
// Author:
//	Gonzalo Paniagua Javier <gonzalo@ximian.com>
//	Sridhar Kulkarni (sridharkulkarni@gmail.com)
//	Marek Safar (marek.safar@gmail.com)
//
// Modified by:
//	Brent Farris (brent@beardedmangames.com)
//
// Copyright (C) Ximian, Inc. http://www.ximian.com
// Copyright 2011 Xamarin Inc.
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if !UNITY_WEBGL
#if !NetFX_CORE

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace BeardedManStudios.Forge.Networking
{
    /// <summary>
    /// 缓存Udp客户端
    /// </summary>
	public class CachedUdpClient : IDisposable
	{
        // HOST PORT CHARACTER SEPARATOR
        public const char HOST_PORT_CHARACTER_SEPARATOR = '+';

        // 是否销毁, 关闭Socket
		private bool disposed = false;
        // socket是否建立连接
		private bool active = false;
		private Socket socket;
		private AddressFamily family = AddressFamily.InterNetwork;
		private byte[] recvbuffer;

		public CachedUdpClient()
			: this(AddressFamily.InterNetwork)
		{
		}

		public CachedUdpClient(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
				throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");

			this.family = family;
			InitSocket(null);
		}

		public CachedUdpClient(int port)
		{
			if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
				throw new ArgumentOutOfRangeException("port");

			this.family = AddressFamily.InterNetwork;

			IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
			InitSocket(localEP);
		}

		public CachedUdpClient(IPEndPoint localEP)
		{
			if (localEP == null)
				throw new ArgumentNullException("localEP");

			this.family = localEP.AddressFamily;

			InitSocket(localEP);
		}

		public CachedUdpClient(int port, AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
				throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");

			if (port < IPEndPoint.MinPort ||
				port > IPEndPoint.MaxPort)
			{
				throw new ArgumentOutOfRangeException("port");
			}

			this.family = family;

			IPEndPoint localEP;

			if (family == AddressFamily.InterNetwork)
				localEP = new IPEndPoint(IPAddress.Any, port);
			else
				localEP = new IPEndPoint(IPAddress.IPv6Any, port);
			InitSocket(localEP);
		}

		public CachedUdpClient(Socket targetSocket)
		{
			socket = targetSocket;
		}

		public CachedUdpClient(string hostname, int port)
		{
			if (hostname == null)
				throw new ArgumentNullException("hostname");

			if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
				throw new ArgumentOutOfRangeException("port");

			InitSocket(null);
			Connect(hostname, port);
		}

		private void InitSocket(EndPoint localEP)
		{
			if (socket != null)
			{
				socket.Close();
				socket = null;
			}

			socket = new Socket(family, SocketType.Dgram, ProtocolType.Udp);

			if (localEP != null)
				socket.Bind(localEP);

			recBuffer.SetSize(65536);
		}

		#region Close
		public void Close()
		{
			((IDisposable)this).Dispose();
		}
		#endregion
		#region Connect

		void DoConnect(IPEndPoint endPoint)
		{
            /*赶上EACCES，然后打开SO_BROADCAST，
            *因为UDP套接字没有默认设置
            */
            /* Catch EACCES and turn on SO_BROADCAST then,
			 * as UDP Sockets don't have it set by default
			 */
            try
			{
				socket.Connect(endPoint);
			}
			catch (SocketException ex)
			{
				if (ex.ErrorCode == (int)SocketError.AccessDenied)
				{
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

					socket.Connect(endPoint);
				}
				else
				{
					throw;
				}
			}
		}

		public void Connect(IPEndPoint endPoint)
		{
			CheckDisposed();
			if (endPoint == null)
				throw new ArgumentNullException("endPoint");

			DoConnect(endPoint);
			active = true;
		}

		public void Connect(IPAddress addr, int port)
		{
			if (addr == null)
				throw new ArgumentNullException("addr");

			if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
				throw new ArgumentOutOfRangeException("port");


			Connect(new IPEndPoint(addr, port));
		}

		public void Connect(string hostname, int port)
		{
			if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
				throw new ArgumentOutOfRangeException("port");

			IPAddress[] addresses = Dns.GetHostAddresses(hostname);
			for (int i = 0; i < addresses.Length; i++)
			{
				try
				{
					this.family = addresses[i].AddressFamily;
					Connect(new IPEndPoint(addresses[i], port));
					break;
				}
				catch (Exception e)
				{
					if (i == addresses.Length - 1)
					{
						if (socket != null)
						{
							socket.Close();
							socket = null;
						}
						/// This is the last entry, re-throw the exception
						throw e;
					}
				}
			}
		}
		#endregion
		#region Multicast methods
		public void DropMulticastGroup(IPAddress multicastAddr)
		{
			CheckDisposed();
			if (multicastAddr == null)
				throw new ArgumentNullException("multicastAddr");

			if (family == AddressFamily.InterNetwork)
				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership,
					new MulticastOption(multicastAddr));
			else
				socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership,
					new IPv6MulticastOption(multicastAddr));
		}

		public void DropMulticastGroup(IPAddress multicastAddr,
			int ifindex)
		{
			CheckDisposed();

			/* LAMESPEC: exceptions haven't been specified
			 * for this overload.
			 */
			if (multicastAddr == null)
			{
				throw new ArgumentNullException("multicastAddr");
			}

			/* Does this overload only apply to IPv6?
			 * Only the IPv6MulticastOption has an
			 * ifindex-using constructor.  The MS docs
			 * don't say.
			 */
			if (family == AddressFamily.InterNetworkV6)
			{
				socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, new IPv6MulticastOption(multicastAddr, ifindex));
			}
		}

		public void JoinMulticastGroup(IPAddress multicastAddr)
		{
			CheckDisposed();

			if (multicastAddr == null)
				throw new ArgumentNullException("multicastAddr");

			if (family == AddressFamily.InterNetwork)
				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
					new MulticastOption(multicastAddr));
			else
				socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership,
					new IPv6MulticastOption(multicastAddr));
		}

		public void JoinMulticastGroup(int ifindex,
			IPAddress multicastAddr)
		{
			CheckDisposed();

			if (multicastAddr == null)
				throw new ArgumentNullException("multicastAddr");

			if (family == AddressFamily.InterNetworkV6)
				socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, new IPv6MulticastOption(multicastAddr, ifindex));
			else
				throw new SocketException((int)SocketError.OperationNotSupported);
		}

		public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive)
		{
			CheckDisposed();
			if (multicastAddr == null)
				throw new ArgumentNullException("multicastAddr");
			if (timeToLive < 0 || timeToLive > 255)
				throw new ArgumentOutOfRangeException("timeToLive");

			JoinMulticastGroup(multicastAddr);
			if (family == AddressFamily.InterNetwork)
				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive,
					timeToLive);
			else
				socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastTimeToLive,
					timeToLive);
		}

		public void JoinMulticastGroup(IPAddress multicastAddr,
			IPAddress localAddress)
		{
			CheckDisposed();

			if (family == AddressFamily.InterNetwork)
				socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddr, localAddress));
			else
				throw new SocketException((int)SocketError.OperationNotSupported);
		}

		#endregion
		#region Data I/O
		private BMSByte recBuffer = new BMSByte();
		private EndPoint endPoint = null;
		private Dictionary<EndPoint, string> connections = new Dictionary<EndPoint, string>();
		public BMSByte Receive(ref IPEndPoint remoteEP, ref string endpoint)
		{
			CheckDisposed();

			recBuffer.Clear();

			if (endPoint == null)
				endPoint = new IPEndPoint(IPAddress.Any, 0);

            // 将数据报接收到数据缓冲区并存储终结点。
            // buffer Byte 类型的数组，它是存储接收到的数据的位置。
            // remoteEP 按引用传递的 EndPoint，表示远程服务器。
            // 返回值 接收到的字节数。
            int dataRead = socket.ReceiveFrom(recBuffer.byteArr, ref endPoint);

			if (!connections.ContainsKey(endPoint))
				connections.Add(endPoint, (((IPEndPoint)endPoint).Address.ToString() + HOST_PORT_CHARACTER_SEPARATOR + ((IPEndPoint)endPoint).Port.ToString()));

			endpoint = connections[endPoint];

			//if (dataRead < recBuffer.Size)
			//	recBuffer = CutArray(recBuffer, dataRead);

			recBuffer.SetSize(dataRead);

			remoteEP = (IPEndPoint)endPoint;
			return recBuffer;
		}

		int DoSend(byte[] dgram, int bytes, IPEndPoint endPoint)
		{
			/* Catch EACCES and turn on SO_BROADCAST then,
			 * as UDP Sockets don't have it set by default
			 */
			try
			{
				if (endPoint == null)
				{
                    // 使用指定的 SocketFlags，将指定字节数的数据发送到已连接的 Socket（从指定的偏移量开始）。
                    //buffer Byte 类型的数组，它包含要发送的数据。
                    //offset 数据缓冲区中开始发送数据的位置。
                    // size 要发送的字节数。
                    // socketFlags SocketFlags 值的按位组合。
                    // 返回值已发送到 Socket 的字节数。

                    return (socket.Send(dgram, 0, bytes,
						SocketFlags.None));
				}
				else
				{
                    // 使用指定的 SocketFlags，将指定字节数的数据发送到指定终结点（从缓冲区中的指定位置开始）
                    // remoteEP EndPoint，它表示数据的目标位置。
                    return (socket.SendTo(dgram, 0, bytes,
						SocketFlags.None,
						endPoint));
				}
			}
			catch (SocketException ex)
			{
                Loger.LogError(ex.ToString());
				if (ex.ErrorCode == (int)SocketError.AccessDenied)
				{
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
					if (endPoint == null)
					{
						return (socket.Send(dgram, 0, bytes, SocketFlags.None));
					}
					else
					{
						return (socket.SendTo(dgram, 0, bytes, SocketFlags.None, endPoint));
					}
				}
				else
					throw new InvalidProgramException(ex.Message);
			}
		}

		public int Send(byte[] dgram, int bytes)
		{
			CheckDisposed();
			if (dgram == null)
				throw new ArgumentNullException("dgram");

			if (!active)
				throw new InvalidOperationException("Operation not allowed on " +
					"non-connected Sockets.");

			return (DoSend(dgram, bytes, null));
		}

		public int Send(byte[] dgram, int bytes, IPEndPoint endPoint)
		{
			CheckDisposed();
			if (dgram == null)
				throw new ArgumentNullException("dgram is null");

			if (active)
			{
				if (endPoint != null)
					throw new InvalidOperationException("Cannot send packets to an " +
						"arbitrary host while connected.");

				return (DoSend(dgram, bytes, null));
			}

			return (DoSend(dgram, bytes, endPoint));
		}

		public int Send(byte[] dgram, int bytes, string hostname, int port)
		{
			return Send(dgram, bytes,
				new IPEndPoint(Dns.GetHostAddresses(hostname)[0], port));
		}

		private byte[] CutArray(byte[] orig, int length)
		{
			byte[] newArray = new byte[length];
			Buffer.BlockCopy(orig, 0, newArray, 0, length);

			return newArray;
		}
		#endregion

		IAsyncResult DoBeginSend(byte[] datagram, int bytes,
			IPEndPoint endPoint,
			AsyncCallback requestCallback,
			object state)
		{
			/* Catch EACCES and turn on SO_BROADCAST then,
			 * as UDP Sockets don't have it set by default
			 */
			try
			{
				if (endPoint == null)
				{
					return (socket.BeginSend(datagram, 0, bytes, SocketFlags.None, requestCallback, state));
				}
				else
				{
					return (socket.BeginSendTo(datagram, 0, bytes, SocketFlags.None, endPoint, requestCallback, state));
				}
			}
			catch (SocketException ex)
			{
				if (ex.ErrorCode == (int)SocketError.AccessDenied)
				{
					socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
					if (endPoint == null)
					{
						return (socket.BeginSend(datagram, 0, bytes, SocketFlags.None, requestCallback, state));
					}
					else
					{
						return (socket.BeginSendTo(datagram, 0, bytes, SocketFlags.None, endPoint, requestCallback, state));
					}
				}
				else
				{
					throw;
				}
			}
		}

		public IAsyncResult BeginSend(byte[] datagram, int bytes,
			AsyncCallback requestCallback,
			object state)
		{
			return (BeginSend(datagram, bytes, null,
				requestCallback, state));
		}

		public IAsyncResult BeginSend(byte[] datagram, int bytes,
			IPEndPoint endPoint,
			AsyncCallback requestCallback,
			object state)
		{
			CheckDisposed();

			if (datagram == null)
			{
				throw new ArgumentNullException("datagram");
			}

			return (DoBeginSend(datagram, bytes, endPoint,
				requestCallback, state));
		}

		public IAsyncResult BeginSend(byte[] datagram, int bytes,
			string hostname, int port,
			AsyncCallback requestCallback,
			object state)
		{
			return (BeginSend(datagram, bytes, new IPEndPoint(Dns.GetHostAddresses(hostname)[0], port), requestCallback, state));
		}

		public int EndSend(IAsyncResult asyncResult)
		{
			CheckDisposed();

			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult is a null reference");
			}

			return (socket.EndSend(asyncResult));
		}

		public IAsyncResult BeginReceive(AsyncCallback requestCallback, object state)
		{
			CheckDisposed();

			recvbuffer = new byte[8192];

			EndPoint ep;

			if (family == AddressFamily.InterNetwork)
			{
				ep = new IPEndPoint(IPAddress.Any, 0);
			}
			else
			{
				ep = new IPEndPoint(IPAddress.IPv6Any, 0);
			}

			return (socket.BeginReceiveFrom(recvbuffer, 0, 8192,
				SocketFlags.None,
				ref ep,
				requestCallback, state));
		}

		public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint remoteEP)
		{
			CheckDisposed();

			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult is a null reference");
			}

			EndPoint ep;

			if (family == AddressFamily.InterNetwork)
			{
				ep = new IPEndPoint(IPAddress.Any, 0);
			}
			else
			{
				ep = new IPEndPoint(IPAddress.IPv6Any, 0);
			}

			int bytes = socket.EndReceiveFrom(asyncResult,
				ref ep);
			remoteEP = (IPEndPoint)ep;

			/* Need to copy into a new array here, because
			 * otherwise the returned array length is not
			 * 'bytes'
			 */
			byte[] buf = new byte[bytes];
			Array.Copy(recvbuffer, buf, bytes);

			return (buf);
		}

		#region Properties
		protected bool Active
		{
			get { return active; }
			set { active = value; }
		}

		public Socket Client
		{
			get { return socket; }
			set { socket = value; }
		}

        // 获取已经从网络接收且可供读取的数据量。
        public int Available
		{
			get
			{
				return (socket.Available);
			}
		}

        //获取或设置 Boolean 值，该值指定 Socket 是否允许将 Internet 协议 (IP) 数据报分段。
        // [备注]
        //如果数据报大小超过传输介质的最大传送单位(MTU)，则需要将数据报分段。
        //可以由发送主机（所有版本的 Internet 协议）或中间路由器（仅 Internet 协议版本 4）将数据报分段。
        //如果必须将一个数据报分段，并且已设置 DontFragment 选项，则该数据报被丢弃并且将 Internet 控制消息协议(ICMP) 错误信息发送回数据报发送方。
        //对传输控制协议(TCP) 套接字设置此属性不起任何作用。
        public bool DontFragment
		{
			get
			{
				return (socket.DontFragment);
			}
			set
			{
				socket.DontFragment = value;
			}
		}

        /// <summary>
        /// 获取或设置一个 Boolean 值，该值指定 Socket 是否可以发送或接收广播数据包。
        /// </summary>
		public bool EnableBroadcast
		{
			get
			{
				return (socket.EnableBroadcast);
			}
			set
			{
				socket.EnableBroadcast = value;
			}
		}

        // 获取或设置 Boolean 值，该值指定 Socket 是否仅允许一个进程绑定到端口。
        public bool ExclusiveAddressUse
		{
			get
			{
				return (socket.ExclusiveAddressUse);
			}
			set
			{
				socket.ExclusiveAddressUse = value;
			}
		}

        // 获取或设置一个值，该值指定传出的多路广播数据包是否传递到发送应用程序。
        public bool MulticastLoopback
		{
			get
			{
				return (socket.MulticastLoopback);
			}
			set
			{
				socket.MulticastLoopback = value;
			}
		}

        // 	获取或设置一个值，指定 Socket 发送的 Internet 协议 (IP) 数据包的生存时间 (TTL) 值。
        public short Ttl
		{
			get
			{
				return (socket.Ttl);
			}
			set
			{
				socket.Ttl = value;
			}
		}

		#endregion
		#region Disposing
        /// <summary>
        /// 手动释放
        /// </summary>
		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// 释放， 关闭socket
        /// </summary>
        /// <param name="disposing">是否是手动释放</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			disposed = true;

			if (disposing)
			{
				if (socket != null)
					socket.Close();

				socket = null;
			}
		}

        /// <summary>
        /// 析构方法， 释放关闭socket
        /// </summary>
		~CachedUdpClient()
		{
			Dispose(false);
		}

        /// <summary>
        /// 检测是否释放， 如果释放就抛错误
        /// </summary>
		private void CheckDisposed()
		{
			if (disposed)
				throw new ObjectDisposedException(GetType().FullName);
		}
		#endregion

#if Net_4_5

public Task<UdpReceiveResult> ReceiveAsync ()
{
return Task<UdpReceiveResult>.Factory.FromAsync (BeginReceive, r => {
IPEndPoint remoteEndPoint = null;
return new UdpReceiveResult (EndReceive (r, ref remoteEndPoint), remoteEndPoint);
}, null);
}

public Task<int> SendAsync (byte[] datagram, int bytes)
{
return Task<int>.Factory.FromAsync (BeginSend, EndSend, datagram, bytes, null);
}

public Task<int> SendAsync (byte[] datagram, int bytes, IPEndPoint endPoint)
{
return Task<int>.Factory.FromAsync (BeginSend, EndSend, datagram, bytes, endPoint, null);
}

public Task<int> SendAsync (byte[] datagram, int bytes, string hostname, int port)
{
var t = Tuple.Create (datagram, bytes, hostname, port, this);

return Task<int>.Factory.FromAsync ((callback, state) => {
var d = (Tuple<byte[], int, string, int, UdpClient>) state;
return d.Item5.BeginSend (d.Item1, d.Item2, d.Item3, d.Item4, callback, null);
}, EndSend, t);

}
#endif
	}
}

#endif
#endif