using BeardedManStudios.Forge.Networking.Frame;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BeardedManStudios.Forge.Networking
{
	public class CommonServerLogic
	{
        // UDPServer / TCPServer
		private NetWorker server;

		public CommonServerLogic(NetWorker server)
		{
			this.server = server;
		}

		public bool PlayerIsReceiver(NetworkingPlayer player, FrameStream frame, float proximityDistance, NetworkingPlayer skipPlayer = null)
		{
            // 不要将消息发送给尚未被服务器接受的播放器
            // Don't send messages to a player who has not been accepted by the server yet
            if ((!player.Accepted && !player.PendingAccepted) || player == skipPlayer)
				return false;

			if (player == frame.Sender)
			{
                //如果发送给其他人，则不要发送消息给发送方
                // Don't send a message to the sending player if it was meant for others
                if (frame.Receivers == Receivers.Others || frame.Receivers == Receivers.OthersBuffered || frame.Receivers == Receivers.OthersProximity)
					return false;
			}

            //检查请求是否基于邻近
            // Check to see if the request is based on proximity
            if (frame.Receivers == Receivers.AllProximity || frame.Receivers == Receivers.OthersProximity)
			{
                //如果目标玩家与发件人不在同一个邻近区域
                //那么它不应该被发送给那个玩家
                // If the target player is not in the same proximity zone as the sender
                // then it should not be sent to that player
                if (player.ProximityLocation.Distance(frame.Sender.ProximityLocation) > proximityDistance)
				{
					return false;
				}
			}

			return true;
		}

        /// <summary>
        /// 检查所有的客户端，看他们是否超时
        /// Checks all of the clients to see if any of them are timed out
        /// </summary>
        public void CheckClientTimeout(Action<NetworkingPlayer> timeoutDisconnect)
		{
			List<NetworkingPlayer> timedoutPlayers = new List<NetworkingPlayer>();
			while (server.IsBound)
			{
				server.IteratePlayers((player) =>
				{
                    //在此检查期间不要处理服务器
                    // Don't process the server during this check
                    if (player == server.Me)
						return;

					if (player.TimedOut())
					{
						timedoutPlayers.Add(player);
					}
				});

				if (timedoutPlayers.Count > 0)
				{
					foreach (NetworkingPlayer player in timedoutPlayers)
						timeoutDisconnect(player);

					timedoutPlayers.Clear();
				}

                //再等一下再等一下
                // Wait a second before checking again
                Thread.Sleep(1000);
			}
		}

        /// <summary>
        /// Disconnects a client
        /// </summary>
        /// <param name="client">The target client to be disconnected</param>
        /// <summary>
        ///断开一个客户端
        /// </ summary>
        /// <param name =“client”>要断开连接的目标客户端</ param>
        public void Disconnect(NetworkingPlayer player, bool forced,
			List<NetworkingPlayer> DisconnectingPlayers, List<NetworkingPlayer> ForcedDisconnectingPlayers)
		{
			if (player.IsDisconnecting || DisconnectingPlayers.Contains(player) || ForcedDisconnectingPlayers.Contains(player))
				return;

			if (!forced)
				DisconnectingPlayers.Add(player);
			else
				ForcedDisconnectingPlayers.Add(player);
		}
	}
}