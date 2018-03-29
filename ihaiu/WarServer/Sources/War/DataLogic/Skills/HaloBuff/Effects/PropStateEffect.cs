using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:59:03 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 属性状态效果
    /// </summary>
    public class PropStateEffect : Effect
    {
        public PropStateEffectConfig mconfig;

        public override AbstractEffect SetConfig(EffectConfig config)
        {
            mconfig = (PropStateEffectConfig)config;
            return base.SetConfig(config);
        }

        /** 启动 */
        protected override void OnStart()
        {
            base.OnStart();

            unit.prop.AppProps(mconfig.attachPropData, true);
        }

        /** 停止 */
        protected override void OnStop()
        {
            base.OnStop();

            if (unit != null) unit.prop.RevokeProps(mconfig.attachPropData, true);
        }
    }
}
