using com.ihaiu;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 4:00:46 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [ConfigCsv("Config/Avatar", true, false)]
    public class AvatarConfigReader : ConfigReader<AvatarConfig>
    {
        public override void ParseCsv(string[] csv)
        {
            AvatarConfig config = new AvatarConfig();
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.name = csv.GetString(GetHeadIndex("name"));
            config.tip = csv.GetString(GetHeadIndex("tip"));
            config.icon = csv.GetString(GetHeadIndex("icon"));
            config.MiniIcon = csv.GetString(GetHeadIndex("mini_icon"));
            config.pieceIcon = csv.GetString(GetHeadIndex("piece_icon"));
            config.model = csv.GetString(GetHeadIndex("model"));
            config.model_H = csv.GetString(GetHeadIndex("model_H"));
            config.audioMove = csv.GetString(GetHeadIndex("audio_move"));
            config.audioHit = csv.GetString(GetHeadIndex("audio_hit"));
            config.audioDead = csv.GetString(GetHeadIndex("audio_dead"));
            config.audioBirth = csv.GetString(GetHeadIndex("audio_birth"));
            config.audioAttackLoop = csv.GetString(GetHeadIndex("audio_attack_loop"));
            //config.effectpath = new Dictionary<int, string>();
            //for (int i = 1; i < 7; i++)
            //{
            //    string tem   =   csv.GetString(GetHeadIndex("Effect" + i));
            //    config.effectpath.Add(i, tem);
            //}
            configs.Add(config.id, config);
        }
    }
}
