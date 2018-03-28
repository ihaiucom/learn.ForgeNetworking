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
    [ConfigCsv("Config/SkillLink", true, false)]
    public class SkillLinkConfigReader : ConfigReader<SkillLinkConfig>
    {
        public override void ParseCsv(string[] csv)
        {

            SkillLinkConfig config = new SkillLinkConfig();
            config.skillId = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.tip2 = csv.GetString(GetHeadIndex("tip_2"));
            config.tip3 = csv.GetString(GetHeadIndex("tip_3"));
            config.unlockTip = csv.GetString(GetHeadIndex("unlock_tip"));
            config.icon = csv.GetString(GetHeadIndex("icon"));
            config.unlockUnitId = csv.GetInt32(GetHeadIndex("unlock_unit_id"));
            config.unlockUnitLevel = csv.GetInt32(GetHeadIndex("unlock_unit_level"));
            config.skillGroup = csv.GetInt32(GetHeadIndex("skill_group"));
            config.cd = csv.GetInt32(GetHeadIndex("cd"));
            config.isPassive = csv.GetInt32(GetHeadIndex("skill_type")) == 2;
            config.passiveType = (SkillPassiveType)csv.GetInt32(GetHeadIndex("trigger_type"));
            config.castType = (SkillCastType)csv.GetInt32(GetHeadIndex("cast_type"));
            config.castRadius = csv.GetSingle(GetHeadIndex("cast_radius"));
            config.castLineWidth = csv.GetSingle(GetHeadIndex("cast_line_width"));
            config.castFanAngle = csv.GetSingle(GetHeadIndex("cast_fan_angle"));
            config.castCircleRadius = csv.GetSingle(GetHeadIndex("cast_circle_radius"));
            config.aiScoreUnitType = csv.GetInt32(GetHeadIndex("AIScoreUnitType"));
            config.aiScoreUnitTypeTip = csv.GetString(GetHeadIndex("AIScoreUnitType_tip"));
            config.aiScoreDistance = csv.GetInt32(GetHeadIndex("AIScoreDistance"));
            config.aiScoreDistanceTip = csv.GetString(GetHeadIndex("AIScoreDistance_tip"));
            config.aiScoreAttackHatred = csv.GetInt32(GetHeadIndex("AIScoreAttackHatred"));
            config.aiScoreAttackHatredTip = csv.GetString(GetHeadIndex("AIScoreAttackHatred_tip"));
            config.aiScoreWeight = csv.GetInt32(GetHeadIndex("AIScoreWeight"));
            config.aiScoreWeightTip = csv.GetString(GetHeadIndex("AIScoreWeight_tip"));
            config.targetRelation = (RelationType) csv.GetInt32(GetHeadIndex("target_relation"));
            config.targetUnitType = (UnitType)csv.GetInt32(GetHeadIndex("target_unit_type"));
            config.targetBuildType = (UnitBuildType)csv.GetInt32(GetHeadIndex("target_build_type"));
            config.targetSoliderType = (UnitSoliderType)csv.GetInt32(GetHeadIndex("target_solider_type"));
            config.targetSpaceType = (UnitSpaceType)csv.GetInt32(GetHeadIndex("space_type"));
            config.targetProfessionType = (UnitProfessionType)csv.GetInt32(GetHeadIndex("profession_type"));

            configs.Add(config.skillId, config);
        }
    }
}
