namespace Games.Wars
{
    /// <summary>
    /// 目标圆形区域
    /// </summary>
    public class SkillTriggerUseTargetCircleArea : SkillTriggerLocation
    {
        /// <summary>
        /// 圆形半径
        /// </summary>
        public  float           targetFanRadius         = 1;
        /// <summary>
        /// 目标圆形距离自身距离
        /// </summary>
        public  float           targetCircleAreaDis     = 1;

    }
}