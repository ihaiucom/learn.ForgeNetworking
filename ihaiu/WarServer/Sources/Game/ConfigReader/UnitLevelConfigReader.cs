using com.ihaiu;
using Games;
using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 8:10:26 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ConfigCsv("Config/UnitLevel", true, false)]
    public class UnitLevelConfigReader : ConfigReader<UnitLevelConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            UnitLevelConfig config = new UnitLevelConfig();
            config.levelId = csv.GetInt32(GetHeadIndex("level_id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.quality = csv.GetInt32(GetHeadIndex("quality"));
            config.level = csv.GetInt32(GetHeadIndex("level"));
            config.pieceId = csv.GetInt32(GetHeadIndex("piece_id"));
            config.pieceNum = csv.GetInt32(GetHeadIndex("piece_num"));
            config.money = csv.GetInt32(GetHeadIndex("money"));
            config.moneyNum = csv.GetInt32(GetHeadIndex("money_num"));
            config.cystalExp = csv.GetInt32(GetHeadIndex("cystal_exp"));


            config.aiSoliders = csv.GetInt32List(GetHeadIndex("ai_solider"), ',');
            config.aiHeros = csv.GetInt32List(GetHeadIndex("ai_hero"), ',');
            config.skillList = new List<Wars.WarEnterSkillData>();

            int atkSkillId = csv.GetInt32(GetHeadIndex("atk_skill"));
            WarEnterSkillData skillData = new WarEnterSkillData();
            skillData.skillId = atkSkillId;
            skillData.skillLevel = 1;
            config.skillList.Add(skillData);



            List<int> skillLevelIdList = csv.GetInt32List(GetHeadIndex("skills"), ';');
            foreach(int skillId in skillLevelIdList)
            {
                if(skillId > 0)
                {
                    skillData = new WarEnterSkillData();

                    skillData.skillId = skillId;
                    skillData.skillLevel = 1;
                    //skillData.skillId       = skilLevelId / 1000;
                    //skillData.skillLevel    = skilLevelId % 1000;
                    config.skillList.Add(skillData);
                }
            }


            config.propList = new List<Prop>();

            int begin = GetHeadIndex("hp");
            int end = GetHeadIndex("movement_speed");
            for (int i = begin; i <= end; i++)
            {
                string filed = GetHeadField(i);
                float val = csv.GetSingle(i);
                Prop prop = new Prop(filed,val);
                prop.PropType = PropType.Base;
                config.propDict.Add(prop.Id, prop);
                config.propList.Add(prop);
            }

            config.buildCost = csv.GetInt32(GetHeadIndex("build_cost"));
            config.buildCd = csv.GetInt32(GetHeadIndex("build_CD"));
            config.rebuildCost = csv.GetInt32(GetHeadIndex("rebuild_cost"));
            config.occupyCost = csv.GetInt32(GetHeadIndex("occupy_cost"));
            config.employCost = csv.GetInt32(GetHeadIndex("employ_cost"));
			config.deathCost = csv.GetInt32(GetHeadIndex("death_cost"));
            config.employType = (UnityEmployType)csv.GetInt32(GetHeadIndex("employ_type"));
            config.employArg = csv.GetSingle(GetHeadIndex("employ_arg"));

            configs.Add(config.levelId, config);

        }

        public UnitLevelConfig GetConfig(int unitId, int unitLevel)
        {
            int levelId = unitId * 1000 + unitLevel;
            UnitLevelConfig config = GetConfig(levelId);
            if(config == null)
            {
                Loger.LogErrorFormat("不存在配置 UnitLevelConfig unitId={0}, unitLevel={1}, levelId={2}",
                    unitId, unitLevel, levelId);
            }
            return config;
        }
    }
}
