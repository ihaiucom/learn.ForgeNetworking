using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      11/24/2017 4:57:30 PM
*  @Description:    单位数据--AI
* ==============================================================================
*/
namespace Games.Wars
{
    public partial class UnitData
    {

        /** AI 积分 */
        public float aiScore = 0;



        /// <summary>
        /// 添加仇恨值
        /// </summary>
        /// <param name="enemyUnitUid">敌人单位UID</param>
        /// <param name="hatred"仇恨值></param>
        public void AddHatred(int enemyUnitUid, int hatred)
        {
			Loger.Log ("添加仇恨值 " + ToStringBase() +",      enemyUnitUid=" +enemyUnitUid + "  hatred=" + hatred );
            sceneData.AddHatred(uid, enemyUnitUid, hatred);
        }

        /// <summary>
        /// 获取仇恨值
        /// </summary>
        /// <param name="enemyUnitUid">敌人单位UID</param>
        /// <returns></returns>
        public int GetHatred(int enemyUnitUid)
        {
            return sceneData.GetHatred(uid, enemyUnitUid);
        }

    }
}
