using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:29:46 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 状态--属性效果
    /// </summary>
    public class PropStateEffectConfig : EffectConfig
    {
        public PropAttachData attachPropData;

        public PropStateEffectConfig()
        {
            effectType = EffectType.PropStateEffect;
        }

        public PropStateEffectConfig(PropAttachData attachPropData)
        {
            effectType = EffectType.PropStateEffect;
            this.attachPropData = attachPropData;
        }

        public PropStateEffectConfig(List<Prop> props)
        {
            effectType = EffectType.PropStateEffect;
            this.attachPropData = new PropAttachData(props);
        }


        public PropStateEffectConfig(Prop[] props)
        {
            effectType = EffectType.PropStateEffect;
            this.attachPropData = new PropAttachData(new List<Prop>(props));
        }
        public PropStateEffectConfig(int haloBuffId, List<BuffProp> BuffPropList)
        {
            effectType = EffectType.PropStateEffect;
            this.attachPropData = new PropAttachData(haloBuffId, BuffPropList);
        }
    }
}
