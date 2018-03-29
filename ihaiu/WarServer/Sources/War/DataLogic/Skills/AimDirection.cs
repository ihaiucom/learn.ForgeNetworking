namespace Games.Wars
{
    public class AimDirection
    {
        /// <summary>
        /// 技能执行的方向和位置
        /// </summary>
        public  TargetLocation  targetLocation;
        /// <summary>
        /// 直线宽度
        /// </summary>
        public  float           targetLocationWidth     = 0.02F;
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
        /// <summary>
        /// 目标圆形距离自身距离
        /// </summary>
        public  float           targetCircleAreaDis     = 1;
        /// <summary>
        /// 启用等级
        /// </summary>
        public  int             activeLv                = 1;
        /// <summary>
        /// 停用等级
        /// </summary>
        public  int             blockLv                 = 15;
    }
}
