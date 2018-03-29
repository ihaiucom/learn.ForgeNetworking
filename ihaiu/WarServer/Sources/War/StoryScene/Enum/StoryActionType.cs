using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 行为类型
    /// </summary>
    public enum StoryActionType : int
    {
        None            = 0,


        // 进入剧情
        Enter,
        // 退出剧情
        Exit,

        // [单位]添加 (可以在有戏中运行的)
        //UnitAdd,
        // [单位]移除 (可以在有戏中运行的)
        //UnitRemove,
        // [单位]播放动画 (可以在有戏中运行的)
        HeroPlayAnimation,
        // [单位]修改属性 (可以在有戏中运行的)
        HeroPropSet,
        // [单位]添加属性 (可以在有戏中运行的)
        HeroPropAdd,
        // [单位]位置设置
        HeroPosition,
        // [单位]移动
        HeroMove,

        // [物件]添加 (只是显示，不能在游戏中运行)
        ObjAdd,
        // [物件]显示
        ObjShow,
        // [物件]隐藏
        ObjHide,
        // [物件]移除 (只是显示，不能在游戏中运行)
        ObjRemove,
        // [物件]播放动作 (只是显示，不能在游戏中运行)
        ObjPlayAnimation,
        // [物件]移动 (只是显示，不能在游戏中运行)
        ObjMove,

        
        // [UI] 打开关闭遮罩
        UIMask,
        // [UI] 显示隐藏UI组件
        UIShowHideComplete,

        // 添加文本气泡
        AddBubble,
        
        // [摄像机] 动画位移
        CameraAnimationPosition,
        // [摄像机] 动画旋转
        CameraAnimationRotation,
        // [摄像机] 震屏
        CameraShake,
        // [摄像机] 跟随
        CameraFollow,

        // [场景物件] 场景内的物件操作
        SceneObjManager,
        // [场景物件] 场景内的物件移除
        SceneObjRemove,

        // 暂停剧情
        Pause,
    }
}