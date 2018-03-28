using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/26/2017 3:52:34 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    public class AvatarConfig
    {
        /** 编号 */
        public int      id;
        /** 名称 */
        public string   name;
        /** 说明 */
        public string   tip;
        /** 图标 */
        public string   icon;
        /// <summary>
        ///  小地图icon
        /// </summary>
        public string   MiniIcon;
        /** 碎片图标 */
        public string   pieceIcon;
        /** 模型预设 */
        public string   model;
        /** 高清模型 */
        public string   model_H;
        /** 技能特效路径 */
        //public  Dictionary<int,string> effectpath       = new Dictionary<int, string>();

        // 移动声效
        public string audioMove;
        // 受击声效
        public string audioHit;
        // 死亡声效
        public string audioDead;
        // 出生声效
        public string audioBirth;
        // 循环普攻声效
        public string audioAttackLoop;

    }
}
