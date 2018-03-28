using System;
using System.Collections.Generic;
namespace Games
{
    public class AttributePackConfig
    {
        /** 编号 */
        public int      id;
        /// <summary>
        /// 生命值
        /// </summary>
        public  int     hp;
        /// <summary>
        /// 最大生命
        /// </summary>
        public  int     hpMax;
        /// <summary>
        /// 伤害
        /// </summary>
        public  int     damage;
        /// <summary>
        /// 物理防御
        /// </summary>
        public  int     physicalDefence;
        /// <summary>
        /// 魔法防御
        /// </summary>
        public  int     magicDefence;
        /// <summary>
        /// 物理攻击
        /// </summary>
        public  int     physicalAttack;
        /// <summary>
        /// 魔法攻击
        /// </summary>
        public  int     magicAttack;
        /// <summary>
        /// 生命回复
        /// </summary>
        public  float   hpRecover;
        /// <summary>
        /// 能量回复
        /// </summary>
        public  float   energyRecover;
        /// <summary>
        /// 雷达半径
        /// </summary>
        public  int     radarRadius;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public  int     attackRadius;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public  float    attackSpeed;
        /// <summary>
        /// 移动速度
        /// </summary>
        public  int     movementSpeed;
    }
}
