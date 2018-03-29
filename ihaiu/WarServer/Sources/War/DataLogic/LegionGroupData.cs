using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 4:25:53 PM
*  @Description:    势力组数据
* ==============================================================================
*/
namespace Games.Wars
{

    [Serializable]
    /** 势力组数据 */
    public class LegionGroupData : PooledClassObject
    {
        /** 势力组ID */
        public int                          groupId;
        /** 势力列表 */
        public List<LegionData>             list    = new List<LegionData>();
        /** 势力字典 */
        public Dictionary<int, LegionData>  dict    = new Dictionary<int, LegionData>();
        /** 势力数量 */
        public int                          count   = 0;

        /** 添加势力 */
        public void AddLegion(LegionData legionData)
        {
            list.Add(legionData);
            dict.Add(legionData.legionId, legionData);
            legionData.group = this;
            count++;
        }

        /** 目标势力是否和自己是同盟 */
        public bool IsOnceGroup(int legionId)
        {
            return dict.ContainsKey(legionId);
        }

        #region Pool
        public override void OnUse()
        {
            base.OnUse();
        }

        public override void OnRelease()
        {
            groupId = 0;
            count   = 0;
            list.Clear();
            dict.Clear();
            base.OnRelease();
        }
        #endregion
    }
}
