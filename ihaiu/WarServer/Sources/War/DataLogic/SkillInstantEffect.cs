using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class SkillInstantEffect
    {
        /// <summary>
        /// 攻击数据
        /// </summary>
        public  SkillDate           skillDate;
        public  int                 legionId            = -1;
        /// <summary>
        /// 攻击目标
        /// </summary>
        public  UnitData            unitData            = null;
        public  string              path                = "";
        public  GameObject          gObject;
        public  Transform           tForm;
        /// <summary>
        /// 是否使用中
        /// </summary>
        public  bool                bActive             = false;
        /// <summary>
        /// 是否显示中
        /// </summary>
        public  bool                bShow               = false;
        public  float               life;
        public  float               startTime;
        public  float               cd;
        /// <summary>
        /// 已经产生过伤害的列表
        /// </summary>
        public  Dictionary<int,int>   HaveDamageDic       = new Dictionary<int, int>();
        public  bool                bBullet             = false;
        public  SkillTriggerEvent   skillTriggerEvent;
        public  Projectile          projectile;


        public  ProjectileMoveMethod projectileMoveMethod;
        public  bool                bRotate             = false;
        public  Vector3             endPos;
        public  float               endDis;
        public  int                 hitCount;
        /// <summary>
        /// 移动了多少距离了
        /// </summary>
        private  float               haveMoveDistance;
        /// <summary>
        /// 上一帧位置
        /// </summary>
        private  Vector3             proPosition;
        /// <summary>
        /// 子弹的音效
        /// </summary>
        public StudioEventEmitter       eventEmitter;
        public bool HaveMove
        {
            get
            {
                if (projectile != null)
                {
                    haveMoveDistance += Vector3.Distance(proPosition, tForm.position);
                    proPosition = tForm.position;
                    if (haveMoveDistance >= projectile.fizzleDistance)
                    {
                        return true;
                    }
                    else
                    {
                        if (Vector3.Distance(tForm.position, endPos) <= 0.02F)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
        }

        public void showHideEffect(bool active)
        {
            if (!active)
            {
                unitData = null;
                skillDate = null;
                legionId = -1;
                skillTriggerEvent = null;
                projectile = null;
                bBullet = false;
                HaveDamageDic.Clear();
                bRotate = false;
                if (eventEmitter != null)
                {
                    eventEmitter.Stop();
                }
            }
            else
            {
                proPosition = tForm.position;
                haveMoveDistance = 0;
                if (eventEmitter != null)
                {
                    eventEmitter.Play();
                }
            }
            bShow = active;
            bActive = active;
            if (gObject != null)
            {
                gObject.SetActive(active);
            }
        }

    }
}