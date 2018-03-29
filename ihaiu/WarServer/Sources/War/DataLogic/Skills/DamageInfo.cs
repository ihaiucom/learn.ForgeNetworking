using FMODUnity;

namespace Games.Wars
{
    public class DamageInfo
    {
        public  DamageInfBaseCSV                        damageInfBaseCSV            = new DamageInfBaseCSV();
        /// <summary>
        /// 参考属性类型
        /// </summary>
        public  SelectTarget                            selectTarget                = SelectTarget.Self;
        public  bool                                    passiveJudgment             = false;
        public  int                                     buffid                      = 0;
        public  float                                   bufflife                    = 0;
        public  int                                     haloid                      = 0;
        public  float                                   halolife                    = 0;
        /// <summary>
        /// 是否震屏
        /// </summary>
        public  bool                                    bShakeEffect                = false;
        /// <summary>
        /// 振幅
        /// </summary>
        public  float                                   shakeAmplitude              = 0;
        /// <summary>
        /// 震屏时间
        /// </summary>
        public  float                                   shakeTime                   = 0;
        /// <summary>
        /// 影响范围
        /// </summary>
        public  float                                   shakeRange                  = 0;
        /// <summary>
        /// 伤害音效
        /// </summary>
        [SoundKey]
        public  string                                  DamageMusic                 = "";
    }
}
