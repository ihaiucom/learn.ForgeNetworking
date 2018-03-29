using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 随身特效对象 ,UnitStateEffect调用
    /// </summary>
    public class StateEffectInf
    {
        public  bool            active      = false;
        public  float           life        = -1;
        public  string          path;
        public  GameObject      gObject;
        public  Transform       tForm;
        public  int             aimId;

        public void Hide()
        {
            life = -1;
            gObject.SetActive(false);
            active = false;
        }
    }
}