namespace Games.Wars
{
    public enum BuffTriggerType
    {
        /// <summary>
        /// 一次性
        /// </summary>
        Disposable = 0,
        /// <summary>
        /// 周期性
        /// </summary>
        Periodic = 1,
        /// <summary>
        /// 一定次数，依据条件减少次数
        /// </summary>
        Frequency = 2,
    }
}