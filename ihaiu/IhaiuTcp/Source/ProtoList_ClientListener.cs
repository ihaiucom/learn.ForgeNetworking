using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 3:33:37 PM
*  @Description:    
* ==============================================================================
*/
namespace ihaiu
{
    /// <summary>
    /// 客户端注册 监听协议
    /// </summary>
    public partial class ProtoList
    {
        public void ClientRegisteredListener()
        {
            ServerRegisteredSender();
        }
    }
}
