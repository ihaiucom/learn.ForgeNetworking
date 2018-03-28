using com.ihaiu;
using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 7:59:43 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ConfigCsv("Config/Skill", true, false)]
    public class SkillConfigReader : ConfigReader<SkillConfig>
    {
        public override void ParseCsv(string[] csv)
        {

            SkillConfig config = new SkillConfig();
            config.skillId = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.icon = csv.GetString(GetHeadIndex("icon"));
            config.isPassive = csv.GetInt32(GetHeadIndex("skill_type")) == 2;
            config.passiveType = (SkillPassiveType)csv.GetInt32(GetHeadIndex("trigger_type"));
            config.castType = (SkillCastType)csv.GetInt32(GetHeadIndex("cast_type"));
            config.castRadius = csv.GetSingle(GetHeadIndex("cast_radius"));
            config.castLineWidth = csv.GetSingle(GetHeadIndex("cast_line_width"));
            config.castFanAngle = csv.GetSingle(GetHeadIndex("cast_fan_angle"));
            config.castCircleRadius = csv.GetSingle(GetHeadIndex("cast_circle_radius"));
            config.aiScoreUnitType = csv.GetInt32(GetHeadIndex("AIScoreUnitType"));
            config.aiScoreDistance = csv.GetInt32(GetHeadIndex("AIScoreDistance"));
            config.aiScoreAttackHatred = csv.GetInt32(GetHeadIndex("AIScoreAttackHatred"));
            config.aiScoreWeight = csv.GetInt32(GetHeadIndex("AIScoreWeight"));
            config.targetRelation = (RelationType) csv.GetInt32(GetHeadIndex("target_relation"));
            config.targetUnitType = (UnitType)csv.GetInt32(GetHeadIndex("target_unit_type"));
            config.targetBuildType = (UnitBuildType)csv.GetInt32(GetHeadIndex("target_build_type"));
            config.targetSoliderType = (UnitSoliderType)csv.GetInt32(GetHeadIndex("target_solider_type"));
            config.targetSpaceType = (UnitSpaceType)csv.GetInt32(GetHeadIndex("space_type"));
            config.targetProfessionType = (UnitProfessionType)csv.GetInt32(GetHeadIndex("profession_type"));
            config.isLinkSkill = csv.GetInt32(GetHeadIndex("is_link_skill"));
            config.skillLinkList = new Dictionary<int, int>();
            List<int> ids = csv.GetInt32List(GetHeadIndex("skill_link_list"), ';');
            if (ids.Count > 0)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    config.skillLinkList.Add(i, ids[i]);
                }
            }
            configs.Add(config.skillId, config);
        }
    }
}
