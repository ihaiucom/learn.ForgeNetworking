using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class PassiveTriggercondition
    {
        public  Triggercondition                        triggercondition            = Triggercondition.None;
        public  bool                                    andor                       = false;
        public  List<JudgmentInfo>                      judgmentInfoList            = new List<JudgmentInfo>();
    }
}
