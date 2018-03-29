namespace Games.Wars
{
    public class SecondTarget
    {
        /// <summary>
        /// 二次伤害目标数量
        /// </summary>
        public  int                                     maxSceondTargets            = 0;
        /// <summary>
        /// 特效路径
        /// </summary>
        public  string                                  secondEffectPath            = "";
        public  string                                  secondEffectPathsuf         = "";
        /// <summary>
        /// 攻击到目标才有二次伤害
        /// </summary>
        public  bool                                    hitActiveSecond             = false;
        /// <summary>
        /// 生命周期
        /// </summary>
        public  float                                   life                        = 1;
        /// <summary>
        /// 二次伤害规则
        /// </summary>
        public  WarSkillRule                            warSkillRule                = WarSkillRule.RandomRange;
        /// <summary>
        /// 二次伤害半径
        /// </summary>
        public  float                                   secondaryRadius             = 0;
        /// <summary>
        /// 具体伤害
        /// </summary>
        public  DamageInfo                              damageInfo                  = new DamageInfo();
        /// <summary>
        /// 爆炸音效
        /// </summary>
        public  string                                  secondaryMusic              = "";
    }
}
