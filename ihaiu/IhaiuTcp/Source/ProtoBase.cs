using BeardedManStudios.Forge.Networking;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 10:40:21 AM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    public abstract class ProtoBase
    {
        protected HBaseTCP baseTcp;
        public ProtoBase(HBaseTCP baseTcp)
        {
            this.baseTcp = baseTcp;
        }

        public void OnMessage(ProtoMsg msg, NetworkingPlayer player)
        {
           
        }
    }
}
