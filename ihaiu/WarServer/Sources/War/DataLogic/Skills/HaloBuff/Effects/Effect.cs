using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:45:02 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    public class Effect : AbstractEffect
    {
        public bool isStarted = false;

        /** 启动 */
        override public void Start(UnitData unit, UnitData caster)
        {
            if (isStarted)
                return;

            this.unit = unit;
            this.caster = caster;
            isStarted = true;
            OnStart();
        }

        virtual protected void OnStart()
        {

        }

        /** 停止 */
        override public void Stop()
        {
            if (isStarted)
            {
                OnStop();
            }
            isStarted = false;
        }

        virtual protected void OnStop()
        {

        }


        /** 脉冲 */
        override public void Pulse(UnitData unit, UnitData caster)
        {
            this.unit = unit;
            this.caster = caster;

            OnPulse();
        }


        virtual protected void OnPulse()
        {
        }
    }
}
