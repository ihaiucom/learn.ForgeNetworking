using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 11:32:16 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗资源 */
    public class WarRes : AbstractRoomObject
    {
        /// <summary>
        /// 战斗UI
        /// </summary>
        public  static  string  WAR_UI                      =   "PrefabUI/UI/WarUI";

		/// <summary>
		/// 单位头顶图标 （本来是用来显示可维修状态，现在暂时去除）
		/// </summary>
		public  static  string  UNIT_STATUS_UI              =   "PrefabUI/UI/UnitStatus";

        /// <summary>
        /// 单位点击菜单
        /// </summary>
        public static string UNIT_MENU_UI                   =   "PrefabUI/UI/WarUnitMenu";

        /// <summary>
        /// 小地图显示icon
        /// </summary>
        public  static  string  WAR_MINIMAPICON             =   "PrefabUI/UI/MiniMapIcon";
        /// <summary>
        /// 小地图提示
        /// </summary>
        public  static  string  WAR_MINIMAPFLASH            =   "PrefabUI/UI/MiniMapFlash";
        /// <summary>
        /// 技能可以释放的背景
        /// </summary>
        public  static  string  WAR_SKILLCANPUTBG           =   "ImageSprites/Icon/bg_027";
        /// <summary>
        /// 技能无法释放的背景
        /// </summary>
        public  static  string  WAR_SKILLCANCELBG           =   "ImageSprites/Icon/bg_025";
        /// <summary>
        /// 伤害prefab
        /// </summary>
        public  static  string  WAR_UIBloodDame             =   "PrefabUI/UI/WarAddDuc";
        /// <summary>
        /// 提示
        /// </summary>
        public  static  string  WAR_UITip                   =   "PrefabUI/UI/WarTip";
        /// <summary>
        /// 血条
        /// </summary>
        public  static  string  WAR_UIBloodHp               =   "PrefabUI/UI/";
        /// <summary>
        /// 中立机关提示 button
        /// </summary>
        public  static  string  WAR_NeutralityButton        =   "PrefabUI/UI/NeutralityTip";
        /// <summary>
        /// 死亡能量回收特效
        /// </summary>
        public  static  string  WAR_UIDieEffect             =   "PrefabUI/UI/EngeryEffect";
        /// <summary>
        /// 气泡提示
        /// </summary>
        public  static  string  WAR_UIAddText               =   "PrefabUI/UI/AddText";
        /// <summary>
        /// 黑色遮罩
        /// </summary>
        public  static  string  WAR_UIMask                  =   "PrefabUI/UI/StoryUI";


        /// <summary>
        /// 战斗--预设--单位--玩家控制器
        /// </summary>
        public static string WAR_PREFAB_UNIT_PLAYER = "PrefabWar/Unit_Player";

        /// <summary>
        /// 战斗--预设--单位--英雄
        /// </summary>
        public static string WAR_PREFAB_UNIT_HERO = "PrefabWar/Unit_Hero";

        /// <summary>
        /// 战斗--预设--单位--士兵
        /// </summary>
        public static string WAR_PREFAB_UNIT_SOLIDER = "PrefabWar/Unit_Solider";

        /// <summary>
        /// 战斗--预设--单位--建筑
        /// </summary>
        public static string WAR_PREFAB_UNIT_BUILD = "PrefabWar/Unit_BUILD";

        /// <summary>
        /// 战斗--预设--建筑格子
        /// </summary>
        public static string WAR_PREFAB_BUILDCELL = "PrefabWar/BuildCell";

        /// <summary>
        /// 战斗--预设--PUN--玩家控制器
        /// </summary>
        public static string WAR_PREFAB_PUN_PLAYER = "PrefabWar/PUN_Player";
        /// <summary>
        /// 战斗--预设--PUN--场景
        /// </summary>
        public static string WAR_PREFAB_PUN_Scene = "PrefabWar/PUN_Scene";


        /// <summary>
        /// 战斗--预设--TS--玩家控制器
        /// </summary>
        public static string WAR_PREFAB_TS_Player = "PrefabWar/TS_Player";
    }
}
