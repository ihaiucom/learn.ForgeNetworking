using System.Collections.Generic;
namespace Games.Wars
{
    public partial class UnitData
    {
        /** 镜像单位的主体UID */
        public int                  cloneUnitMainUid = -1;
        /** 是否是镜像单位 */
        public bool isCloneUnit { get { return cloneUnitMainUid != -1; } }
        /** 是否和主体一起死亡 */
        public bool                 isDeathWithMain = false;
        /** 镜像的单位ID列表 */
        public List<int>            cloneChilds = new List<int>();
        /** 镜像存活时间内，减少血量间隔时间 */
        public float                lifeDelateTime              = 0;
        /// <summary>
        /// 输出百分比
        /// </summary>
        public float                attackPer                   = 1;
        /// <summary>
        /// 防御百分比(受伤)
        /// </summary>
        public float                hitPer                      = 1;
        /// <summary>
        /// 单位时间减少血量
        /// </summary>
        public float                reduceHpPerTime             = 0;


        /// <summary>
        /// 技能附加的buff信息
        /// Dictionary<int, Dictionary<int, SkillActionConfigBuffCreate>>
        ///     被动技能id           技能id   buff信息（buffid，生命周期）
        /// </summary>
        public Dictionary<int, Dictionary<int, SkillActionConfigBuffCreate>>  skillAddBuffDic             = new Dictionary<int, Dictionary<int, SkillActionConfigBuffCreate>>();
        public List<SkillActionConfigBuffCreate> GetAddBuffId(int currentSkillId)
        {
            List <SkillActionConfigBuffCreate> result = new List<SkillActionConfigBuffCreate>();
            if (skillAddBuffDic.Count > 0)
            {
                foreach (var bassItem in skillAddBuffDic)
                {
                    foreach (var item in bassItem.Value)
                    {
                        if (item.Key == currentSkillId)
                        {
                            result.Add(item.Value);
                        }
                    }
                }
            }
            return result;
        }
    }
}
