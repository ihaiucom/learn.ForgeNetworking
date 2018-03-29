using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 3:42:54 PM
*  @Description:    战斗结算数据
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗结算数据 */
    public class WarOverData
    {
        /** 是否手动点退出 */
        public bool isManualExit = false;

        /** 关卡ID */
        public int          stageId = 0;
        /** 关卡类型 */
        public StageType    stageType = StageType.Dungeon;
        /** 结果类型 */
        public WarOverType  overType = WarOverType.Draw;
        /** 星星数量 */
        public int          starNum = 0;
        /** 击杀数量 */
        public int          killNum = 0;

        public override string ToString()
        {
            return string.Format("WarOverData: stageId={0}, stageType={1}, overType={2}", stageId, stageType, overType);
        }
    }

}
