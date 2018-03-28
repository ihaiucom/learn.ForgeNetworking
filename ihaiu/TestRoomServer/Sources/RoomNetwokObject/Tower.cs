﻿using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;
using Rooms.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;


public class Tower : UnitNetworkObject
{
    public override int ClassId { get { return UnitClassId.Tower; } }


    public Tower(RoomScene networker, int createCode = 0, byte[] metadata = null) : base(networker,  createCode, metadata) { }
    public Tower(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { }

}