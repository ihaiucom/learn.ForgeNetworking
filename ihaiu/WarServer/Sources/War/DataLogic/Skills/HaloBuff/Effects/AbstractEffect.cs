using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 6:19:18 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 抽象效果
    /// </summary>
    public class AbstractEffect
    {
        public WarRoom room;

        public WarLTime Time
        {
            get
            {
                return room.LTime;
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public string identifier;



        /** 创建者 */
        public UnitData caster;

        /** 拥有者 */
        public UnitData unit;

        /** 配置 */
        public EffectConfig config;


        /** 设置配置 */
        virtual public AbstractEffect SetConfig(EffectConfig config)
        {
            this.config = config;
            return this;
        }


        /** 启动 */
        virtual public void Start()
        {
            Start(unit, caster);
        }

        virtual public void Start(UnitData unit, UnitData caster)
        {
        }

        /** 停止 */
        virtual public void Stop()
        {
        }

        /** 脉冲 */
        virtual public void Pulse(UnitData unit, UnitData caster)
        {
        }

        virtual public void Pulse()
        {
            Pulse(unit, caster);
        }

        virtual public void Pulse(List<UnitData> unitList, UnitData caster)
        {
        }

    }
}
