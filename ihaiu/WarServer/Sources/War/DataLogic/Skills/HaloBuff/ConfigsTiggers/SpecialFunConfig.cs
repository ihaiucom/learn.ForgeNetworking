using FMODUnity;
using System;
using UnityEngine;
namespace Games.Wars
{
    [Serializable]
    public class SpecialFunConfig
    {
        /// <summary>
        /// 特殊类型
        /// </summary>
        public SpecialFunctionType          specialFun;

        public  static SpecialFunConfig CreateConfig(SpecialFunctionType specialFun)
        {
            SpecialFunConfig config = null;
            switch (specialFun)
            {
                case SpecialFunctionType.Crit:
                    config = new SpecialFunConfigCrit();
                    break;
                default:
                    config = new SpecialFunConfig();
                    break;
            }
            config.specialFun = specialFun;
            return config;
        }

    }
}