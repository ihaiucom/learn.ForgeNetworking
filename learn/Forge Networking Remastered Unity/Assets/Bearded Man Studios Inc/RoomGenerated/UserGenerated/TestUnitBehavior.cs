using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector3\"][\"Vector3\", \"float\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"dir\"][\"target\", \"speed\"]]")]
	public abstract partial class TestUnitBehavior : INetworkBehavior
	{
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_MOVE_TO = 1 + 5;
		
		public TestUnitNetworkObject networkObject = null;

		public void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (TestUnitNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			networkObject.RegisterRpc("Move", Move, typeof(Vector3));
			networkObject.RegisterRpc("MoveTo", MoveTo, typeof(Vector3), typeof(float));
			networkObject.RegistrationComplete();
		}

		public void Initialize(NetWorker networker)
		{
			Initialize(new TestUnitNetworkObject(networker));
		}

        public void Initialize(NetWorker networker, byte[] metadata = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Arguments:
        /// Vector3 dir
        /// </summary>
        public abstract void Move(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// Vector3 target
		/// float speed
		/// </summary>
		public abstract void MoveTo(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}