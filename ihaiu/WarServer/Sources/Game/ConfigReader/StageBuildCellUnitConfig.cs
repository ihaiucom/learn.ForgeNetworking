using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/14/2017 10:17:37 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡--建筑格子--初始单位配置 */
    public class StageBuildCellUnitConfig : StageUnitConfig
    {
        /** 所属势力ID */
        public int legionId = 0;
        /** 关联建筑格子 */
        public List<int> switchBuildCellList = new List<int>();

        [Newtonsoft.Json.JsonIgnore]
        public string switchBuildCellListString
        {
            get
            {
                return switchBuildCellList.ToStr<int>(",", "", "");
            }

            set
            {
                switchBuildCellList = value.ToInt32List();
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public bool HasSetting
        {
            get
            {
                return unitId > 0 && legionId != -1;
            }
        }

        public StageBuildCellUnitConfig Clone()
        {
            StageBuildCellUnitConfig config = new StageBuildCellUnitConfig();
            CopyTo(config);
            config.legionId = legionId;
            config.switchBuildCellList.Clear();
            foreach(int cellId in switchBuildCellList)
            {
                config.switchBuildCellList.Add(cellId);
            }

            return config;
        }

        public override string ToString()
        {
            return string.Format("势力:{0}, 单位ID:{1}, 单位等级:{2}   {3}", legionId, unitId, unitLevel, name);
        }


    }
}
