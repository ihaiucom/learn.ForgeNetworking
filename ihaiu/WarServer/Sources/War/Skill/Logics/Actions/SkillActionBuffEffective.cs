using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为实现 -- 创建buff
    /// </summary>
    public class SkillActionBuffEffective : SkillAction
    {
        private SkillActionConfigBuffCreate actionConfig;
        public override void SetConfig(SkillActionConfig config)
        {
            base.SetConfig(config);
            actionConfig = (SkillActionConfigBuffCreate)config.config;
            endType = StoryActionEndType.Call;
        }

        protected override void OnStart()
        {
            if (actionConfig.buffId > 0)
            {
                if (warSkill.actionUnitAgent.unitData.skillAddBuffDic.ContainsKey(warSkill.skillInfoConfig.skillId))
                {
                    Dictionary<int, SkillActionConfigBuffCreate> dicInfo = warSkill.actionUnitAgent.unitData.skillAddBuffDic[warSkill.skillInfoConfig.skillId];
                    if (dicInfo.Count > 0)
                    {
                        int[] keys = new int[dicInfo.Count];
                        dicInfo.Keys.CopyTo(keys, 0);
                        for (int i = 0; i < dicInfo.Count; i++)
                        {
                            dicInfo[keys[i]] = actionConfig;
                        }
                    }

                }
            }
            End();
        }

    }
}