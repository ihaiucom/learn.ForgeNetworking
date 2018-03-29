using Games.PB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games.Wars
{
    public partial class WarSurrenderController : AbstractRoomObject
    {
        public enum State
        {
            None,
            Begin,
            End,
        }


        public WarSurrenderController(WarRoom room)
        {
            this.room = room;
        }

        /** 玩家列表 */
        private Dictionary<int, TSBallotState>   legionDict = new Dictionary<int, TSBallotState>();

        /** 流程状态 */
        public State state = State.None;
        /** 发起投降的玩家 */
        private int beginLegionId = -1;
        /** 投降时间配置 */
        private float timeConfig = 10;
        /** 投降倒计时 */
        private float lefttime = 0;



        /// <summary>
        /// 事件
        /// </summary>
        public Action changeEvent;

        /** 同意人数 */
        public int okNum = 0;
        /** 反对人数 */
        public int noNum = 0;

        /// <summary>
        /// 投票数量
        /// </summary>
        private int ballotNum = 0;

        /// <summary>
        /// 投票总数
        /// </summary>
        public int ballotTotal = 0;

        /// <summary>
        /// 投票结果
        /// </summary>
        private TSBallotState overSelect;



        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                //stream.SendNext(state);
                //stream.SendNext(lefttime);
                //stream.SendNext(okNum);
                //stream.SendNext(noNum);
                //stream.SendNext(ballotNum);
                //stream.SendNext(ballotTotal);
            }
            else
            {
                //State state = (State)stream.ReceiveNext();
                //lefttime = (float)stream.ReceiveNext();
                //okNum = (int)stream.ReceiveNext();
                //noNum = (int)stream.ReceiveNext();
                //ballotNum = (int)stream.ReceiveNext();
                //ballotTotal = (int)stream.ReceiveNext();

                
            }
        }

        /// <summary>
        /// 开始投票
        /// </summary>
        void Begin()
        {
            if (state == State.Begin) return;
            state = State.Begin;
            overSelect = TSBallotState.None;
            legionDict.Clear();
            lefttime = timeConfig;
        }

        /// <summary>
        /// 结束投票
        /// </summary>
        void Stop()
        {
            Debug.Log("~~投降投票 Stop");
            state = State.End;
            lefttime = 0;
            beginLegionId = -1;
            if(overSelect == TSBallotState.None)
            {
                CheckOver(true);
            }


            if (changeEvent != null)
            {
                changeEvent();
            }
        }

        /// <summary>
        /// 玩家投票
        /// </summary>
        /// <param name="legionId">玩家</param>
        /// <param name="select">选项</param>
        public void OnBattlot(int legionId, TSBallotState select)
        {
            Debug.Log("~~投降投票 OnBattlot legionId=" + legionId + "  select=" + select + "  state=" + state);
            if(state != State.Begin)
            {
                beginLegionId = legionId;
                Begin();
            }

            if(!legionDict.ContainsKey(legionId))
            {
                legionDict.Add(legionId, select);
            }

            CheckOver();
        }

        /// <summary>
        /// 检测结果
        /// </summary>
        private void CheckOver(bool isEnd = false)
        {

            Debug.Log("~~投降投票 CheckOver isEnd=" + isEnd);
            List<LegionData> list = room.sceneData.OnlineLegionList;
            ballotNum = 0;
            ballotTotal = list.Count;


            okNum = 0;
            noNum = 0;
            overSelect = TSBallotState.None;

            foreach (LegionData legion in list)
            {
                if (legionDict.ContainsKey(legion.legionId))
                {
                    ballotNum++;
                    if(legionDict[legion.legionId] == TSBallotState.Yes)
                    {
                        okNum++;
                    }
                    else
                    {
                        noNum++;
                    }
                }
            }


            if (ballotNum >= ballotTotal)
            {
                overSelect = TSBallotState.No;
                if (okNum >= ballotTotal)
                {
                    overSelect = TSBallotState.Yes;
                }

                if(!isEnd)
                {
                    Stop();
                }
            }

            if(isEnd && overSelect == TSBallotState.None)
            {
                overSelect = TSBallotState.No;
            }


            Debug.Log("~~投降投票 CheckOver changeEvent=" + changeEvent);
            if (changeEvent != null)
            {
                changeEvent();
            }

            if (overSelect == TSBallotState.Yes)
            {
                if (photonView.isMine)
                {
                    room.sceneData.GameStageOver(WarOverType.Fail);
                }
            }
        }


        
        
        public PhotonView photonView;
        public void OnSyncedUpdate()
        {

            if (state != State.Begin)
                return;



            lefttime -= LTime.deltaTime;
            if(lefttime <= 0)
            {
                Stop();
                room.clientOperationUnit.ResetSurrender();
            }
        }



    }
}
