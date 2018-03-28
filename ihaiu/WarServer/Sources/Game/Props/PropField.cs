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
    public partial class PropField
    {

 		// CN:能量值
 		// EN:energy
 		// Tip:建造机关消耗的战斗资源
 		// ID:1
 		public static string Energy = "energy"; 
 		// CN:能量最大值
 		// EN:energy_max
 		// Tip:能量值无法超过能量最大值
 		// ID:2
 		public static string EnergyMax = "energy_max"; 
 		// CN:生命值
 		// EN:hp
 		// Tip:受到伤害时扣减生命值，当生命值为0时，人物就会死亡
 		// ID:3
 		public static string Hp = "hp"; 
 		// CN:生命最大值
 		// EN:hp_max
 		// Tip:生命值无法超过生命最大值
 		// ID:4
 		public static string HpMax = "hp_max"; 
 		// CN:护盾
 		// EN:shield
 		// Tip:受到伤害时代替生命值承受伤害，被打破或持续时间结束均失效
 		// ID:5
 		public static string Shield = "shield"; 
 		// CN:物理攻击
 		// EN:physical_attack
 		// Tip:造成的物理伤害的基准值（结算减伤前）
 		// ID:6
 		public static string PhysicalAttack = "physical_attack"; 
 		// CN:魔法攻击
 		// EN:magic_attack
 		// Tip:造成的魔法伤害的基准值（结算减伤前）
 		// ID:7
 		public static string MagicAttack = "magic_attack"; 
 		// CN:物理防御
 		// EN:physical_defence
 		// Tip:人物减免物理攻击伤害的能力（减伤公式待定）
 		// ID:8
 		public static string PhysicalDefence = "physical_defence"; 
 		// CN:魔法防御
 		// EN:magic_defence
 		// Tip:人物减免魔法攻击伤害的能力（减伤公式待定）
 		// ID:9
 		public static string MagicDefence = "magic_defence"; 
 		// CN:雷达半径
 		// EN:radar_radius
 		// Tip:AI检测敌人半径
 		// ID:10
 		public static string RadarRadius = "radar_radius"; 
 		// CN:射程
 		// EN:attack_radius
 		// Tip:人物的最大有效攻击距离
 		// ID:11
 		public static string AttackRadius = "attack_radius"; 
 		// CN:攻速
 		// EN:attack_speed
 		// Tip:用于加减攻击间隔百分比
 		// ID:12
 		public static string AttackSpeed = "attack_speed"; 
 		// CN:移动速度
 		// EN:movement_speed
 		// Tip:每秒移动距离
 		// ID:13
 		public static string MovementSpeed = "movement_speed"; 
 		// CN:伤害
 		// EN:Damage
 		// Tip:伤害
 		// ID:14
 		public static string Damage = "damage"; 
 		// CN:冰冻
 		// EN:Freezed
 		// Tip:不可以移动、攻击
 		// ID:15
 		public static string StateFreezed = "state_freezed"; 
 		// CN:眩晕
 		// EN:Vertigo
 		// Tip:不可以移动、攻击
 		// ID:16
 		public static string StateVertigo = "state_vertigo"; 
 		// CN:沉默
 		// EN:Silence
 		// Tip:不可以放技能
 		// ID:17
 		public static string StateSilence = "state_silence"; 
 		// CN:中毒
 		// EN:Posion
 		// Tip:中毒
 		// ID:18
 		public static string StatePosion = "state_posion"; 
 		// CN:灼烧
 		// EN:Burn
 		// Tip:灼烧
 		// ID:19
 		public static string StateBurn = "state_burn"; 
 		// CN:生命回复
 		// EN:hp_recover
 		// Tip:每秒回复的生命值
 		// ID:20
 		public static string HpRecover = "hp_recover"; 
 		// CN:能量回复
 		// EN:energy_recover
 		// Tip:每秒回复的能量值
 		// ID:21
 		public static string EnergyRecover = "energy_recover"; 
 		// CN:攻击间隔
 		// EN:AttackInterval
 		// Tip:攻击间隔
 		// ID:22
 		public static string AttackInterval = "attack_interval"; 
 		// CN:状态生命回复
 		// EN:State HP Recover
 		// Tip:状态生命回复
 		// ID:23
 		public static string StateHPRecover = "state_hprecover"; 
 		// CN:韧性
 		// EN:Toughness
 		// Tip:获得治疗/护盾效果时额外回复生命
 		// ID:24
 		public static string Toughness = "Toughness"; 
 		// CN:格挡
 		// EN:Defense
 		// Tip:最初的部分生命被替换为护盾
 		// ID:25
 		public static string Defense = "Defense"; 
 		// CN:暴击
 		// EN:Critical
 		// Tip:有概率造成2倍伤害
 		// ID:26
 		public static string Critical = "Critical"; 
 		// CN:急速
 		// EN:CoolDown
 		// Tip:加快技能冷却
 		// ID:27
 		public static string CoolDown = "CoolDown"; 
 		// CN:鼓舞
 		// EN:Encourage
 		// Tip:治疗英雄时同时为其回复部分能量
 		// ID:28
 		public static string Encourage = "Encourage"; 
 		// CN:破甲
 		// EN:Piercing
 		// Tip:对有护盾的目标造成额外伤害
 		// ID:29
 		public static string Piercing = "Piercing"; 
 		// CN:格挡减免%
 		// EN:BlockRatio
 		// Tip:格挡时减免的伤害比例
 		// ID:30
 		public static string BlockRatio = "BlockRatio"; 
 		// CN:暴击增幅%
 		// EN:CriRatio
 		// Tip:暴击时附加的伤害比例
 		// ID:31
 		public static string CriRatio = "CriRatio"; 
 		// CN:生命%
 		// EN:HpPercent
 		// Tip:按比例提高生命
 		// ID:32
 		public static string HpPercent = "HpPercent"; 
 		// CN:伤害%
 		// EN:PhysicalAttackPer
 		// Tip:按比例提高伤害
 		// ID:33
 		public static string PhysicalAttackPer = "PhysicalAttackPer"; 
 		// CN:治疗%
 		// EN:MagicAttackPer
 		// Tip:按比例提高治疗
 		// ID:34
 		public static string MagicAttackPer = "MagicAttackPer"; 
 		// CN:伤害加成%
 		// EN:DamageIncrease
 		// Tip:按比例提高造成的伤害
 		// ID:35
 		public static string DamageIncrease = "DamageIncrease"; 
 		// CN:伤害减免%
 		// EN:DamageDecrease
 		// Tip:按比例降低受到的伤害
 		// ID:36
 		public static string DamageDecrease = "DamageDecrease"; 



        /** 字段列表 */
        private static List<string> list;
        public static List<string> List
        {
            get
            {
                if(list == null)
                {
                    list = new List<string>();

                   list.Add( Energy );
                   list.Add( EnergyMax );
                   list.Add( Hp );
                   list.Add( HpMax );
                   list.Add( Shield );
                   list.Add( PhysicalAttack );
                   list.Add( MagicAttack );
                   list.Add( PhysicalDefence );
                   list.Add( MagicDefence );
                   list.Add( RadarRadius );
                   list.Add( AttackRadius );
                   list.Add( AttackSpeed );
                   list.Add( MovementSpeed );
                   list.Add( Damage );
                   list.Add( StateFreezed );
                   list.Add( StateVertigo );
                   list.Add( StateSilence );
                   list.Add( StatePosion );
                   list.Add( StateBurn );
                   list.Add( HpRecover );
                   list.Add( EnergyRecover );
                   list.Add( AttackInterval );
                   list.Add( StateHPRecover );
                   list.Add( Toughness );
                   list.Add( Defense );
                   list.Add( Critical );
                   list.Add( CoolDown );
                   list.Add( Encourage );
                   list.Add( Piercing );
                   list.Add( BlockRatio );
                   list.Add( CriRatio );
                   list.Add( HpPercent );
                   list.Add( PhysicalAttackPer );
                   list.Add( MagicAttackPer );
                   list.Add( DamageIncrease );
                   list.Add( DamageDecrease );

                }
                return list;
            }
        }



        /**  ID字典 */
        private static Dictionary<string, int> idDict;
        public static Dictionary<string, int> IdDict
        {
            get
            {
                if(idDict == null)
                {
                    idDict = new Dictionary<string, int>();

                  idDict.Add( Energy, PropId.Energy );
                  idDict.Add( EnergyMax, PropId.EnergyMax );
                  idDict.Add( Hp, PropId.Hp );
                  idDict.Add( HpMax, PropId.HpMax );
                  idDict.Add( Shield, PropId.Shield );
                  idDict.Add( PhysicalAttack, PropId.PhysicalAttack );
                  idDict.Add( MagicAttack, PropId.MagicAttack );
                  idDict.Add( PhysicalDefence, PropId.PhysicalDefence );
                  idDict.Add( MagicDefence, PropId.MagicDefence );
                  idDict.Add( RadarRadius, PropId.RadarRadius );
                  idDict.Add( AttackRadius, PropId.AttackRadius );
                  idDict.Add( AttackSpeed, PropId.AttackSpeed );
                  idDict.Add( MovementSpeed, PropId.MovementSpeed );
                  idDict.Add( Damage, PropId.Damage );
                  idDict.Add( StateFreezed, PropId.StateFreezed );
                  idDict.Add( StateVertigo, PropId.StateVertigo );
                  idDict.Add( StateSilence, PropId.StateSilence );
                  idDict.Add( StatePosion, PropId.StatePosion );
                  idDict.Add( StateBurn, PropId.StateBurn );
                  idDict.Add( HpRecover, PropId.HpRecover );
                  idDict.Add( EnergyRecover, PropId.EnergyRecover );
                  idDict.Add( AttackInterval, PropId.AttackInterval );
                  idDict.Add( StateHPRecover, PropId.StateHPRecover );
                  idDict.Add( Toughness, PropId.Toughness );
                  idDict.Add( Defense, PropId.Defense );
                  idDict.Add( Critical, PropId.Critical );
                  idDict.Add( CoolDown, PropId.CoolDown );
                  idDict.Add( Encourage, PropId.Encourage );
                  idDict.Add( Piercing, PropId.Piercing );
                  idDict.Add( BlockRatio, PropId.BlockRatio );
                  idDict.Add( CriRatio, PropId.CriRatio );
                  idDict.Add( HpPercent, PropId.HpPercent );
                  idDict.Add( PhysicalAttackPer, PropId.PhysicalAttackPer );
                  idDict.Add( MagicAttackPer, PropId.MagicAttackPer );
                  idDict.Add( DamageIncrease, PropId.DamageIncrease );
                  idDict.Add( DamageDecrease, PropId.DamageDecrease );

        
                }
                return idDict;
            }
        }




        /** 获取属性ID，根据属性字段 */
        public static int GetPropId( string propField )
        {
            return IdDict[propField];
        }

    }
}

