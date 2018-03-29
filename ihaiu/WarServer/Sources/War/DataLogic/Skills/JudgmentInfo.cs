using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class JudgmentInfo
    {
        public  JudgmentTargger                         judgmentTargger             = JudgmentTargger.None;
        public  int                                     sameTypeCount               = 0;
        public  UnitType                                unitType;
        public  float                                   disTarget                   = 0;
        public  int                                     randomRangeCount            = 0;
        public  int                                     buffCount                   = 0;
        public  int                                     monsterCount                = 0;
        public  int                                     propId                      = 0;
        public  float                                   propVal                     = 0;
        public  PropUnit                                prop;
    }
}