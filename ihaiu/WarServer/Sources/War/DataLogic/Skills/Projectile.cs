namespace Games.Wars
{
    public class Projectile
    {
        /// <summary>
        /// 投射物的方向和移动方法
        /// </summary>
        public  ProjectileMoveMethod                    projectileMoveMethod;
        public  float                                   searchDis                   = 0;
        /// <summary>
        /// 最大投射物的创建数量
        /// </summary>
        public  int                                     maxProjectiles              = 1;
        /// <summary>
        /// cd间隔
        /// </summary>
        public  float                                   createCD                    = 0;
        /// <summary>
        /// 多重箭角度
        /// </summary>
        public  float                                   angleFromEach               = 45;
        /// <summary>
        /// 移动速度
        /// </summary>
        public  float                                   moveSpeed                   = 1;
        /// <summary>
        /// 撞检测宽度
        /// </summary>
        public  float                                   collisionRayWidth           = 0.02F;
        /// <summary>
        /// 最大飞行距离
        /// </summary>
        public  float                                   fizzleDistance              = 1;
        /// <summary>
        /// 最大穿透数量
        /// </summary>
        public  int                                     hitCount                    = 1;
        /// <summary>
        /// 是否抛物线
        /// </summary>
        public  bool                                    bRotation                   = false;
        /// <summary>
        /// 立即生效伤害详情
        /// </summary>
        public  DamageInfo                              damageInfo                  = new DamageInfo();
        /// <summary>
        /// 是否仅仅存在二次伤害
        /// </summary>
        public  bool                                    onlySecondDamage            = false;
        /// <summary>
        /// 二次伤害详情
        /// </summary>
        public  SecondTarget                            secondTarget                = new SecondTarget();
        /// <summary>
        /// 子弹音效
        /// </summary>
        public  string                                  projectMusic                = "";
    }
}
