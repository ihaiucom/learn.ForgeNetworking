using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{
    /// <summary>
    /// 特效控制器
    /// </summary>
    public class UnitStateEffect : AbstractUnitMonoBehaviour
    {
        // 美术状态管理器
        #region 状态变量
        /// <summary>
        /// 特效对象池
        /// </summary>
        private     List<StateEffectInf>        poolStateEffectInf          = new List<StateEffectInf>();
        /// <summary>
        /// 护盾
        /// </summary>
        private     List<StateEffectInf>        shieldEffectInf             = new List<StateEffectInf>();
        /// <summary>
        /// 冰冻
        /// </summary>
        private     List<StateEffectInf>        stateFreezedEffectInf       = new List<StateEffectInf>();
        /// <summary>
        /// 眩晕
        /// </summary>
        private     List<StateEffectInf>        stateVertigoEffectInf       = new List<StateEffectInf>();
        /// <summary>
        /// 沉默
        /// </summary>
        private     List<StateEffectInf>        stateSilenceEffectInf       = new List<StateEffectInf>();
        /// <summary>
        /// 中毒
        /// </summary>
        private     List<StateEffectInf>        statePosionEffectInf        = new List<StateEffectInf>();
        /// <summary>
        /// 灼烧
        /// </summary>
        private     List<StateEffectInf>        stateBurnEffectInf          = new List<StateEffectInf>();
        /// <summary>
        /// 速度变化
        /// </summary>
        private     List<StateEffectInf>        stateSpeedEffectInf         = new List<StateEffectInf>();
        /// <summary>
        /// 加血
        /// </summary>
        private     List<StateEffectInf>        stateHpRecoverEffectInf     = new List<StateEffectInf>();

        /// <summary>
        /// 护盾
        /// </summary>
        private bool            shield                      = false;
        /// <summary>
        /// 冰冻
        /// </summary>
        private bool            stateFreezed                = false;
        /// <summary>
        /// 眩晕
        /// </summary>
        private bool            stateVertigo                = false;
        /// <summary>
        /// 沉默
        /// </summary>
        private bool            stateSilence                = false;
        /// <summary>
        /// 中毒
        /// </summary>
        private bool            statePosion                 = false;
        /// <summary>
        /// 灼烧
        /// </summary>
        private bool            stateBurn                   = false;
        /// <summary>
        /// 速度变化
        /// </summary>
        private bool            stateSpeed                  = false;
        /// <summary>
        /// 加血
        /// </summary>
        private bool            stateHpRecover              = false;
        #endregion

        #region 状态倒计时
        void timeLimit(ref List<StateEffectInf> list)
        {
            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (LTime.time >= list[i].life)
                    {
                        list[i].Hide();
                        list.RemoveAt(i);
                    }
                }
            }
        }
        #endregion
        #region 清空状态
        void clearState(ref List<StateEffectInf> list)
        {
            if (list.Count > 0)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    list[i].Hide();
                    list.RemoveAt(i);
                }
            }
        }
        #endregion

        private     List<StateEffectInf>        poolBuffEffectInf          = new List<StateEffectInf>();

        #region 获取单位当前状态
        void Update()
        {
            if (poolBuffEffectInf.Count > 0)
            {
                for (int i = poolBuffEffectInf.Count - 1; i >= 0; i--)
                {
                    if (LTime.time >= poolBuffEffectInf[i].life || statusOver(poolBuffEffectInf[i].aimId))
                    {
                        poolBuffEffectInf[i].Hide();
                        poolBuffEffectInf.RemoveAt(i);
                    }
                }
            }

            return;
            #region 护盾
            if (unitData.prop.Shield > 0)
            {
                // 出现护盾
                if (!shield)
                {
                    shield = true;
                }
                timeLimit(ref shieldEffectInf);
            }
            else
            {
                // 护盾消失
                if (shield)
                {
                    shield = false;
                    clearState(ref shieldEffectInf);
                }
            }
            #endregion

            #region 冰冻
            if (unitData.prop.StateFreezed)
            {
                // 被冰冻
                if (!stateFreezed)
                {
                    stateFreezed = true;
                }
                timeLimit(ref stateFreezedEffectInf);
            }
            else
            {
                // 取消冰冻
                if (stateFreezed)
                {
                    stateFreezed = false;
                    clearState(ref stateFreezedEffectInf);
                }
            }
            #endregion

            #region 眩晕
            if (unitData.prop.StateVertigo)
            {
                // 被眩晕
                if (!stateVertigo)
                {
                    stateVertigo = true;
                }
                timeLimit(ref stateVertigoEffectInf);
            }
            else
            {
                // 取消眩晕
                if (stateVertigo)
                {
                    stateVertigo = false;
                    clearState(ref stateVertigoEffectInf);
                }
            }
            #endregion

            #region 沉默
            if (unitData.prop.StateSilence)
            {
                // 被沉默
                if (!stateSilence)
                {
                    stateSilence = true;
                }
                timeLimit(ref stateSilenceEffectInf);
            }
            else
            {
                // 取消沉默
                if (stateSilence)
                {
                    stateSilence = false;
                    clearState(ref stateSilenceEffectInf);
                }
            }
            #endregion

            #region 中毒
            if (unitData.prop.StatePosion)
            {
                // 被中毒
                if (!statePosion)
                {
                    statePosion = true;
                }
                timeLimit(ref statePosionEffectInf);
            }
            else
            {
                // 取消中毒
                if (statePosion)
                {
                    statePosion = false;
                    clearState(ref statePosionEffectInf);
                }
            }
            #endregion

            #region 灼烧
            if (unitData.prop.StateBurn)
            {
                // 被灼烧
                if (!stateBurn)
                {
                    stateBurn = true;
                }
                timeLimit(ref stateBurnEffectInf);
            }
            else
            {
                // 取消灼烧
                if (stateBurn)
                {
                    stateBurn = false;
                    clearState(ref stateBurnEffectInf);
                }
            }
            #endregion

            #region 速度变化
            //if (unitData.prop.MovementSpeed)
            //{
            //    // 被灼烧
            //    if (!stateSpeed)
            //    {
            //        stateSpeed = true;
            //    }
            //    timeLimit(ref stateSpeedEffectInf);
            //}
            //else
            //{
            //    // 取消灼烧
            //    if (stateSpeed)
            //    {
            //        stateSpeed = false;
            //        clearState(ref stateSpeedEffectInf);
            //    }
            //}
            #endregion

            #region 加血
            if (unitData.prop.StateHPRecover)
            {
                // 被加血
                if (!stateHpRecover)
                {
                    stateHpRecover = true;
                }
                timeLimit(ref stateHpRecoverEffectInf);
            }
            else
            {
                // 取消加血
                if (stateHpRecover)
                {
                    stateHpRecover = false;
                    clearState(ref stateHpRecoverEffectInf);
                }
            }
            #endregion
        }
        #endregion

        bool statusOver(int aimId)
        {
            if (aimId == PropId.Shield)
            {

                switch (aimId)
                {
                    case PropId.Shield:
                        {
                            if (unitData.prop.Shield <= 0)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.StateFreezed:
                        {
                            if (!unitData.prop.StateFreezed)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.StateVertigo:
                        {
                            if (!unitData.prop.StateVertigo)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.StateSilence:
                        {
                            if (!unitData.prop.StateSilence)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.StatePosion:
                        {
                            if (!unitData.prop.StatePosion)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.StateBurn:
                        {
                            if (!unitData.prop.StateBurn)
                            {
                                return true;
                            }
                        }
                        break;
                    case PropId.MovementSpeed:
                        {
                           
                        }
                        break;
                    case PropId.StateHPRecover:
                        {
                            if (!unitData.prop.StateHPRecover)
                            {
                                return true;
                            }
                        }
                        break;
                }
            }
            return false;
        }

        public  void PutBuffEffect(string path, float life, Vector3 offset, int aimId)
        {
            if (path == null || path.Length < 2 || life <= 0)
            {
                return;
            }
            StateEffectInf item = poolBuffEffectInf.Find(m => m.path.Equals(path));
            if (item == null)
            {
                item = getStateEffectInf(path, life, offset);
                poolBuffEffectInf.Add(item);
            }
            item.life = Mathf.Max(LTime.time + life, item.life);
            item.aimId = aimId;
        }

        /// <summary>
        /// 添加状态特效路径
        /// </summary>
        /// <param name="propid"></param>
        /// <param name="path"></param>
        /// <param name="life"></param>
        /// <param name="offset"></param>
        /// <param name="bSuperposition"></param>
        public void PutStateEffect(int propid, string path, float life, Vector3 offset, bool bSuperposition)
        {
            if (path == null || path.Length < 2)
            {
                return;
            }
            switch (propid)
            {
                case PropId.Shield:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref shieldEffectInf);
                    }
                    break;
                case PropId.StateFreezed:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateFreezedEffectInf);
                    }
                    break;
                case PropId.StateVertigo:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateVertigoEffectInf);
                    }
                    break;
                case PropId.StateSilence:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateSilenceEffectInf);
                    }
                    break;
                case PropId.StatePosion:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref statePosionEffectInf);
                    }
                    break;
                case PropId.StateBurn:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateBurnEffectInf);
                    }
                    break;
                case PropId.MovementSpeed:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateSpeedEffectInf);
                    }
                    break;
                case PropId.StateHPRecover:
                    {
                        ShowEffect(path, life, offset, bSuperposition, ref stateHpRecoverEffectInf);
                    }
                    break;
            }
        }

        #region 判断并生产特效
        void ShowEffect(string path, float life, Vector3 offset, bool bSuperposition,ref List<StateEffectInf> list)
        {
            if (bSuperposition)
            {
                list.Add(getStateEffectInf(path, life, offset));
            }
            else
            {
                StateEffectInf item = list.Find(m => m.path.Equals(path));
                if (item == null)
                {
                    item = getStateEffectInf(path, life, offset);
                    list.Add(item);
                }
                item.life = Mathf.Max(LTime.time + life, item.life);
            }
        }
        #endregion

        StateEffectInf getStateEffectInf(string path, float life, Vector3 offset)
        {
            StateEffectInf item = poolStateEffectInf.Find(m => m != null && !m.active && m.path.Equals(path));
            if (item == null)
            {
                item = InitEffect(path);
            }
            item.active = true;
            item.tForm.localPosition = offset;
            item.life = LTime.time + life;
            item.gObject.SetActive(true);
            return item;
        }

        /// <summary>
        /// 从对象池中获取特效
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        StateEffectInf InitEffect(string path)
        {
            StateEffectInf item = new StateEffectInf();
            item.path = path;
            item.gObject = room.clientRes.GetGameObjectInstall(path, modelTform);
            item.tForm = item.gObject.transform;
            poolStateEffectInf.Add(item);
            return item;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
        }
        protected override void OnRelease()
        {
            base.OnRelease();
            for (int i = poolStateEffectInf.Count - 1; i >= 0; i--)
            {
                if (poolStateEffectInf[i] != null && poolStateEffectInf[i].gObject != null)
                {
                    Destroy(poolStateEffectInf[i].gObject);
                }
            }
            poolStateEffectInf.Clear();
        }
    }
    
}