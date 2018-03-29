using System;
using System.Collections.Generic;
namespace Games.Wars
{
    /// <summary>
    /// 士兵  AI类型
    /// </summary>
    [Flags]
    public enum UnitSoliderAIType
    {
        /** 正常操作 */
        None = 0,

        /** 依照路径移动，到达终点后自动销毁 */
        PathOver = 1,

        /** 无移动，生命周期结束后自动销毁 */
        LiveOver = 2,

        /** 依照路径移动，到达终点或生命周期结束后自动销毁 */
        PathOrLiveOver = 3,
    }
}
