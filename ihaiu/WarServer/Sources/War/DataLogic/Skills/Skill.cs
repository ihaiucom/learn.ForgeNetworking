using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 技能基本类
    /// </summary>
    [Serializable]
    public class SkillBass
    {
        #region 基本信息
        public  int                                     skillLv                     = 1;
        public  int                                     legionId                    = -1;
        /// <summary>
        /// 技能id
        /// </summary>
        public  int                                     skillId                     = 0;
        /// <summary>
        /// 前置技能id
        /// </summary>
        public  int                                     preSkillId                  = 0;
        /// <summary>
        /// 技能名称
        /// </summary>
        public  string                                  skillName                   = "";
        /// <summary>
        /// 技能icon
        /// </summary>
        public  string                                  skillIcon                   = "";
        /// <summary>
        /// 后缀
        /// </summary>
        public  string                                  skillIconsuf                = "";
        /// <summary>
        /// 技能描述
        /// </summary>
        public  string                                  skillDes                    = "";
        /// <summary>
        /// 技能优先级
        /// </summary>
        public  int                                     skillPriority               = 0;
        /// <summary>
        /// 技能cd
        /// </summary>
        public  float                                   skillCD                     = 0;
        #endregion
        /// <summary>
        /// 激活类型
        /// </summary>
        public  SkillActivation                         skillActivation             = null;
        #region 主动技能需求
        /// <summary>
        /// 施法方向选择
        /// </summary>
        public  List<AimDirection>                      aimDirectionList            = new List<AimDirection>();
        #endregion
        #region 被动技能需求
        /// <summary>
        /// 触发条件
        /// </summary>
        public  List<PassiveTriggercondition>          passiveTriggerList           = new List<PassiveTriggercondition>();
        #endregion
        /// <summary>
        /// 技能事件
        /// </summary>
        public  ActionEvent                             actionEvent                 = new ActionEvent();
    }
    
    
}