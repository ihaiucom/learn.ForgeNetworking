using System;
using System.Collections.Generic;
using Games.Wars;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 5:37:14 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 单位等级 */
    public class UnitLevelConfig
    {
        /** 单位等级ID */
        public int      levelId;
        /** 名称 */
        public string   name;
        /** 品质 */
        public int      quality;
        /** 等级 */
        public int      level;

        /** 升级需要碎片物品ID */
        public int      pieceId;
        /** 升级需要碎片物品数量 */
        public int      pieceNum;
        /** 升级需要货币物品ID */
        public int      money;
        /** 升级需要货币物品数量 */
        public int      moneyNum;
        /** 获得水晶经验 */
        public int      cystalExp;
        /// <summary>
        /// 建造费用
        /// </summary>
        public int              buildCost;

        /// <summary>
        /// 建造CD 
        /// </summary>
        public int              buildCd;

        /// <summary>
        /// 修理费用
        /// </summary>
        public int              rebuildCost;

        /// <summary>
        /// 占领费用
        /// </summary>
        public int              occupyCost;

        /// <summary>
        /// 雇佣费用
        /// </summary>
        public int              employCost;

		/// <summary>
		/// 怪物死亡获得
		/// </summary>
		public int              deathCost;

        /// <summary>
        /// 雇佣结束条1：使用时间2：攻击次数
        /// </summary>
        public UnityEmployType  employType;

        /// <summary>
        /// 雇佣参数
        /// </summary>
        public float            employArg;

        /// <summary>
        /// 士兵技能AI列表
        /// </summary>
        public List<int> aiSoliders = new List<int>();

        /// <summary>
        /// 英雄ai列表
        /// 第一个为pvp天梯ai
        /// </summary>
        public List<int> aiHeros    = new List<int>();


        /** 技能列表 */
        public List<WarEnterSkillData>    skillList = new List<WarEnterSkillData>();

        /** 属性列表 */
        public List<Prop> propList = new List<Prop>();

        public Dictionary<int, Prop> propDict = new Dictionary<int, Prop>(); 

        public float GetPropVal(int propId)
        {
            if (propDict.ContainsKey(propId))
            {
                return propDict[propId].Val;
            }
            return 0;
        }

        public float hp
        {
            get
            {
              
                return GetPropVal(PropId.Hp);
            }
        }

        private PropAttachData _protoAttachData;
        public PropAttachData protoAttachData
        {
            get
            {
                if(_protoAttachData == null)
                {
                    _protoAttachData = new PropAttachData(propList);
                }
                return _protoAttachData;
            }
        }

    }
}
