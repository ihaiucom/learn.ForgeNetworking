namespace Games.Wars
{
    public class AttackRule
    {
        #region 攻击规则
        /// <summary>
        /// 攻击对象和自己的关系
        /// </summary>
        public  RelationType    relationType            = RelationType.Enemy;
        /// <summary>
        /// 对方单位类型
        /// </summary>
        public  UnitType        unitType                = UnitType.All;
        /// <summary>
        /// 对方建筑类型
        /// </summary>
        public UnitBuildType    unitBuildType           = UnitBuildType.All;
        /// <summary>
        /// 空间类型
        /// </summary>
        public  UnitSpaceType   unitSpaceType           = UnitSpaceType.All;
        /// <summary>
        /// 攻击规则 
        /// </summary>
        public  WarSkillRule    warSkillRule            = WarSkillRule.RandomRange;
        #endregion
    }
}
