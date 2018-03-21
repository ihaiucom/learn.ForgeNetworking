using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;

namespace Rooms.Forge.Networking.Generated
{
	public partial class RoomNetworkObjectFactory : INetworkObjectFactory
    {
        // 验证创建请求
        private bool ValidateCreateRequest(RoomScene networker, int identity, int classId, uint id, FrameStream frame)
		{
            // TODO:  编写自定义代码来验证客户端对象创建请求
            return true;
		}

        public void NetworkCreateObject(RoomScene networker, int identity, int classId, uint id, FrameStream frame, System.Action<NetworkObject> callback)
        {
            if (networker.IsServer)
            {
                if (frame.Sender != null && frame.Sender != networker.Me)
                {
                    if (!ValidateCreateRequest(networker, identity, classId, id, frame))
                        return;
                }
            }

            NetworkObject obj = null;
            switch (identity)
            {
                case UnitNetworkObject.IDENTITY:
                    switch(classId)
                    {
                        case UnitClassId.Default:
                            obj = new UnitNetworkObject(networker, id, frame);
                            break;
                        case UnitClassId.Hero:
                            obj = new Hero(networker, id, frame);
                            break;
                        case UnitClassId.Solider:
                            obj = new Solider(networker, id, frame);
                            break;
                        case UnitClassId.Tower:
                            obj = new Tower(networker, id, frame);
                            break;
                    }
                    break;
            }

            if (callback != null)
                callback(obj);
        }
    }
}