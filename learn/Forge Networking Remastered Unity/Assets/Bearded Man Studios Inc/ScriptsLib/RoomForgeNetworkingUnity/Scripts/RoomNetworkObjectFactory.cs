using BeardedManStudios.Forge.Networking.Frame;
using Rooms.Forge.Networking;

namespace Rooms.Forge.Networking.Generated
{
	public partial class RoomNetworkObjectFactory
	{
        // 验证创建请求
        private bool ValidateCreateRequest(RoomScene networker, int identity, uint id, FrameStream frame)
		{
            // TODO:  编写自定义代码来验证客户端对象创建请求
            return true;
		}
	}
}