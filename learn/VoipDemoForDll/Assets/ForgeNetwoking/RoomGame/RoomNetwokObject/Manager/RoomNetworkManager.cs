using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.SimpleJSON;
using Rooms.Forge.Networking.Generated;
using System.Collections.Generic;
using UnityEngine;


namespace Rooms.Forge.Networking.Generated
{
    public partial class RoomNetworkObjectFactory
    {
        private RoomScene Networker { get; set; }
        private BMSByte metadata = new BMSByte();

        public RoomNetworkObjectFactory(RoomScene Networker)
        {
            this.Networker = Networker;
        }

        private void WriteMetadata(Vector3? position = null, Quaternion? rotation = null)
        {
            metadata.Clear();

            byte transformFlags = 0x0;
            transformFlags |= (byte)(position != null ? 0x1 : 0x0);
            transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
            ObjectMapper.Instance.MapBytes(metadata, transformFlags);

            if (position != null)
                ObjectMapper.Instance.MapBytes(metadata, position.Value);

            if (rotation != null)
                ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
        }

        public UnitNetworkObject InstantiateUnit( Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
        {
            int index = 0;
            UnitNetworkObject obj = null;
            if (!sendTransform && position == null && rotation == null)
            {
                obj = new UnitNetworkObject(Networker, index);
            }
            else
            {
                WriteMetadata(position, rotation);
                obj = new UnitNetworkObject(Networker, index, metadata.CompressBytes());
            }
            return obj;
        }

        public UnitNetworkObject InstantiateHero(Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
        {
            int index = 0;
            Hero obj = null;
            if (!sendTransform && position == null && rotation == null)
            {
                obj = new Hero(Networker, index);
            }
            else
            {
                WriteMetadata(position, rotation);
                obj = new Hero(Networker, index, metadata.CompressBytes());
            }
            return obj;
        }

        public UnitNetworkObject InstantiateSolider(Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
        {
            int index = 0;
            Solider obj = null;
            if (!sendTransform && position == null && rotation == null)
            {
                obj = new Solider(Networker, index);
            }
            else
            {
                WriteMetadata(position, rotation);
                obj = new Solider(Networker, index, metadata.CompressBytes());
            }
            return obj;
        }


        public UnitNetworkObject InstantiateTower(Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
        {
            int index = 0;
            Tower obj = null;
            if (!sendTransform && position == null && rotation == null)
            {
                obj = new Tower(Networker, index);
            }
            else
            {
                WriteMetadata(position, rotation);
                obj = new Tower(Networker, index, metadata.CompressBytes());
            }
            return obj;
        }
    }
}