using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 具体触发条件
    /// </summary>
    public class PassiveJudgment
    {
        public  JudgmentTargger     judgmentTargger             = JudgmentTargger.None;

        public PassiveJudgment config { get; set; }

        public void SetConfig()
        {
            switch (judgmentTargger)
            {
                case JudgmentTargger.SameTypeCount:
                    config = new PassiveJudgmentSameTypeCount();
                    break;
                case JudgmentTargger.TargetType:
                    config = new PassiveJudgmentTargetType();
                    break;
                case JudgmentTargger.TargetDis:
                    config = new PassiveJudgmentTargetDis();
                    break;
                case JudgmentTargger.RandRangeCount:
                    config = new PassiveJudgmentRandRangeCount();
                    break;
                case JudgmentTargger.SelfBuffCount:
                    config = new PassiveJudgmentSelfBuffCount();
                    break;
                case JudgmentTargger.MonsterWaves:
                    config = new PassiveJudgmentMonsterWaves();
                    break;
                case JudgmentTargger.SelfProperties:
                    config = new PassiveJudgmentSelfProperties();
                    break;
                case JudgmentTargger.SkillAddBuff:
                    config = new PassiveJudgmentTrigerOnSkill();
                    break;
                case JudgmentTargger.ChangeNormalSkill:
                    config = new PassiveJudgmentChangeNormalSkill();
                    break;
                default:
                    config = new PassiveJudgment();
                    break;
            }
            config.judgmentTargger = judgmentTargger;
        }
    }
}