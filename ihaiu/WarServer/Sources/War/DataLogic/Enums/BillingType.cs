using System;
using System.Collections.Generic;
namespace Games.Wars
{
    /// <summary>
    /// 结算类型，所有结算类型都包含时间到结束
    /// </summary>
    [Flags]
    public enum BillingType
    {
        /** 正常操作，怪物全部消灭 */
        None = 0,

        /** 双方英雄某一个死亡 */
        DeathHero = 1,

        /** 士兵结束 */
        SoliderFinal,
    }
}
