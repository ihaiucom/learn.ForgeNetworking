using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class UnitBloodBag : MonoBehaviour
    {
        public  GameObject[]    ShowHideObjs;
        [HideInInspector]
        public bool             bShowHide = true;
        public void OnShowHide(bool show = true)
        {
            if (bShowHide != show)
            {
                for (int i = 0; i < ShowHideObjs.Length; i++)
                {
                    ShowHideObjs[i].SetActive(show);
                }
                bShowHide = show;
            }
        }
    }
}