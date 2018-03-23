using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[]")]
	[GeneratedRPCVariableNames("{\"types\":[]")]
	public abstract partial class ZfTestAABehavior : INetworkBehavior
	{
		
		public ZfTestAANetworkObject networkObject = null;

		public void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (ZfTestAANetworkObject)obj;
			networkObject.AttachedBehavior = this;

			networkObject.RegistrationComplete();
		}

		public void Initialize(NetWorker networker)
		{
			Initialize(new ZfTestAANetworkObject(networker));
		}

        void INetworkBehavior.Initialize(NetworkObject obj)
        {
            throw new NotImplementedException();
        }

        void INetworkBehavior.Initialize(NetWorker networker, byte[] metadata)
        {
            throw new NotImplementedException();
        }


        // DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
    }
}