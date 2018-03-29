using System.Collections.Generic;

namespace Games.Wars
{
    /// <summary>
    /// 技能携带buff
    /// </summary>
    public class PassiveJudgmentTrigerOnSkill : PassiveJudgment
    {
        // 携带buff的技能列表
        public List<int>    skillIdList             = new List<int>();
    }
}