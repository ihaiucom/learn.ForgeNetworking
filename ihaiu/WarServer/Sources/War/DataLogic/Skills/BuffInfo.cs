using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    //public class BuffInfo
    //{
    //    public  int                     BuffInfoId;                 // id
    //    public  string                  BuffInfoName;               // 名称
    //    public  bool                    isBuffLayer                 = false;                // 依据一定条件，依照次数减少的buff
    //    public  float                   mCD;                        // 间隔生效cd
    //    public  bool                    mSuperposition;             // 能否叠加
    //    public  string                  mEffectPath;                // 特效路径
    //    public  Vector3                 mEffectPos;                 // 位置
    //    public  float                   mRadius;                    // 区域，针对halo
    //    public  DamageInfBaseCSV        damageInfBaseCSV            = new DamageInfBaseCSV();
    //    public  DamageType              damageType;
    //    public  int                     mDamageFirstid;             // 即时伤害 大于0，表示添加buff即生效一次伤害
    //    public  int                     damageRefId;
    //    public  PropType                damageRefRpopType;
    //    public  float                   damageRatio;
    //    public  float                   mDamageFirstVal             = 0;
    //    public  DamageInfBaseCSV        pulDamageInfBaseCSV         = new DamageInfBaseCSV();
    //    public  DamageType              damageTypePul;
    //    public  int                     mDamagePulseid;             // 脉冲伤害 大于0，表示存在脉冲伤害
    //    public  int                     pulDamageRefId;
    //    public  PropType                pulDamageRefRpopType;
    //    public  float                   pulDamageRatio;
    //    public  float                   mDamagePulseVal             = 0;
    //    public  float                   mLife;                      // 生命周期
    //    public  Prop[]                  mProp;                      // 附加属性，数组小于0表示不存在附加属性
    //    public  List<BuffProp>          buffPropList                = new List<BuffProp>();
    //    /// <summary>
    //    /// 仇恨值
    //    /// </summary>
    //    public  int                     hatredValue                 = 0;

    //    /// <summary>
    //    /// 特殊属性
    //    /// </summary>
    //    public  SpecialFunctionType     specialFun                  = SpecialFunctionType.None;
    //    public  float                   specialVal                  = 0;
    //}

    public class HaloBuff
    {
        /// <summary>
        /// id
        /// </summary>
        public int                      HaloBuffId;
        /// <summary>
        /// 名称或描述
        /// </summary>
        public string                   HaloBuffName;
        /// <summary>
        /// 类型判断，是buff或是光环
        /// </summary>
        public BuffHaloType             buffHaloType;
        /// <summary>
        /// 触发类型
        /// </summary>
        public BuffTriggerType          buffTriggerType;
        /// <summary>
        /// 触发信息
        /// </summary>
        public BuffTriggerConfig        buffTriggerConfig;
        /// <summary>
        /// 附加属性
        /// </summary>
        public List<BuffProp>           buffPropList                = new List<BuffProp>();
        // 即时影响类型
        public List<DamageInfBaseCSV>   immediateDamage             = new List<DamageInfBaseCSV>();
        // 脉冲影响类型
        public List<DamageInfBaseCSV>   pulseDamage                 = new List<DamageInfBaseCSV>();

        /// <summary>
        /// 仇恨值
        /// </summary>
        public  int                     hatredValue                 = 0;

        /// <summary>
        /// 特殊属性
        /// </summary>
        public SpecialFunctionType      specialFun                  = SpecialFunctionType.None;
        public SpecialFunConfig         specialFunConfig;
    }

}