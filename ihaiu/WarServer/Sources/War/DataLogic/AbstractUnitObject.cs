using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/6/2017 6:07:57 PM
*  @Description:    抽象 单位对象
* ==============================================================================
*/
namespace Games.Wars
{
    public class AbstractUnitObject<T> : PooledClassObject, IRoomObject, IUnitComponent, IGamePause, ISyncedUpdate where T : AbstractUnitObject<T>
    {

        // =====================================
        // PooledClassObject
        // -------------------------------------
        public override void OnRelease()
        {
            room        = null;
            unitData    = null;
            base.OnRelease();
        }



        // =====================================
        // IRoomObject
        // -------------------------------------
        /** 房间--门面 */
        public WarRoom room { get; set; }

        /** 房间--场景数据 */
        public WarSceneData sceneData
        {
            get
            {
                return room.sceneData;
            }
        }



        /** 房间--时间 */
        public WarTime Time
        {
            get
            {
                return room.Time;
            }
        }



        /** 房间--逻辑时间 */
        public WarLTime LTime
        {
            get
            {
                return room.LTime;
            }
        }

        // =====================================
        // IUnitComponent
        // -------------------------------------

        /** 单位数据 */
        public UnitData unitData
        {
            get;
            set;
        }


        /** 势力ID */
        public int LegionId
        {
            get
            {
                return unitData.legionId;
            }
        }



        /** 归属势力ID */
        public int AscriptionLegionId
        {
            get
            {
                return unitData.ascriptionLegionId;
            }
        }

        /** 势力数据 */
        public LegionData legionData
        {
            get
            {
                return sceneData.GetLegion(unitData.legionId);
            }
        }


        /** 归属势力数据 */
        public LegionData ascriptionLegionData
        {
            get
            {
                return sceneData.GetLegion(unitData.ascriptionLegionId);
            }
        }

        /** 是否是自己的 */
        public bool clientIsOwn
        {
            get
            {
                return unitData.legionId == room.clientOwnLegionId && !unitData.isCloneUnit;
            }
        }


        /** 是否是敌人的 */
        public bool clientIsEnemy
        {
            get
            {
                return GetRelationTypeByClientOwn() == RelationType.Enemy;
            }
        }

        /** 是否是好友的 */
        public bool clientIsFriendly
        {
            get
            {
                return GetRelationTypeByClientOwn() == RelationType.Friendly;
            }
        }

        /** 是否是服务器对象 */
        public bool isServerObject
        {
            get
            {
                if (unitData.unitType != UnitType.Hero)
                {
                    return true;
                }

                if (legionData.isRobot)
                {
                    return true;
                }
                return false;
            }
        }

        /** 是否是客户端对象 */
        public bool isClientObject
        {
            get
            {
                return !isServerObject;
            }
        }

        /** 是否是客户端拥有权限的对象 */
        public bool isClientOwnObject
        {
            get
            {
                return isClientObject && clientIsOwn;
            }
        }

        /** 获取关系类型 */
        public RelationType GetRelationType(int legionId)
        {
            return sceneData.relationMatrixConfig.GetRelationType(this.LegionId, legionId);
        }

        /** 获取关系类型,相对于客户端自己 */
        public RelationType GetRelationTypeByClientOwn()
        {
            return sceneData.relationMatrixConfig.GetRelationType(this.LegionId, room.clientOwnLegionId);
        }


        /** 单位--安装 */
        virtual public void OnUnitInstall()
        {

        }

        /** 单位--卸载 */
        virtual public void OnUnitUninstall()
        {

        }


        // =====================================
        // IGamePause
        // -------------------------------------
        /** 游戏--暂停 */
        virtual public void OnGamePause()
        {

        }

        /** 游戏--继续 */
        virtual public void OnGameUnPause()
        {

        }

        // =====================================
        // ISyncedUpdate
        // -------------------------------------
        virtual public void OnSyncedUpdate()
        {

        }

    }

}
