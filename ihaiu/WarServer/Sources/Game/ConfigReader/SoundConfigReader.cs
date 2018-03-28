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
    [ConfigCsv("Config/Sound", true, false)]
    public class SoundConfigReader : ConfigReader<SoundConfig>
    {
        public Dictionary<string, SoundConfig> configsByKey = new Dictionary<string, SoundConfig>();
        public override void ParseCsv(string[] csv)
        {
#if UNITY_EDITOR
            SoundConfig config = SoundConfig.CreateInstance<SoundConfig>();
#else
            SoundConfig config = new SoundConfig();
#endif
            config.id = csv.GetInt32(GetHeadIndex("id"));
            config.owner = csv.GetString(GetHeadIndex("owner"));
            config.action = csv.GetString(GetHeadIndex("action"));
            config.describe = csv.GetString(GetHeadIndex("describe"));
            config.key = csv.GetString(GetHeadIndex("key"));
            config.soundPath = csv.GetString(GetHeadIndex("sound_path"));
            config.soundArgName = csv.GetString(GetHeadIndex("sound_arg_name"));
            config.soundArgValue = csv.GetString(GetHeadIndex("sound_arg_value"));
            config.delayed = csv.GetSingle(GetHeadIndex("delayed"));
            config.time = csv.GetSingle(GetHeadIndex("time"));
            config.soundType = (SoundType)csv.GetInt32(GetHeadIndex("sound_type"));
            config.loopType = (SoundLoopType)csv.GetInt32(GetHeadIndex("loop_type"));

            config.editor_video = csv.GetString(GetHeadIndex("editor_video"));
            config.editor_make_state = csv.GetString(GetHeadIndex("editor_make_state"));
            config.editor_config_state = csv.GetString(GetHeadIndex("editor_config_state"));
            config.editor_game_feedback = csv.GetString(GetHeadIndex("editor_game_feedback"));
            config.editor_audio_feedback = csv.GetString(GetHeadIndex("editor_audio_feedback"));

            configs.Add(config.id, config);
            if(!string.IsNullOrEmpty(config.key) && !configsByKey.ContainsKey(config.key))
                configsByKey.Add(config.key, config);
        }

        public override void Reload()
        {
            configsByKey.Clear();
            base.Reload();
        }

        public SoundConfig GetConfigByKey(string key)
        {
            if(key.StartsWith("event:"))
            {

                Loger.LogErrorFormat("SoundConfigRender key是eventPath {0}", key);
            }

            if (configsByKey.ContainsKey(key))
                return configsByKey[key];

            Loger.LogErrorFormat("SoundConfigRender 不存在key={0}的SounConfig", key);

#if UNITY_EDITOR
            SoundConfig config = SoundConfig.CreateInstance<SoundConfig>();
            config.key = key;
            return config;
#else
            return null;
#endif
        }

        private static SoundConfigReader _Instance;
        public static SoundConfigReader Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new SoundConfigReader();
                    _Instance.Load();
                }
                return _Instance;
            }
        }


    }
}