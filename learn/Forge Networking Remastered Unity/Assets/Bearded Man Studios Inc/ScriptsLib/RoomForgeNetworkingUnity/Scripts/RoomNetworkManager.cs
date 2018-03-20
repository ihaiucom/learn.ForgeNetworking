using BeardedManStudios;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.SimpleJSON;
using System.Collections.Generic;

namespace Rooms.Forge.Networking.Unity
{
	public partial class RoomNetworkManager
	{
        public delegate void InstantiateEvent(INetworkBehavior unityGameObject, NetworkObject obj);
        public event InstantiateEvent objectInitialized;
        private BMSByte metadata = new BMSByte();
        public RoomScene Networker { get; private set; }
        public RoomNetworkManager(RoomScene roomScene)
        {
            Networker = roomScene;

            Networker.objectCreated += CaptureObjects;
        }

        public void Destory()
        {
            if (Networker != null)
                Networker.objectCreated -= CaptureObjects;
        }

        private void CaptureObjects(NetworkObject obj)
        {
            if (obj.CreateCode < 0)
                return;

        }

        private void InitializedObject(INetworkBehavior behavior, NetworkObject obj)
        {
            if (objectInitialized != null)
                objectInitialized(behavior, obj);

            obj.pendingInitialized -= InitializedObject;
        }

    }
}