using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Games.Wars
{
    public class ProjectileList
    {
        public SkillDate                skillDate;
        public UnitAgent                unitAgent;
        public SkillTriggerEvent        skillTriggerEvent;
        public Projectile               projectile;
        public float                    startTime;
        public float                    nextTime;
        public float                    endTime;
        public int                      maxCount                = 0;
    }
}