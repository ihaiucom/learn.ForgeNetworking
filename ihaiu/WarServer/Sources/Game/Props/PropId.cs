// ======================================
// 该文件自动生成,，不要修改，否则会替换
// 默认Menu: Game/Tools/Generate Csharp PropId And PropField
// auth: 曾峰
// email:zengfeng75@qq.com
// qq: 593705098
// http://blog.ihaiu.com
// --------------------------------------			





using System;
using System.Collections.Generic;
namespace Games
{
    public partial class PropId
    {

 		// CN:能量值
 		// EN:energy
 		// Tip:建造机关消耗的战斗资源
 		// Field:energy
 		public const int Energy = 1; 


 		// CN:能量最大值
 		// EN:energy_max
 		// Tip:能量值无法超过能量最大值
 		// Field:energy_max
 		public const int EnergyMax = 2; 


 		// CN:生命值
 		// EN:hp
 		// Tip:受到伤害时扣减生命值，当生命值为0时，人物就会死亡
 		// Field:hp
 		public const int Hp = 3; 


 		// CN:生命最大值
 		// EN:hp_max
 		// Tip:生命值无法超过生命最大值
 		// Field:hp_max
 		public const int HpMax = 4; 


 		// CN:护盾
 		// EN:shield
 		// Tip:受到伤害时代替生命值承受伤害，被打破或持续时间结束均失效
 		// Field:shield
 		public const int Shield = 5; 


 		// CN:物理攻击
 		// EN:physical_attack
 		// Tip:造成的物理伤害的基准值（结算减伤前）
 		// Field:physical_attack
 		public const int PhysicalAttack = 6; 


 		// CN:魔法攻击
 		// EN:magic_attack
 		// Tip:造成的魔法伤害的基准值（结算减伤前）
 		// Field:magic_attack
 		public const int MagicAttack = 7; 


 		// CN:物理防御
 		// EN:physical_defence
 		// Tip:人物减免物理攻击伤害的能力（减伤公式待定）
 		// Field:physical_defence
 		public const int PhysicalDefence = 8; 


 		// CN:魔法防御
 		// EN:magic_defence
 		// Tip:人物减免魔法攻击伤害的能力（减伤公式待定）
 		// Field:magic_defence
 		public const int MagicDefence = 9; 


 		// CN:雷达半径
 		// EN:radar_radius
 		// Tip:AI检测敌人半径
 		// Field:radar_radius
 		public const int RadarRadius = 10; 


 		// CN:射程
 		// EN:attack_radius
 		// Tip:人物的最大有效攻击距离
 		// Field:attack_radius
 		public const int AttackRadius = 11; 


 		// CN:攻速
 		// EN:attack_speed
 		// Tip:用于加减攻击间隔百分比
 		// Field:attack_speed
 		public const int AttackSpeed = 12; 


 		// CN:移动速度
 		// EN:movement_speed
 		// Tip:每秒移动距离
 		// Field:movement_speed
 		public const int MovementSpeed = 13; 


 		// CN:伤害
 		// EN:Damage
 		// Tip:伤害
 		// Field:damage
 		public const int Damage = 14; 


 		// CN:冰冻
 		// EN:Freezed
 		// Tip:不可以移动、攻击
 		// Field:state_freezed
 		public const int StateFreezed = 15; 


 		// CN:眩晕
 		// EN:Vertigo
 		// Tip:不可以移动、攻击
 		// Field:state_vertigo
 		public const int StateVertigo = 16; 


 		// CN:沉默
 		// EN:Silence
 		// Tip:不可以放技能
 		// Field:state_silence
 		public const int StateSilence = 17; 


 		// CN:中毒
 		// EN:Posion
 		// Tip:中毒
 		// Field:state_posion
 		public const int StatePosion = 18; 


 		// CN:灼烧
 		// EN:Burn
 		// Tip:灼烧
 		// Field:state_burn
 		public const int StateBurn = 19; 


 		// CN:生命回复
 		// EN:hp_recover
 		// Tip:每秒回复的生命值
 		// Field:hp_recover
 		public const int HpRecover = 20; 


 		// CN:能量回复
 		// EN:energy_recover
 		// Tip:每秒回复的能量值
 		// Field:energy_recover
 		public const int EnergyRecover = 21; 


 		// CN:攻击间隔
 		// EN:AttackInterval
 		// Tip:攻击间隔
 		// Field:attack_interval
 		public const int AttackInterval = 22; 


 		// CN:状态生命回复
 		// EN:State HP Recover
 		// Tip:状态生命回复
 		// Field:state_hprecover
 		public const int StateHPRecover = 23; 


 		// CN:韧性
 		// EN:Toughness
 		// Tip:获得治疗/护盾效果时额外回复生命
 		// Field:Toughness
 		public const int Toughness = 24; 


 		// CN:格挡
 		// EN:Defense
 		// Tip:最初的部分生命被替换为护盾
 		// Field:Defense
 		public const int Defense = 25; 


 		// CN:暴击
 		// EN:Critical
 		// Tip:有概率造成2倍伤害
 		// Field:Critical
 		public const int Critical = 26; 


 		// CN:急速
 		// EN:CoolDown
 		// Tip:加快技能冷却
 		// Field:CoolDown
 		public const int CoolDown = 27; 


 		// CN:鼓舞
 		// EN:Encourage
 		// Tip:治疗英雄时同时为其回复部分能量
 		// Field:Encourage
 		public const int Encourage = 28; 


 		// CN:破甲
 		// EN:Piercing
 		// Tip:对有护盾的目标造成额外伤害
 		// Field:Piercing
 		public const int Piercing = 29; 


 		// CN:格挡减免%
 		// EN:BlockRatio
 		// Tip:格挡时减免的伤害比例
 		// Field:BlockRatio
 		public const int BlockRatio = 30; 


 		// CN:暴击增幅%
 		// EN:CriRatio
 		// Tip:暴击时附加的伤害比例
 		// Field:CriRatio
 		public const int CriRatio = 31; 


 		// CN:生命%
 		// EN:HpPercent
 		// Tip:按比例提高生命
 		// Field:HpPercent
 		public const int HpPercent = 32; 


 		// CN:伤害%
 		// EN:PhysicalAttackPer
 		// Tip:按比例提高伤害
 		// Field:PhysicalAttackPer
 		public const int PhysicalAttackPer = 33; 


 		// CN:治疗%
 		// EN:MagicAttackPer
 		// Tip:按比例提高治疗
 		// Field:MagicAttackPer
 		public const int MagicAttackPer = 34; 


 		// CN:伤害加成%
 		// EN:DamageIncrease
 		// Tip:按比例提高造成的伤害
 		// Field:DamageIncrease
 		public const int DamageIncrease = 35; 


 		// CN:伤害减免%
 		// EN:DamageDecrease
 		// Tip:按比例降低受到的伤害
 		// Field:DamageDecrease
 		public const int DamageDecrease = 36; 


 		// 最大值 
 		public static int MAX = 37; 




    }
}

