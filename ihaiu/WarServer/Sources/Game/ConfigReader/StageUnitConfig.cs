using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/13/2017 6:41:49 PM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    [Serializable]
    /** 关卡--单位配置 */
    public class StageUnitConfig
    {
        /** 单位ID */
        public int      unitId = 0;
        /** 单位等级 */
        public int      unitLevel = 1;
        /** 名称备注 */
        public string   name = string.Empty;
        /** 单位模型id，非0用新的模型替换默认模型，其他无变化 */
        public int      avatarId = 0;

        virtual public StageUnitConfig  Clone()
        {
            StageUnitConfig config = new StageUnitConfig();
            CopyTo(config);
            return config;
        }

        virtual public void CopyTo(StageUnitConfig config)
        {
            config.unitId = unitId;
            config.avatarId = avatarId;
            config.unitLevel = unitLevel;
            config.name = name;
        }

        public override string ToString()
        {
            return string.Format("单位ID:{0}, 单位等级:{1}  {2}", unitId, unitLevel, name);
        }
    }
}
