using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    /// <summary>
    /// 移动物体
    /// </summary>
    public class StoryStageMove
    {
        public  static void skip()
        {
            if (tweenerList.Count > 0)
            {
                for (int i = 0; i < tweenerList.Count; i++)
                {
                    tweenerList[i].Kill(true);
                }
            }
        }

        public static void Init()
        {
            tweenerList = new List<Tweener>();
        }

        private static List<Tweener>    tweenerList = new List<Tweener>();

        public static void Move(Transform mTrans, Vector3[] path, float duration, bool lookAt, TweenCallback call)
        {
            Tweener LookTweener = null;
            if (lookAt)
            {
                LookTweener = mTrans.DOPath(path, duration, PathType.CatmullRom).SetLookAt(0.001F);
            }
            else
            {
                LookTweener = mTrans.DOPath(path, duration, PathType.CatmullRom);
            }
            LookTweener.SetEase(Ease.Linear);
            LookTweener.SetAutoKill(true);
            LookTweener.OnComplete(call);
            tweenerList.Add(LookTweener);
        }

        public static void Rotate(Transform mTrans, Vector3 endValue, float duration, TweenCallback call)
        {
            Tweener LookTweener = mTrans.DORotate(endValue,duration,RotateMode.Fast);
            LookTweener.SetAutoKill(true);
            LookTweener.OnComplete(call);
            tweenerList.Add(LookTweener);
        }
    }
}