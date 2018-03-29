using System;
using System.Collections.Generic;
namespace Games.Wars
{
    /// <summary>
    /// Global的key值
    /// </summary>
    [Flags]
    public enum GlobalKey : int
    {
        //--副本战前准备时间
        PreparationTime = 1,
        //--副本所有人准备完毕后，等待x秒进入loading
        WaitTime = 2,
        //--监狱的单位ID
        PrisonID = 3,
        //--匹配成功确认进入时间
        ReadyOkTime = 4,
        //--loading加载时间
        LoadingTime = 5,
        //--体力恢复间隔（秒）
        PhysicalRecovery = 6,
        //--竞技场挑战CD（秒）
        PVPLadderCD = 7,
        //--竞技场挑战次数上限
        PVPLadderMaxTimes = 8,
        //--竞技场挑战次数每日重置时间（服务器时间）
        PVPLadderTicketsRest = 9,
        //--竞技场排名奖励每日发放时间（服务器时间）
        PVPLadderRewardGive = 10,
        //--竞技场门票Item表格ID
        PVPLadderTickItemID = 11,
        //--竞技场门票价格（绑定金砖）
        PVPLadderTickPrice = 12,
        //--怪物死亡模式：1无特效，2有血腥特效
        DeathGrading = 25,
    }
}
