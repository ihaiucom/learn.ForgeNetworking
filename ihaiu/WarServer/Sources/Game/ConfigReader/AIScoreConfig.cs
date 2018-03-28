using Games.Wars;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/17/2017 9:57:42 AM
*  @Description:    
* ==============================================================================
*/
namespace Games
{
    /// <summary>
    /// AI打分 -- 单位类型
    /// </summary>
    public class AIScoreConfig
    {
        /** ID */
        public int id;
        /** 名称 */
        public string name;
        /** 英雄 */
        public float hero               = 0;
        /**  建筑--主基地 */
        public float buildMainbase      = 0;
        /**  建筑--攻击类型机关 */
        public float buildTowerAttack   = 0;
        /**  建筑--防御类型机关 */
        public float buildTowerDefense  = 0;
        /**  建筑--辅助类型机关 */
        public float buildTowerAuxiliary = 0;
        /**  建筑--门类型机关 */
        public float buildTowerDoor = 0;
        /**  士兵--小怪 */
        public float soliderGeneral = 0;
        /**  士兵--精英 */
        public float soliderElite = 0;
        /**  士兵--Boss */
        public float soliderBoss = 0;


        public float GetVal(UnitData unit)
        {
            return GetVal(unit.unitType, unit.buildType, unit.soliderType);
        }

        public float GetVal(UnitType unitType, UnitBuildType buildType, UnitSoliderType soliderType)
        {

            switch (unitType)
            {
                case UnitType.Hero:
                    return hero;
                case UnitType.Build:
                    switch(buildType)
                    {
                        case UnitBuildType.Mainbase:
                            return buildMainbase;
                        case UnitBuildType.Tower_Attack:
                            return buildTowerAttack;
                        case UnitBuildType.Tower_Defense:
                            return buildTowerDefense;
                        case UnitBuildType.Tower_Auxiliary:
                            return buildTowerAuxiliary;
                        case UnitBuildType.Tower_Door:
                            return buildTowerDoor;
                    }
                    break;
                case UnitType.Solider:
                    switch(soliderType)
                    {
                        case UnitSoliderType.General:
                            return soliderGeneral;
                        case UnitSoliderType.Elite:
                            return soliderElite;
                        case UnitSoliderType.Boss:
                            return soliderBoss;
                    }
                    break;
            }

            return 1;
        }

    }
}
