using BeardedManStudios.Forge.Networking.Frame;

namespace BeardedManStudios.Forge.Networking.Generated
{
	public partial class NetworkObjectFactory
	{
        // 验证创建请求
        private bool ValidateCreateRequest(NetWorker networker, int identity, uint id, FrameStream frame)
		{
            // TODO:  编写自定义代码来验证客户端对象创建请求
            // TODO:  Write custom code to validate client object create requests
            return true;
		}
	}
}