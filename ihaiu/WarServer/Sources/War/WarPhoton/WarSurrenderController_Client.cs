using Games.PB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Games.Wars
{
    public partial class WarSurrenderController
    {
        /** 是否需要投降 才能退出 */
        public bool RequireSurrender
        {
            get
            {
                return room.isNetModel;
            }
        }

        /** 是否可以退出 */
        public bool EnableExit
        {
            get
            {
                return !RequireSurrender;
            }
        }


        /** 是否可以投降 */
        public bool EnableSurrender
        {
            get
            {
                return OwnVoted == false;
            }
        }

        // 自己是否已经投票
        public bool OwnVoted
        {
            get
            {
                return legionDict.ContainsKey(room.clientOwnLegionId);
            }
        }

        // 投票
        public void SendBattlot(bool isYes)
        {
            room.clientOperationUnit.BallotSurrender(isYes ? TSBallotState.Yes : TSBallotState.No);
        }

    }
}
