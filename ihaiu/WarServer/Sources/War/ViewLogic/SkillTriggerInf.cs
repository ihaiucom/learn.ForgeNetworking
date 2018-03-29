using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class SkillTriggerInf
    {
        public  bool                    bLife;
        public  float                   startTime;
        public  int                     legionId;
        public  UnitData                origin;
        public  UnitData                target;
        public  int                     skilllv;
        public  SkillTriggerEvent       skillTriggerEvent;
    }
}