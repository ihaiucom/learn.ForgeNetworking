using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/29/2017 10:58:43 AM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 单位移动--使用AstarPathfindingProject
    /// </summary>
    
    public class UnitMoveAstar : AbstractUnitMoveBehaviour
    {

        public enum MoveState
        {
            None,
            Move,
            Stop,
        }

        private Seeker      seeker;
        public MoveState    state = MoveState.Stop;
        private Action      onArrive;
        public Vector3      targetPosition;
        public Path         path;
        public int          currentWaypoint = 0;
        public int          currentWaypointCount = 0;
        // 到达下一个节点距离
        public float        nextWaypointDistance = 1;
        // 到达距离
        public float        arriveDistance = 2;
        // 速度
        public float        speed = 2;
        // 是否有朝向
        public bool         isRotation = true;

        public override void Init(UnitData unitData)
        {
            state = MoveState.Stop;
            base.Init(unitData);
            seeker = GetComponent<Seeker>();
            if (seeker == null)
                seeker = gameObject.AddComponent<Seeker>();
        }


        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="speed">速度</param>
        /// <param name="arriveDistance">到达距离</param>
        /// <param name="onArrive">到达目标点回调</param>
        public void MoveTo(Vector3 position, float speed, float arriveDistance, Action onArrive)
        {
            BeginMove(position, speed, arriveDistance,  onArrive);
        }

        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="onArrive">到达目标点回调</param>
        override public void MoveTo(Vector3 position, Action onArrive)
        {
            BeginMove(position, speed, arriveDistance,  onArrive);
        }


        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="position">位置</param>
        override public void MoveTo(Vector3 position)
        {
            BeginMove(position, speed, arriveDistance,  null);
        }

        private void BeginMove(Vector3 position, float speed, float arriveDistance, Action onArrive)
        { 
            this.targetPosition = position;
            this.speed = speed;
            this.arriveDistance = arriveDistance;
            this.onArrive = onArrive;
            path = null;
            state = MoveState.Move;
            aniManager.Do_Run();
            if(unitAgent.photonView.isMine)
                seeker.StartPath(unitAgent.position, targetPosition, OnPathComplete, unitData.GetPathGraphMask());
        }


        /// <summary>
        /// 停止移动
        /// </summary>
        override public void StopMove()
        {
            if(state != MoveState.Stop)
            {
                state = MoveState.Stop;
                aniManager.Do_Idle();
            }
        }

        private void OnPathComplete(Path p)
        {
            if(p.error)
            {
                path = null;
            }
            else
            {
                path = p;
                currentWaypoint = 0;
                currentWaypointCount = path.vectorPath.Count;
            }
        }

        private Vector3 point;
        private Vector3 dir;
        private Quaternion quaternion;

        private void Update()
        {
            switch(state)
            {
                case MoveState.Move:
                    if (path != null)
                    {

                        if (currentWaypoint >= currentWaypointCount)
                        {
                            StopMove();

                            if (onArrive != null)
                            {
                                onArrive();
                            }
                            return;
                        }

                        point = path.vectorPath[currentWaypoint];
                        dir = (point - unitAgent.position).normalized;
                        quaternion = Quaternion.LookRotation(dir);
                        unitAgent.Move(quaternion, speed, isRotation, false);
                        if(currentWaypoint < currentWaypointCount - 1)
                        {
                            if (Vector3.Distance(unitAgent.position, point) < nextWaypointDistance)
                            {
                                currentWaypoint++;
                            }
                        }
                        else
                        {
                            if (Vector3.Distance(unitAgent.position, point) < arriveDistance)
                            {
                                currentWaypoint++;
                            }
                        }

                    }
                    break;
            }
        }

    }
}
