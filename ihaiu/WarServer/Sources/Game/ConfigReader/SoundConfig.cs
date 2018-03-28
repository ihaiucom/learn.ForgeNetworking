using com.ihaiu;
using Games.Wars;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 1:33:35 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 技能配置 */
#if UNITY_EDITOR
    public class SoundConfig : ScriptableObject , IToCsv
#else
    public class SoundConfig : IToCsv
#endif
    {
        // 编辑器检测错误
        public string error;

        // ID
        public int id;

        // 拥有者
        public string owner;

        // 行为
        public string action;

        //音频描述
        public string describe;

        //音频类型 1BGM(Music)，2UI，3大厅展示(Show)，4施法音效(Attack,Skill)，5飞行音效(Fly)，6命中音效(Hit)，7受击音效(GetHit)，8死亡音效(Death)，9移动音效(Move)
        public SoundType soundType;

        // Key
        public string key;

        // 事件路径
        public string soundPath;

        // 事件参数名称
        public string soundArgName;

        // 事件参数值描述
        public string soundArgValue;

        // 触发时间   填入延时播放时间,用于微调音频播放时机,单位(秒)
        public float delayed = 0;


        // 音效时长 单位(秒)
        public float time = 0;

        // 循环模式    0无循环，1整体循环，2循环-结束，3起始-循环-结束
        public SoundLoopType loopType = 0;

        // 视频
        public string editor_video;

        // 制作进度
        public string editor_make_state;

        // 配置进度
        public string editor_config_state;

        // 工作室反馈
        public string editor_game_feedback;

        // 音频部反馈
        public string editor_audio_feedback;

        public SoundConfig Clone()
        {
#if UNITY_EDITOR
            SoundConfig n = SoundConfig.CreateInstance<SoundConfig>();
#else
            SoundConfig n = new SoundConfig();
#endif
            n.id = id;
            n.owner = owner;
            n.action = action;
            n.describe = describe;
            n.soundType = soundType;
            n.key = key;
            n.soundPath = soundPath;
            n.soundArgName = soundArgName;
            n.soundArgValue = soundArgValue;
            n.delayed = delayed;
            n.time = time;
            n.loopType = loopType;
            return n;

        }


        public string ToCsv(char delimiter)
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}", delimiter,
                id, owner, action, describe, editor_video, editor_make_state, editor_config_state, editor_game_feedback, editor_audio_feedback, (int)soundType, key, soundPath, soundArgName, soundArgValue, delayed, time, (int) loopType
                );
        }

    }

    public enum SoundLoopType
    {
        [Help("无循环")]
        Default = 0,

        [Help("整体循环")]
        Loop = 1,

        [Help("循环-结束")]
        Loop_End = 2,

        [Help("起始-循环-结束")]
        Begin_Loop_End = 3,
    }

    public enum SoundType
    {
        Music = 1,

        UI,

        [Help("大厅展示")]
        Show,

        [Help("施法音效")]
        Skill,

        [Help("飞行音效")]
        Fly,

        [Help("命中音效")]
        Hit,

        [Help("受击音效")]
        GetHit,

        [Help("死亡音效")]
        Death,

        [Help("移动音效")]
        Move,

        [Help("其他音效")]
        OtherSFX,
    }


}
