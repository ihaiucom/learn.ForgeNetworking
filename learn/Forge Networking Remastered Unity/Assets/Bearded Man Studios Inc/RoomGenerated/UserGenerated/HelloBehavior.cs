using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace Rooms.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector3\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"target\"]]")]
	public abstract partial class HelloBehavior : INetworkBehavior
	{
		public const byte RPC_MOVE = 0 + 5;
		
		public HelloNetworkObject networkObject = null;

		public void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (HelloNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			networkObject.RegisterRpc("Move", Move, typeof(Vector3));
			networkObject.RegistrationComplete();
		}

		public void Initialize(RoomScene networker)
		{
			Initialize(new HelloNetworkObject(networker));
		}

        public void Initialize(RoomScene networker, byte[] metadata = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Arguments:
        /// Vector3 target
        /// </summary>
        public abstract void Move(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}