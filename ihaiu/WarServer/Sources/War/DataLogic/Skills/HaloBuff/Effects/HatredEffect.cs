using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 8:17:43 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 仇恨效果
    /// </summary>
    public class HatredEffect : Effect
    {

        public HatredEffectConfig mconfig;


        public override AbstractEffect SetConfig(EffectConfig config)
        {
            mconfig = (HatredEffectConfig)config;

            return base.SetConfig(config);
        }

        /** 启动 */
        protected override void OnStart()
        {
            unit.AddHatred(caster.uid, mconfig.hatred);
            base.OnStart();
        }



        protected override void OnStop()
        {
            base.OnStop();
            unit.AddHatred(caster.uid, -mconfig.hatred);
        }
    }
}
