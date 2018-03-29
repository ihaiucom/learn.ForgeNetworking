using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 技能数据 UnitControlSkillData - UnitControl 调用
    /// </summary>
    public class SkillDate
    {
        /// <summary>
        /// 攻击发起者
        /// </summary>
        public UnitAgent            unitAgentSend;
        /// <summary>
        /// 受击者
        /// </summary>
        public UnitData             unitDataBy;
        /// <summary>
        /// 当前技能
        /// </summary>
        //public SkillBass            skillBass           = null;
        /// <summary>
        /// 当前是否普攻
        /// </summary>
        public bool                 isNormalSkill       = false;
        /// <summary>
        /// 技能朝向,没有攻击目标时适用
        /// </summary>
        public Quaternion           quaternion;
        /// <summary>
        /// 技能攻击点
        /// </summary>
        public Vector3              skillAttackPoint;
        /// <summary>
        /// 是否自动搜索目标，针对自动释放的英雄技能
        /// </summary>
        public bool                 bAuToSearchTarget   = false;
        /// <summary>
        /// 攻击前确定方向
        /// </summary>
        //public Vector3              lookAtPos           = Vector3.zero;

        public void OnSetValue(UnitAgent unitAgentSend, UnitData unitDataBy, int skillIndexOf, Quaternion quaternion, Vector3 skillAttackPoint, bool bAuToSearchTarget = false)
        {
            this.unitAgentSend = unitAgentSend;
            this.unitDataBy = unitDataBy;
            //SkillController skillController = unitAgentSend.unitData.GetSkillByIndex(skillIndexOf);
            //if (skillController != null)
            //{
            //    this.skillBass = skillController.skillBass;
            //}
            if (skillIndexOf == 0)
            {
                isNormalSkill = true;
            }
            else
            {
                isNormalSkill = false;
            }
            this.quaternion = quaternion;
            this.skillAttackPoint = skillAttackPoint;
            this.bAuToSearchTarget = bAuToSearchTarget;
            if (bAuToSearchTarget)
            {
                this.skillAttackPoint = new Vector3(-100001, 0);
            }
        }

        public void OnSetValue(UnitAgent unitAgentSend, UnitData unitDataBy, int skillId)
        {
            this.unitAgentSend = unitAgentSend;
            this.unitDataBy = unitDataBy;
            //SkillController skillController = unitAgentSend.unitData.GetSkill(skillId);
            //this.skillBass = skillController.skillBass;
            this.quaternion = unitAgentSend.rotationQuaternion;
            if (unitDataBy != null)
            {
                this.skillAttackPoint = unitDataBy.AnchorAttackbyPos;
            }
        }

    }
}