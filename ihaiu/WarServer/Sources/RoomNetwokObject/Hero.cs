using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;
using Rooms.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;

#if UNITY
using UnityEngine;
#endif

public class Hero : UnitNetworkObject
{
    public override int ClassId { get { return UnitClassId.Hero; } }

    public Hero(RoomScene networker, int createCode = 0, byte[] metadata = null) : base(networker,  createCode, metadata) {  }
    public Hero(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { }

    protected override void Initialize()
    {
        base.Initialize();
        Loger.LogFormat("Initialize NetworkReady={0}, NetworkId={1}, ClientRegistered={2}", NetworkReady, NetworkId, ClientRegistered);

    }

#if UNITY

    GameObject go;

    protected override void OnNetworkStart()
    {
        Loger.LogFormat("OnNetworkStart NetworkReady={0}, NetworkId={1}, ClientRegistered={2}", NetworkReady, NetworkId, ClientRegistered);

        GameObject prefab = Resources.Load<GameObject>("Hero");
        go = GameObject.Instantiate<GameObject>(prefab);
        go.name = string.Format("Hero_NetworkId={0}__Owner={1}__IsOwner={2}", NetworkId, Owner.NetworkId, IsOwner);
        go.transform.position = position;
        go.GetComponent<BindNetworkObject>().unitNetworkObject = this;
    }

    protected override void OnDestroy()
    {
        Loger.LogFormat("OnDestroy NetworkReady={0}, NetworkId={1}, ClientRegistered={2}", NetworkReady, NetworkId, ClientRegistered);

        if (go != null)
            GameObject.Destroy(go);
    }

#endif
}
