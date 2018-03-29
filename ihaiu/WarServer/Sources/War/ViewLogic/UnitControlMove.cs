using Games.PB;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 单位操控，维修回收机关
    /// </summary>
    public partial class UnitControl : AbstractUnitMonoBehaviour
    {
        /// <summary>
        /// 目标单位
        /// </summary>
        private UnitAgent   currentEnterUnitAgent = null;
        #region 进入龙击枪
        /// <summary>
        /// 进入龙击枪
        /// </summary>
        /// <param name="move"></param>
        /// <param name="_unitAgent"></param>
        public void OnMovePointer(bool move, UnitAgent _unitAgent)
        {
            unitAgent.Move(_unitAgent.position, unitAgent.unitData.prop.MovementSpeed, 2, true, onArrive);
            currentEnterUnitAgent = _unitAgent;
        }
        void onArrive()
        {
            unitAgent.transform.SetParent(currentEnterUnitAgent.animatorManager.TowerPlayPos);
            unitAgent.transform.localPosition = Vector3.zero;
            unitAgent.AnchorRotation.localEulerAngles = Vector3.zero;
            unitAgent.forwardObject.localEulerAngles = Vector3.zero;
            Game.camera.CameraMg.EnterFirstCamera(true, unitAgent.AnchorRotation, currentEnterUnitAgent);
        }
        #endregion
        #region 机关回收
        public void OnMoveUnitRecovery(UnitAgent _unitAgent)
        {
            unitAgent.Move(_unitAgent.position, unitAgent.unitData.prop.MovementSpeed, 2, true, OnRecoveryArrive);
            currentEnterUnitAgent = _unitAgent;
        }
        void OnRecoveryArrive()
        {
            if(unitAgent.photonView.isMine)
            {
                room.clientOperationUnit.EndRecoveryUnit(currentEnterUnitAgent.uid, true);
            }
            //room.clientNetS.EndRecoveryUnit(currentEnterUnitAgent.uid, uid);
            unitAgent.animatorManager.Do_Idle();// 回收瞬间完成，进入待机
        }
        #endregion
        #region 机关维修
        private bool    EnterFixUnit = false;
        public void OnMoveToRebuildUnit(UnitAgent tower)
        {
            Debug.Log("OnMoveToRebuildUnit");
            EnterFixUnit = false;
            unitAgent.Move(tower.position, unitAgent.unitData.prop.MovementSpeed, 2, true, OnArriveRebuildUnit);
            currentEnterUnitAgent = tower;
        }

        // 到达要维修的单位身边
        void OnArriveRebuildUnit()
        {
            if (unitAgent.photonView.isMine)
            {
                Debug.Log("OnArriveRebuildUnit");
                room.clientOperationUnit.DoingRebuildUnit(currentEnterUnitAgent.uid);
            }
        }
        #endregion
        #region 接受维修信息
        public void DoingRebuildUnit(UnitAgent tower)
        {
            EnterFixUnit = true;
            unitAgent.animatorManager.Do_Fix(); // 做维修动作
            tower.ActionDoingRebuild();
        }
        #endregion
        #region 停止维修或回收
        public void EndRebuildUnit(UnitAgent tower)
        {
            if (EnterFixUnit)
            {
                unitAgent.animatorManager.Do_Idle();
                EnterFixUnit = false;
                if (tower != null && tower.isActiveAndEnabled)
                {
                    tower.ActionEndRebuild();
                }
            }

        }
        #endregion


    }

}
