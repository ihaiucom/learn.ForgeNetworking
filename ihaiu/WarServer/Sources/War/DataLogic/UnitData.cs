using Games.PB;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/4/2017 4:36:58 PM
*  @Description:    单位数据
* ==============================================================================
*/
namespace Games.Wars
{
    //#if UNITY_EDITRO
    //    [Serializable]
    //#endif
    public partial class UnitData : AbstractUnitObject<UnitData>, IUnitInstall
    {
        
        /** 唯一ID */
        public int              uid;

        /** 单位ID */
        public int              unitId;

        /** 单位等级 */
        public int              unitLevel;

        /** AvatarConfig */
        public AvatarConfig     avatarConfig;

        /** 单位配置 */
        public UnitConfig       unitConfig;

        /** 单位等级配置 */
        public UnitLevelConfig  unitLevelConfig;

        /** 名称 */
        public string           name;

        /** 势力ID */
        public int              legionId;

        public string ToStringBase()
        {
            return string.Format("name{0}, uid={1}, unitId={2}, legionId={3}, unitType={4}", name, uid, unitId, legionId, unitType);
        }

        /** 当前归属势力ID */
        public int ascriptionLegionId
        {
            get
            {
                if (IsEmploying)
                    return employLegionId;
                return legionId;
            }
        }

        public int GetPathGraphMask()
        {
            return UnityPathGraphSetting.GetPathGraphMask(unitType, spaceType);
        }


        #region Type

        /** Type--单位产出类型 */
        public UnitProduceType         unitProduceType                = UnitProduceType.Normal;

        /** Type--单位类型 */
        public UnitType         unitType                = UnitType.None;

        /** Type--建筑类型 */
        public UnitBuildType    buildType               = UnitBuildType.None;

        /** Type--士兵类型 */
        public UnitSoliderType    soliderType             = UnitSoliderType.None;

        /** Type--单位职业类型 */
        public UnitProfessionType professionType      = UnitProfessionType.None;

        /** Type--单位空间类型 */
        public UnitSpaceType    spaceType               = UnitSpaceType.Ground;
        #endregion

        #region 单位功能配置

        /** 是否可以旋转 */
        public bool enableRotation
        {
            get
            {
                return unitConfig.enableRotation;
            }
        }

        /** 是否手动攻击 */
        public bool isManualAttack
        {
            get
            {
                return unitConfig.isManualAttack;
            }
        }

        /** RVO半径 */
        public float rvoRadius
        {
            get
            {
                return unitConfig.rvoRadius;
            }
        }

        #endregion


        #region 单位状态，是否能操作
        /// <summary>
        /// 单位是否异常状态中，不可移动，不可攻击
        /// </summary>
        public bool IsUnusual
        {
            get
            {
                return !isLive || isFix || isSkillAttack || prop.IsFreezed || prop.StateVertigo;
            }
        }
        /// <summary>
        /// 不可移动
        /// </summary>
        public bool HeroDisableMove
        {
            get
            {
                return isSkillAttack || prop.IsFreezed || prop.StateVertigo || prop.MovementSpeed <= 0;
            }
        }
        #endregion

        /** 位置坐标 */
        public Vector3              position            = Vector3.zero;
        /** 朝向坐标 */
        public Vector3              rotation            = Vector3.zero;
        /** 单位半径 */
        public float                unitRadius          = 3;
        /** 单位飞行高度半径 */
        public float                unitFlyHeight       = 0;
        /** 属性 */
        public PropUnit             prop                = new PropUnit();
        /** buff容器 */
        public BuffContainer        buffContainer       = new BuffContainer();
        /** 光环容器 */
        public HaoleContainer       haoleContainer      = new HaoleContainer();

        /** 单位是否在场景中 */
        public bool                 isInScene           = false;
        /** 单位是否活着 */
        public bool                 isLive              = true;
        /// <summary>
        /// 是否无敌状态，技能赋予，仅释放技能过程中
        /// </summary>
        public bool                 invincible          = false;
        /// <summary>
        /// 是否释放无法移动的技能
        /// </summary>
        public bool                 isSkillAttack       = false;
        /** 机关是否修理中，修理中可以被攻击，但不能攻击，被攻击重新死亡 */
        public bool                 isFix               = false;
        /// <summary>
        /// 显示血条时间
        /// </summary>
        public float                showBloodTime       = -1;
        /// <summary>
        /// 隐藏血条时间间隔
        /// </summary>
        public float                hideBloodTime       = 3;

        private UnitAgent _unitAgent;
        public UnitAgent unitAgent
        {
            get
            {
                if (_unitAgent == null)
                {
                    _unitAgent = room.clientSceneView.GetUnit(uid);
                }
                return _unitAgent;
            }
        }
       

        public bool IsHideBlood
        {
            get
            {
                if (isFix)
                {
                    return false;
                }
                //if (!IsInSceneAndLive || isFix || unitType == UnitType.Halo)
                if (!IsInSceneAndLive || unitType == UnitType.Halo)
                {
                    return true;
                }
                if (unitType == UnitType.Hero || soliderType == UnitSoliderType.Boss || isCloneUnit)
                {
                    return false;
                }
                if (showBloodTime < 0 || invincible)
                {
                    return true;
                }
                else
                {
                    if (LTime.time - showBloodTime >= hideBloodTime)
                    {
                        if (unitType == UnitType.Build)
                        {
                            if (prop.Hp < prop.HpMax)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return false;
            }
        }


        public bool IsInSceneAndLive
        {
            get
            {
                return isInScene && isLive;
            }
        }






        #region 特殊属性

        public bool isStateFreezedMove
        {
            get
            {
                return prop.StateFreezed || prop.StateVertigo;
            }
        }

        /** 是否可以移动 */
        public bool disableMove
        {
            get
            {
                return isSkillAttack || prop.StateFreezed || prop.StateVertigo;
            }
        }


        /** 是否可以攻击 */
        public bool disableAttack
        {
            get
            {
                return isSkillAttack || prop.StateFreezed || prop.StateVertigo;
            }
        }
        #endregion



        #region 士兵
        /** 士兵航线ID */
        public int routeId = -1;
        #endregion


        
        public UnitData Clone()
        {
            UnitData n = new UnitData();
            return n;
        }



    }
}
