using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    public class UnitDoor : AbstractUnitMonoBehaviour
    {
        public RVOObstacle[] obstracles;
        public override void Init(UnitData unitData)
        {
            base.Init(unitData);
            obstracles = GetComponentsInChildren<RVOObstacle>();
        }

        public void OpenDoor()
        {
            if(obstracles != null)
            {
                foreach(RVOObstacle item in obstracles)
                {
                    if(!item.enabled) item.enabled = true;
                }
            }
        }

        public void CloseDoor()
        {
            if (obstracles != null)
            {
                foreach (RVOObstacle item in obstracles)
                {
                    if (item.enabled) item.enabled = false;
                }
            }
        }

    }

}