using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;
using Rooms.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : UnitNetworkObject
{
    public override int ClassId { get { return UnitClassId.Hero; } }

    public Hero(RoomScene networker, int createCode = 0, byte[] metadata = null) : base(networker,  createCode, metadata) {  }
    public Hero(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { }

}
