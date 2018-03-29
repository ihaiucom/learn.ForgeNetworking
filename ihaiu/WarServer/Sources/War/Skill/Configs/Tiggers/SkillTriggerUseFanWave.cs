namespace Games.Wars
{
    /// <summary>
    /// 扇形冲击波
    /// </summary>
    public class SkillTriggerUseFanWave : SkillTriggerLocation
    {
        /// <summary>
        /// 最大半径
        /// </summary>
        public  float           targetFanRadius         = 1;
        /// <summary>
        /// 最小半径
        /// </summary>
        public  float           targetMinRadius         = 0;
        /// <summary>
        /// 扇形角度
        /// </summary>
        public  float           targetFanAngle          = 45;

    }
}