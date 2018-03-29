namespace Games.Wars
{
    /// <summary>
    /// 激活类型
    /// </summary>
    public class SkillActivation
    {
        /// <summary>
        /// 主动技能被动技能区分，0主动技能，1被动技能
        /// </summary>
        public  Activation      activation;
        /// <summary>
        /// 强制停止释放技能
        /// </summary>
        public  bool            forceStop               = true;
        /// <summary>
        /// 是否打断其他技能
        /// </summary>
        public  bool            breakOther              = false;
    }
}
