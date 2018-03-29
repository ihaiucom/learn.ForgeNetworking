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
    /// 伤害效果
    /// </summary>
    public class DamageEffect : Effect
    {

        public DamageEffectConfig mconfig;


        public override AbstractEffect SetConfig(EffectConfig config)
        {
            mconfig = (DamageEffectConfig)config;

            return base.SetConfig(config);
        }

        /** 启动 */
        protected override void OnStart()
        {

            DamageData damage = new DamageData();
            damage.AttackSend = caster;
            //damage.AttakcVal = mconfig.damage;
            damage.damageType = mconfig.damageType;
            //damage.DamagePer = mconfig.damageper;
            damage.bSkillDamage = false;
            damage.damageInfBaseCSV = mconfig.damageInfBaseCSV;
            if (!unit.isCloneUnit && !room.sceneData.CheckUnitInSafeRegion(unit))
            {
                unit.OnTakeDamage(damage);
            }
            base.OnStart();
        }


        protected override void OnPulse()
        {
            base.OnPulse();
            DamageData damage = new DamageData();
            damage.AttackSend = caster;
            //damage.AttakcVal = mconfig.damage;
            damage.damageType = mconfig.damageType;
            //damage.DamagePer = mconfig.damageper;
            damage.bSkillDamage = false;
            damage.damageInfBaseCSV = mconfig.damageInfBaseCSV;
            if (!unit.isCloneUnit && !room.sceneData.CheckUnitInSafeRegion(unit))
            {
                unit.OnTakeDamage(damage);
            }
        }

        public override void Pulse(List<UnitData> unitList, UnitData caster)
        {
            base.Pulse(unitList, caster);
            foreach(UnitData unit in unitList)
            {
                Pulse(unit, caster);
            }
        }
    }
}
