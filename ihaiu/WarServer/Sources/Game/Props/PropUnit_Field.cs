
using System;
using System.Collections.Generic;

namespace Games
{
    public partial class PropUnit
    {

 		// CN:能量值
 		// EN:energy
 		// Tip:建造机关消耗的战斗资源
 		// Field:energy

            public float Energy
            {
                get
                {
                        return finals[PropId.Energy];
                 }

                set
                {
                        finals[PropId.Energy] = value;
                }
            }
            

 		// CN:能量最大值
 		// EN:energy_max
 		// Tip:能量值无法超过能量最大值
 		// Field:energy_max

            public float EnergyMax
            {
                get
                {
                        return finals[PropId.EnergyMax];
                 }

                set
                {
                        finals[PropId.EnergyMax] = value;
                }
            }
            

 		// CN:生命值
 		// EN:hp
 		// Tip:受到伤害时扣减生命值，当生命值为0时，人物就会死亡
 		// Field:hp

            public float Hp
            {
                get
                {
                        return finals[PropId.Hp];
                 }

                set
                {
                        finals[PropId.Hp] = value;
                }
            }
            

 		// CN:生命最大值
 		// EN:hp_max
 		// Tip:生命值无法超过生命最大值
 		// Field:hp_max

            public float HpMax
            {
                get
                {
                        return finals[PropId.HpMax];
                 }

                set
                {
                        finals[PropId.HpMax] = value;
                }
            }
            

 		// CN:护盾
 		// EN:shield
 		// Tip:受到伤害时代替生命值承受伤害，被打破或持续时间结束均失效
 		// Field:shield

            public float Shield
            {
                get
                {
                        return finals[PropId.Shield];
                 }

                set
                {
                        finals[PropId.Shield] = value;
                }
            }
            

 		// CN:物理攻击
 		// EN:physical_attack
 		// Tip:造成的物理伤害的基准值（结算减伤前）
 		// Field:physical_attack

            public float PhysicalAttack
            {
                get
                {
                        return finals[PropId.PhysicalAttack];
                 }

                set
                {
                        finals[PropId.PhysicalAttack] = value;
                }
            }
            

 		// CN:魔法攻击
 		// EN:magic_attack
 		// Tip:造成的魔法伤害的基准值（结算减伤前）
 		// Field:magic_attack

            public float MagicAttack
            {
                get
                {
                        return finals[PropId.MagicAttack];
                 }

                set
                {
                        finals[PropId.MagicAttack] = value;
                }
            }
            

 		// CN:物理防御
 		// EN:physical_defence
 		// Tip:人物减免物理攻击伤害的能力（减伤公式待定）
 		// Field:physical_defence

            public float PhysicalDefence
            {
                get
                {
                        return finals[PropId.PhysicalDefence];
                 }

                set
                {
                        finals[PropId.PhysicalDefence] = value;
                }
            }
            

 		// CN:魔法防御
 		// EN:magic_defence
 		// Tip:人物减免魔法攻击伤害的能力（减伤公式待定）
 		// Field:magic_defence

            public float MagicDefence
            {
                get
                {
                        return finals[PropId.MagicDefence];
                 }

                set
                {
                        finals[PropId.MagicDefence] = value;
                }
            }
            

 		// CN:雷达半径
 		// EN:radar_radius
 		// Tip:AI检测敌人半径
 		// Field:radar_radius

            public float RadarRadius
            {
                get
                {
                        return finals[PropId.RadarRadius];
                 }

                set
                {
                        finals[PropId.RadarRadius] = value;
                }
            }
            

 		// CN:射程
 		// EN:attack_radius
 		// Tip:人物的最大有效攻击距离
 		// Field:attack_radius

            public float AttackRadius
            {
                get
                {
                        return finals[PropId.AttackRadius];
                 }

                set
                {
                        finals[PropId.AttackRadius] = value;
                }
            }
            

 		// CN:攻速
 		// EN:attack_speed
 		// Tip:用于加减攻击间隔百分比
 		// Field:attack_speed

            public float AttackSpeed
            {
                get
                {
                        return finals[PropId.AttackSpeed];
                 }

                set
                {
                        finals[PropId.AttackSpeed] = value;
                }
            }
            

 		// CN:移动速度
 		// EN:movement_speed
 		// Tip:每秒移动距离
 		// Field:movement_speed

            public float MovementSpeed
            {
                get
                {
                        return finals[PropId.MovementSpeed];
                 }

                set
                {
                        finals[PropId.MovementSpeed] = value;
                }
            }
            

 		// CN:伤害
 		// EN:Damage
 		// Tip:伤害
 		// Field:damage

        public bool Damage
        {
            get
            {
                return finals[PropId.Damage] > 0;
            }
            set
            {
                finals[PropId.Damage] = value ? 1 : 0;
                bool _b = finals[PropId.Damage] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.Damage, _b);
            }
        }
            

 		// CN:冰冻
 		// EN:Freezed
 		// Tip:不可以移动、攻击
 		// Field:state_freezed

        public bool StateFreezed
        {
            get
            {
                return finals[PropId.StateFreezed] > 0;
            }
            set
            {
                finals[PropId.StateFreezed] = value ? 1 : 0;
                bool _b = finals[PropId.StateFreezed] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StateFreezed, _b);
            }
        }
            

 		// CN:眩晕
 		// EN:Vertigo
 		// Tip:不可以移动、攻击
 		// Field:state_vertigo

        public bool StateVertigo
        {
            get
            {
                return finals[PropId.StateVertigo] > 0;
            }
            set
            {
                finals[PropId.StateVertigo] = value ? 1 : 0;
                bool _b = finals[PropId.StateVertigo] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StateVertigo, _b);
            }
        }
            

 		// CN:沉默
 		// EN:Silence
 		// Tip:不可以放技能
 		// Field:state_silence

        public bool StateSilence
        {
            get
            {
                return finals[PropId.StateSilence] > 0;
            }
            set
            {
                finals[PropId.StateSilence] = value ? 1 : 0;
                bool _b = finals[PropId.StateSilence] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StateSilence, _b);
            }
        }
            

 		// CN:中毒
 		// EN:Posion
 		// Tip:中毒
 		// Field:state_posion

        public bool StatePosion
        {
            get
            {
                return finals[PropId.StatePosion] > 0;
            }
            set
            {
                finals[PropId.StatePosion] = value ? 1 : 0;
                bool _b = finals[PropId.StatePosion] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StatePosion, _b);
            }
        }
            

 		// CN:灼烧
 		// EN:Burn
 		// Tip:灼烧
 		// Field:state_burn

        public bool StateBurn
        {
            get
            {
                return finals[PropId.StateBurn] > 0;
            }
            set
            {
                finals[PropId.StateBurn] = value ? 1 : 0;
                bool _b = finals[PropId.StateBurn] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StateBurn, _b);
            }
        }
            

 		// CN:生命回复
 		// EN:hp_recover
 		// Tip:每秒回复的生命值
 		// Field:hp_recover

            public float HpRecover
            {
                get
                {
                        return finals[PropId.HpRecover];
                 }

                set
                {
                        finals[PropId.HpRecover] = value;
                }
            }
            

 		// CN:能量回复
 		// EN:energy_recover
 		// Tip:每秒回复的能量值
 		// Field:energy_recover

            public float EnergyRecover
            {
                get
                {
                        return finals[PropId.EnergyRecover];
                 }

                set
                {
                        finals[PropId.EnergyRecover] = value;
                }
            }
            

 		// CN:攻击间隔
 		// EN:AttackInterval
 		// Tip:攻击间隔
 		// Field:attack_interval

            public float AttackInterval
            {
                get
                {
                        return finals[PropId.AttackInterval];
                 }

                set
                {
                        finals[PropId.AttackInterval] = value;
                }
            }
            

 		// CN:状态生命回复
 		// EN:State HP Recover
 		// Tip:状态生命回复
 		// Field:state_hprecover

        public bool StateHPRecover
        {
            get
            {
                return finals[PropId.StateHPRecover] > 0;
            }
            set
            {
                finals[PropId.StateHPRecover] = value ? 1 : 0;
                bool _b = finals[PropId.StateHPRecover] == 1 ? true : false;
               // UIHandler.OnPropUnitStateed(PropId.StateHPRecover, _b);
            }
        }
            

 		// CN:韧性
 		// EN:Toughness
 		// Tip:获得治疗/护盾效果时额外回复生命
 		// Field:Toughness

            public float Toughness
            {
                get
                {
                        return finals[PropId.Toughness];
                 }

                set
                {
                        finals[PropId.Toughness] = value;
                }
            }
            

 		// CN:格挡
 		// EN:Defense
 		// Tip:最初的部分生命被替换为护盾
 		// Field:Defense

            public float Defense
            {
                get
                {
                        return finals[PropId.Defense];
                 }

                set
                {
                        finals[PropId.Defense] = value;
                }
            }
            

 		// CN:暴击
 		// EN:Critical
 		// Tip:有概率造成2倍伤害
 		// Field:Critical

            public float Critical
            {
                get
                {
                        return finals[PropId.Critical];
                 }

                set
                {
                        finals[PropId.Critical] = value;
                }
            }
            

 		// CN:急速
 		// EN:CoolDown
 		// Tip:加快技能冷却
 		// Field:CoolDown

            public float CoolDown
            {
                get
                {
                        return finals[PropId.CoolDown];
                 }

                set
                {
                        finals[PropId.CoolDown] = value;
                }
            }
            

 		// CN:鼓舞
 		// EN:Encourage
 		// Tip:治疗英雄时同时为其回复部分能量
 		// Field:Encourage

            public float Encourage
            {
                get
                {
                        return finals[PropId.Encourage];
                 }

                set
                {
                        finals[PropId.Encourage] = value;
                }
            }
            

 		// CN:破甲
 		// EN:Piercing
 		// Tip:对有护盾的目标造成额外伤害
 		// Field:Piercing

            public float Piercing
            {
                get
                {
                        return finals[PropId.Piercing];
                 }

                set
                {
                        finals[PropId.Piercing] = value;
                }
            }
            

 		// CN:格挡减免%
 		// EN:BlockRatio
 		// Tip:格挡时减免的伤害比例
 		// Field:BlockRatio

            public float BlockRatio
            {
                get
                {
                        return finals[PropId.BlockRatio];
                 }

                set
                {
                        finals[PropId.BlockRatio] = value;
                }
            }
            

 		// CN:暴击增幅%
 		// EN:CriRatio
 		// Tip:暴击时附加的伤害比例
 		// Field:CriRatio

            public float CriRatio
            {
                get
                {
                        return finals[PropId.CriRatio];
                 }

                set
                {
                        finals[PropId.CriRatio] = value;
                }
            }
            

 		// CN:生命%
 		// EN:HpPercent
 		// Tip:按比例提高生命
 		// Field:HpPercent

            public float HpPercent
            {
                get
                {
                        return finals[PropId.HpPercent];
                 }

                set
                {
                        finals[PropId.HpPercent] = value;
                }
            }
            

 		// CN:伤害%
 		// EN:PhysicalAttackPer
 		// Tip:按比例提高伤害
 		// Field:PhysicalAttackPer

            public float PhysicalAttackPer
            {
                get
                {
                        return finals[PropId.PhysicalAttackPer];
                 }

                set
                {
                        finals[PropId.PhysicalAttackPer] = value;
                }
            }
            

 		// CN:治疗%
 		// EN:MagicAttackPer
 		// Tip:按比例提高治疗
 		// Field:MagicAttackPer

            public float MagicAttackPer
            {
                get
                {
                        return finals[PropId.MagicAttackPer];
                 }

                set
                {
                        finals[PropId.MagicAttackPer] = value;
                }
            }
            

 		// CN:伤害加成%
 		// EN:DamageIncrease
 		// Tip:按比例提高造成的伤害
 		// Field:DamageIncrease

            public float DamageIncrease
            {
                get
                {
                        return finals[PropId.DamageIncrease];
                 }

                set
                {
                        finals[PropId.DamageIncrease] = value;
                }
            }
            

 		// CN:伤害减免%
 		// EN:DamageDecrease
 		// Tip:按比例降低受到的伤害
 		// Field:DamageDecrease

            public float DamageDecrease
            {
                get
                {
                        return finals[PropId.DamageDecrease];
                 }

                set
                {
                        finals[PropId.DamageDecrease] = value;
                }
            }
            



    }
}


