using Games.PB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      9/19/2017 8:24:46 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /** 战斗操作单位
     * 英雄控制----移动  释放技能
     *  */
    public partial class WarOperationUnit : AbstractRoomObject
    {
        public WarOperationUnit(WarRoom room)
        {
            this.room = room;
        }


        internal void SetEasyTouch()
        {
            room.drive.StartCoroutine(DelaySetEasyTouch());
        }

        IEnumerator DelaySetEasyTouch()
        {
            yield return new WaitForSeconds(1);
            EasyTouch.RemoveCamera(EasyTouch.GetCamera(0));
            EasyTouch.AddCamera(Game.camera.main);
        }

        public void Start()
        {
            //先清空之前的点击事件
            Stop();

            EasyTouch.On_SimpleTap += On_SimpleTap;
            EasyTouch.On_LongTapStart += On_LongTapStart;
            EasyTouch.On_LongTapEnd += On_LongTapEnd;
            UIHandler.OnJoyPos += UIHandler_OnJoyPos;
            UIHandler.OnJoyPos2 += UIHandler_OnJoyPos2;
            UIHandler.OnButtonClick += OnUseSkill;
            //UIHandler.OnPropUnitState += OnPropUnitState;
            UIHandler.OnUIButtonClick += OnUIButtonClick;
            //UIHandler.AnimatorEventIntEvent += AnimatorEventIntEvent;

            UIHandler.OnUIHandlerSkillBullet += OnUIHandlerSkillBullet;
        }

        public void Stop()
        {
            EasyTouch.On_SimpleTap -= On_SimpleTap;
            EasyTouch.On_LongTapStart -= On_LongTapStart;
            EasyTouch.On_LongTapEnd -= On_LongTapEnd;
            EasyTouch.RemoveCamera(Game.camera.main);
            UIHandler.OnJoyPos -= UIHandler_OnJoyPos;
            UIHandler.OnJoyPos2 -= UIHandler_OnJoyPos2;
            UIHandler.OnButtonClick -= OnUseSkill;
            //UIHandler.OnPropUnitState -= OnPropUnitState;
            UIHandler.OnUIButtonClick -= OnUIButtonClick;
            //UIHandler.AnimatorEventIntEvent -= AnimatorEventIntEvent;

            UIHandler.OnUIHandlerSkillBullet -= OnUIHandlerSkillBullet;
        }

        #region 点击事件
        public  int clickuid = -1;
        // 点击
        void On_SimpleTap(Gesture gesture)
        {
            if (gesture.pickedObject != null)
            {
                //bool bClickCanRecover = false;
                switch (1 << gesture.pickedObject.layer)
                {
                    case (int)GameLayer.WarSceneBuildCell:
                        {
                            // 点中了建筑格子
                            BuildCellAgent buildCellAgent = gesture.pickedObject.GetComponent<BuildCellAgent>();
                            if (buildCellAgent != null)
                            {
                                BuildCellData cell = buildCellAgent.buildCellData;
                                // 建筑格子上有单位
                                if (cell != null && cell.hasUnit)
                                {
                                    clickuid = cell.unit.uid;

                                    if (cell.unit.legionData.IsNeutral)
                                    {
                                        // 占领
                                        if (cell.unit.enableOccupy)
                                        {
                                            BeginOccupyUnit(clickuid);
                                        }

                                        // 雇佣
                                        if (cell.unit.enableEmploy)
                                        {
                                            if (!cell.unit.IsEmploying)
                                            {
                                                BeginEmployUnit(clickuid);
                                            }
                                        }
                                    }
                                    else
                                    {

                                        //// 回收机关操作模式
                                        //if (WarUI.Instance.bRecover)
                                        //{
                                        //    if (cell.unit.GetRelationType(legionId) != RelationType.Enemy)
                                        //    {
                                        //        bClickCanRecover = true;
                                        //        RecoveryUnit(clickuid);
                                        //    }
                                        //}
                                        //// 修理
                                        //if (!cell.unit.isLive && cell.unit.GetRelationType(legionId) != RelationType.Enemy)
                                        //{
                                        //    BeginRebuildUnit(clickuid);
                                        //}
                                        if (cell.unit.isLive && room.stageType != StageType.PVPLadder)
                                        {
                                            room.clientViewAgent.ShowUnitMenu(cell.unit);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
                // 更新回收状态
                //WarUI.Instance.bRecover = bClickCanRecover;
                //SetBuildSelectSelectModel(WarUI.Instance.bRecover);
            }
        }

        // 长按开始
        void On_LongTapStart(Gesture gesture)
        {
            if (gesture.pickedObject != null)
            {
                switch (1 << gesture.pickedObject.layer)
                {
                    case (int)GameLayer.WarSceneBuildCell:
                        {
                            BuildCellAgent buildCellAgent = gesture.pickedObject.GetComponent<BuildCellAgent>();
                            if (buildCellAgent != null)
                            {
                                BuildCellData _BuildCellData = buildCellAgent.buildCellData;
                                // 长按显示攻击范围提示
                                if (_BuildCellData != null && _BuildCellData.unit != null && _BuildCellData.hasUnit)
                                {
                                    TowerTipModel tip = WarUI.Instance.mPoolTipTower.Find(m => m.TowerId == 0);
                                    if (tip == null)
                                    {
                                        tip = new TowerTipModel();
                                        tip.Gobject = room.clientRes.GetGameObjectInstall("PrefabWar/UnitTip");
                                        tip.TowerId = 0;
                                        tip.bActive = false;
                                        WarUI.Instance.mPoolTipTower.Add(tip);
                                    }
                                    UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(_BuildCellData.unit.unitId, _BuildCellData.unit.unitLevel);
                                    tip.OnShow(buildCellAgent.position, Vector3.zero, unitLevelConfig.GetPropVal(PropId.AttackRadius));
                                }
                            }
                        }
                        break;
                }
            }
        }
        // 长按结束
        void On_LongTapEnd(Gesture gesture)
        {
            // 长按结束，取消攻击范围提示
            List< TowerTipModel > TowerTipModellist = WarUI.Instance.mPoolTipTower.FindAll(m => m.bActive);
            if (TowerTipModellist.Count > 0)
            {
                foreach (TowerTipModel item in TowerTipModellist)
                {
                    item.OnHide();
                }
            }
        }

        #endregion

        #region joy操作

        /// <summary>
        /// 左侧移动位置的旋钮
        /// </summary>
        void UIHandler_OnJoyPos(WarUIAttackType btid, Vector3 pos, bool run, float realPos)
        {
            if (!enableMove) return;

            syncJoyMove.type = btid;
            syncJoyMove.pos = pos;
            syncJoyMove.run = run;
            syncJoyMove.realPos = realPos;
            syncJoyMove.isUpdated = false;
            syncJoyMove.isSend = false;


            //unitAgent.warUnitcontrol.UIHandler_OnJoyPos(btid, _Pos, _Run, _RealPos);

            //room.clientUnitcontrol.UIHandler_OnJoyPos(btid, _Pos, _Run, _RealPos);
            //return;
            //if (unitData != null && unitAgent.unitControl != null && unitData.unitType == UnitType.Hero && unitData.clientIsOwn)
            //{
            //    if (_Run)
            //    {
            //        unitAgent.StopMove();
            //    }
            //    unitAgent.unitControl.UIHandler_OnJoyPos(btid, _Pos, _Run, _RealPos);
            //}
        }
        public MoveJoy              syncJoyMove             = new MoveJoy();
        public MoveJoy              syncJoySkill            = new MoveJoy();
        public SkillStateJoy        syncJoySkillState       = new SkillStateJoy();
        public Ballot               syncBallotSpawnWaveSkip = new Ballot();
        public Ballot               syncSurrender      = new Ballot();

        /// <summary>
        /// 跳过波次
        /// </summary>
        public void BallotSpawnWaveSkip()
        {
            syncBallotSpawnWaveSkip.isUpdated = false;
            syncBallotSpawnWaveSkip.isSend = false;
            syncBallotSpawnWaveSkip.ballot = TSBallotState.Yes;
        }

        // 跳过波次重置
        public void ResetBallotSpawnWaveSkip()
        {
            syncBallotSpawnWaveSkip.ballot = TSBallotState.None;
        }

        /// <summary>
        /// 投降投票
        /// </summary>
        public void BallotSurrender(TSBallotState select)
        {
            syncSurrender.isUpdated = false;
            syncSurrender.isSend = false;
            syncSurrender.ballot = select;
        }

        // 投降投票重置
        public void ResetSurrender()
        {
            syncSurrender.ballot = TSBallotState.None;
        }


        /// <summary>
        /// 右侧控制技能方向的旋钮
        /// </summary>
        void UIHandler_OnJoyPos2(WarUIAttackType btid, Vector3 pos, bool run, float realPos)
        {
            if (!enableAttack) return;
            syncJoySkill.isUpdated = false;
            syncJoySkill.isSend = false;

            syncJoySkill.type = btid;
            syncJoySkill.pos = pos;
            syncJoySkill.run = run;
            syncJoySkill.realPos = realPos;

        }
        #endregion

        #region 使用技能
        void OnUseSkill(WarUIAttackType type, ButtonState openClose, bool hold, bool outButton, int bulletCount)
        {
            if (!enableAttack) return;
            syncJoySkillState.isUpdated = false;
            syncJoySkillState.isSend = false;

            syncJoySkillState.stateType = type;
            syncJoySkillState.openClose = openClose;
            syncJoySkillState.bulletCurrentCount = bulletCount;

            //TODO
            //unitAgent.warUnitcontrol.OnUseSkill(_Type, _OpenClose);

            //room.clientUnitcontrol.OnUseSkill(_Type, _OpenClose);
            //return;
            //if (unitData.unitType == UnitType.Hero && unitData.clientIsOwn && unitAgent.unitControl != null)
            //{
            //    unitAgent.unitControl.OnUseSkill(_Type, _OpenClose, Auto, bHold);
            //}
        }
        #endregion

        #region 更新子弹弹夹数量
        private UITowerPanel uITowerPanel;
        void OnUIHandlerSkillBullet(int uid, int skillId, int bulletCount, bool openClose, UITowerPanel uITowerPanel)
        {
            if (unitUid == uid)
            {
                this.uITowerPanel = uITowerPanel;
                if (!unitData.switchSkillBuffDic.ContainsKey(skillId))
                {
                    unitData.switchSkillBuffDic.Add(skillId, bulletCount);
                }
                else
                {
                    unitData.switchSkillBuffDic[skillId] += bulletCount;
                }
                if (openClose)
                {
                    if (bulletCount > 0 && !unitData.switchSkillList.Contains(skillId))
                    {
                        unitData.switchSkillList.Add(skillId);
                    }
                }
                else
                {
                    if (unitData.switchSkillList.Contains(skillId))
                    {
                        unitData.switchSkillList.Remove(skillId);
                    }
                }
            }
        }
        public int BulletUpdate(int maxCount)
        {
            int result = 0;
            if (uITowerPanel != null)
            {
                if (uITowerPanel.bulletCurrent >= maxCount)
                {
                    result = maxCount;
                    uITowerPanel.bulletCurrent -= maxCount;
                }
                else
                {
                    result = uITowerPanel.bulletCurrent;
                    uITowerPanel.bulletCurrent = 0;
                }
                uITowerPanel.UpdateBulletCount();
            }
            return result;
        }
        #endregion

        private float _t = 0;
        public void Update()
        {
            if (unitData != null)
            {
                Game.camera.GrayScreen = !unitData.isLive;
            }

            if (Input.anyKeyDown)
            {
                EndRebuildUnit(false);
            }
        }

        #region 是否回收机关按钮回调
        void OnUIButtonClick(WarUIType warUIType, bool State)
        {
            //if (warUIType == WarUIType.Recycle)
            //{
            //    SetBuildSelectSelectModel(State);
            //}
        }
        #endregion

        #region 动作Event回调
        /// <summary>
        /// 动作回调，依据动作判断释放特效开始时间
        /// </summary>
        /// <param name="UnitUid"></param>
        /// <param name="Index"></param>
        void AnimatorEventIntEvent(int UnitUid, int Index)
        {
            UnitAgent _unitAgent = unitAgent;
            if (unitAgent.uid != UnitUid)
            {
                _unitAgent = room.clientSceneView.GetUnit(UnitUid);
            }
            if (_unitAgent == null)
            {
                return;
            }
            switch (Index)
            {
                case 555:
                    {
                        // 死亡动作完成，删除或等待复活
                        //if (_unitAgent.unitData.unitType == UnitType.Build)
                        //{
                        //    BuildCellAgent buildCellAgent = room.clientSceneView.GetBuildCell(_unitAgent.unitData.buildCellUid);
                        //    buildCellAgent.SetWaitReLife();
                        //}
                        //else
                        //{
                        //_unitAgent.UnitDestroy();
                        //}
                    }
                    break;
                    //case 666:
                    //    {
                    //        // 复活完毕
                    //        _unitAgent.RestLifeFinal();
                    //    }
                    //    break;
            }
            //return;
            //UnitAgent _unitAgent = unitAgent;
            //if (unitAgent.uid != UnitUid)
            //{
            //    _unitAgent = room.clientSceneView.GetUnit(UnitUid);
            //}
            //if (_unitAgent == null || _unitAgent.unitSkillEffect == null)
            //{
            //    return;
            //}
            //switch (Index)
            //{
            //    case 1001:
            //        {
            //            // 附加特效
            //        }
            //        break;
            //    case 900:
            //        {
            //            // 开启起手特效
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent,WarSkillEffectType.ShakeBefore);
            //        }
            //        break;
            //    case 901:
            //        {
            //            // 开启后摇特效
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.ShakeBehind);
            //        }
            //        break;
            //    case 1:
            //        {
            //            // 普通攻击特效开启点
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.NormalAttack);
            //        }
            //        break;
            //    case 5:
            //        {
            //            // 左攻击
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.LeftAttack);
            //        }
            //        break;
            //    case 6:
            //        {
            //            // 右攻击
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.RightAttack);
            //        }
            //        break;
            //    case 10:
            //        {
            //            // 技能1攻击特效开启点
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.Skill1);
            //        }
            //        break;
            //    case 20:
            //        {
            //            // 技能2攻击特效开启点
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.Skill2);
            //        }
            //        break;
            //    case 30:
            //        {
            //            // 技能3攻击特效开启点
            //            _unitAgent.unitSkillEffect.OnShowEffect(_unitAgent, WarSkillEffectType.Skill3);
            //        }
            //        break;
            //    case 110: //普通攻击
            //    case 111: //技能1攻击
            //    case 112: //技能2攻击
            //    case 113: //技能3攻击
            //    case 114: //技能4攻击
            //        {
            //            // 攻击点，对敌人发起攻击，一般普通攻击依据动作判断攻击伤害，技能攻击依据特效判断攻击伤害
            //            OnSendAttackOther(_unitAgent, Index);
            //        }
            //        break;
            //    case 555:
            //        {
            //            if (_unitAgent.unitData.unitType == UnitType.Build)
            //            {
            //                BuildCellAgent buildCellAgent = room.clientSceneView.GetBuildCell(_unitAgent.unitData.buildCellUid);
            //                buildCellAgent.SetWaitReLife();
            //            }
            //            else
            //            {
            //                _unitAgent.UnitDestroy();
            //            }
            //        }
            //        break;
            //    case 666:
            //        {
            //            // 复活完毕
            //            _unitAgent.RestLifeFinal();
            //        }
            //        break;
            //}
        }
        #endregion

        #region 发送攻击数据


        /// <summary>
        /// 是否技能产生伤害数据
        /// </summary>
        /// <param name="_unitAgent">攻击者</param>
        /// <param name="Index">哪个技能</param>
        //public void OnSendAttackOther(UnitAgent _unitAgent, int Index)
        //{
        //    Index -= 110;
        //    int skillid = _unitAgent.unitData.GetSkillByIndex(Index).skillId;
        //    OnSendAttackOtherBySkillId(_unitAgent, skillid);
        //}
        /*
        /// <summary>
        /// 是否技能产生伤害数据
        /// </summary>
        /// <param name="_unitAgent">攻击者</param>
        /// <param name="Index">哪个技能</param>
        public void OnSendAttackOtherBySkillId(UnitAgent _unitAgent, int skillid)
        {
            SkillInfo skillInfo = _unitAgent.skillInfo(skillid);
            //unitData.GetSkillByIndex(i).skillId

            if (skillInfo != null && !skillInfo.mBAOE && !skillInfo.mBullet)
            {
                //SkillInfo skillInfo = _unitAgent.mSkillInfoList[Index];
                if (skillInfo.mBufferId > 0)
                {
                    // 自身获取buff
                    room.haoleBuff.AddBuffForUnit(_unitAgent.unitData, _unitAgent.buffInfo(skillInfo.mBufferId), skillInfo.mSkillId);
                }
                if (skillInfo.mHaloId > 0)
                {
                    // 自身获取光环
                }
                if (skillInfo.mToBufferId > 0)
                {
                    // 被攻击者获取buff
                    int _count = 0;
                    List<UnitData> TemUnitDataList = room.sceneData.SearchUnitListInRule(_unitAgent.unitData, skillInfo.mUnitType, skillInfo.mRelationType, skillInfo.mUnitSpaceType, skillInfo.mAttackDis,_unitAgent.position,out _count,true,skillInfo.mWarSkillRule,skillInfo.mRandomAttack);
                    if (_count > 0)
                    {
                        for (int i = 0; i < _count; i++)
                        {
                            room.haoleBuff.AddBuffForUnit(TemUnitDataList[i], _unitAgent.buffInfo(skillInfo.mToBufferId), skillInfo.mSkillId);
                        }
                    }
                    //room.haoleBuff.AddBuffForUnit(_unitAgent.unitData, _unitAgent.mBuffInfo);
                }
                if (skillInfo.mToHaloId > 0)
                {
                    // 被攻击者获取光环
                }

                if (skillInfo.mInitHaloId > 0)
                {
                    // 地面生成光环
                    //BuffInfo bf = new BuffInfo();
                    skillInfo.mLegionId = _unitAgent.unitData.LegionId;
                    room.haoleBuff.AddHalo(skillInfo, _unitAgent.buffInfo(skillInfo.mInitHaloId), unitAgent.mSkillShowPos);
                }
                if (skillInfo.mDisplacement > 0)
                {
                    // 位移技能，进行位移
                    _unitAgent.unitControl.MoveUnitFromOther(skillInfo, skillInfo.mDisplacement, skillInfo.mDisplacementSpeed);
                }
            }
            if (skillInfo == null || skillInfo.mDamage)
            {
                // 产生伤害
                if (_unitAgent == null)
                {
                    return;
                }
                DamageData Dd           = new DamageData();
                Dd.AttackSend = _unitAgent.unitData;
                Dd.AttakcVal = (int)_unitAgent.unitData.prop.PhysicalAttack;
                Dd.bCrit = false;
                Dd.skillid = skillid;
                if (_unitAgent.uid == unitAgent.uid)
                {
                    //TODO 临时攻击攻击
                    UnitSpaceType spaceType = UnitSpaceType.All;
                    //SkillInfo skillInfo = _unitAgent.mSkillInfoList[Index];
                    Dd.AttakcVal = skillInfo.mDanageVal;
                    int _count = 0;
                    if (skillInfo == null)
                    {
                        Dd.AttackBy = room.sceneData.SearchUnitList(_unitAgent.unitData, UnitType.Solider, RelationType.Enemy, spaceType, _unitAgent.unitData.prop.RadarRadius);
                        _count = Dd.AttackBy.Count;
                    }
                    else
                    {
                        Dd.AttackBy = room.sceneData.SearchUnitListInRule(_unitAgent.unitData, skillInfo.mUnitType, skillInfo.mRelationType, skillInfo.mUnitSpaceType, skillInfo.mAttackDis, _unitAgent.position, out _count, true, skillInfo.mWarSkillRule, skillInfo.mRandomAttack);
                    }
                    for (int i = 0; i < _count; i++)
                    {
                        Dd.AttackBy[i].OnTakeDamage(Dd);
                    }
                }
                else
                {
                    Dd.AttackBy = new List<UnitData>();
                    if (skillInfo != null)
                    {
                        //SkillInfo skillInfo = null;
                        //skillInfo = _unitAgent.mSkillInfoList[Index];
                        Dd.AttakcVal = skillInfo.mDanageVal;
                        //if (skillInfo.mBAOE)
                        {
                            int _count = 0;
                            Dd.AttackBy = room.sceneData.SearchUnitListInRule(_unitAgent.unitData, skillInfo.mUnitType, skillInfo.mRelationType, skillInfo.mUnitSpaceType, skillInfo.mAttackDis, _unitAgent.position + _unitAgent.forward * skillInfo.mAOEPoint, out _count, true, skillInfo.mWarSkillRule, skillInfo.mRandomAttack);

                            for (int i = 0; i < _count; i++)
                            {
                                Dd.AttackBy[i].OnTakeDamage(Dd);
                            }
                            return;
                        }
                    }
                    if (_unitAgent.CurrentAttackUnitData != null)
                    {
                        Dd.AttackBy.Add(_unitAgent.CurrentAttackUnitData);
                        _unitAgent.CurrentAttackUnitData.OnTakeDamage(Dd);
                    }
                }
            }
            //AttackTarget = room.sceneData.SearchMinUnitWithList(unitData, EnemyList);
        }
        */
        //public  UnitData    AttackTarget;

        #endregion

        private bool enableMove
        {
            get
            {
                return unitData != null && unitData.isLive && !unitData.prop.IsFreezed && unitData.prop.MovementSpeed > 0;

            }
        }

        private bool enableAttack
        {
            get
            {
                return unitData != null && unitData.isLive && !unitData.prop.IsFreezed;
            }
        }

        /** 操作的UnitData */
        private UnitData unitData;
        /** 操作的UnitAgent */
        private UnitAgent unitAgent;

        public LegionData legionData
        {
            get
            {
                if (unitData == null) return null;
                return unitData.legionData;
            }
        }

        /// <summary>
        /// 当前能量
        /// </summary>
        public float Energy
        {
            get
            {
                if (legionData == null) return 0;
                return legionData.Energy;
            }

            set
            {
                legionData.Energy = value;
            }
        }


        /// <summary>
        /// 能量上限
        /// </summary>
        public float EnergyMax
        {
            get
            {
                return legionData.EnergyMax;
            }

            set
            {
                legionData.EnergyMax = value;
            }
        }

        public bool IsLive
        {
            get
            {
                if (unitData == null) return true;
                return unitData.isLive;
            }
        }

        public int ReliveSecond
        {
            get
            {
                return legionData.reliveSecond;
            }
        }

        /** 设置操作的单位 */
        public void SetUnitAgent(UnitAgent unitAgent)
        {
            if (this.unitAgent != null)
            {
                Loger.LogWarning("WarOperationUnit:SetUnitAgent 设置操作的单位 已经设置过了");
                return;
            }

            this.unitAgent = unitAgent;
            this.unitData = unitAgent.unitData;
        }

        /** 获取操作的单位 */
        public UnitAgent GetUnitAgent()
        {
            return unitAgent;
        }

        public UnitData GetUnitData()
        {
            return unitData;
        }

        private bool isSkillAttack = false;
        public void OnSkillEnterCD()
        {
            if (unitData != null)
            {
                if (isSkillAttack != unitData.isSkillAttack)
                {
                    isSkillAttack = unitData.isSkillAttack;
                    for (int i = 0; i < 2; i++)
                    {
                        WarUI.Instance.uiPointerForSkill[i].enabled = !isSkillAttack;
                        WarUI.Instance.mPlayerSkillUI[i].afterSkillCD.SetActive(isSkillAttack);
                    }
                }
            }
        }

        /** 获取操作的单位 */
        public int unitUid
        {
            get
            {
                return unitData.uid;
            }
        }

        /** 获取操作的势力ID */
        public int legionId
        {
            get
            {
                return unitData.legionId;
            }
        }

        /** 获取单位位置 */
        public Vector3 position
        {
            get
            {
                if (unitAgent != null)
                {
                    return unitAgent.position;
                }
                return Vector3.zero;
            }
        }


        /** 移动到某个位置 */
        public void MoveTo(Vector3 position)
        {

        }

        /** 设置槽哪个方向移动 */
        public void Move(Vector2 direction)
        {

        }

        /// <summary>
        /// 停止移动，也就是摇杆不再操控
        /// </summary>
        public void StopMove()
        {

        }
        /** 使用技能 */
        public void UseSkill(int skillUid)
        {

        }



        /// <summary>
        /// 放置机关
        /// </summary>
        public bool CreateTowerUnit(WarUIType warUIType)
        {
            BuildCellAgent cell = room.clientSceneView.GetSelectBuildCell();
            if (cell == null)
            {
                // 没有选中的建筑格子
                return false;
            }
            int index = (int)warUIType - (int)WarUIAttackType.Tower1;
            // 依据warUIType类型，判断是建造那个机关
            WarEnterUnitData enterUnitData = room.enterData.ownTowerList[index];

            room.punScene.CreateTowerUnit(cell.uid, 0, legionId, enterUnitData.unitId, enterUnitData.unitLevel);



            return true;
        }


        /// <summary>
        /// 建筑格子 设置为选择模式
        /// </summary>
        public void SetBuildSelectSelectModel(bool selectOrUnselect)
        {
            if (selectOrUnselect)
            {
                room.clientSceneView.SetBuildSelectSelectModel();
            }
            else
            {
                room.clientSceneView.CancelBuildSelectSelectModel();
            }
        }


        /// <summary>
        /// 建筑格子 取消为选择模式，点击空白地区，也表示为取消
        /// </summary>
        public void CancelBuildSelectSelectModel()
        {
            room.clientSceneView.CancelBuildSelectSelectModel();
        }




        //===================================================
        // 机关操作--占领操作
        //---------------------------------------------------
        #region 机关操作--回收操作

        public RecoveryUnit syncRecoveryUnit = new RecoveryUnit();
        /** 回收操作 */
        public void BeginRecoveryUnit(int towerUid)
        {
            syncRecoveryUnit.towerUid = towerUid;
            syncRecoveryUnit.operateHeroUid = unitUid;
            syncRecoveryUnit.state = AsyncOperateState.Begin;
            syncRecoveryUnit.isUpdated = false;
            syncRecoveryUnit.isSend = false;
        }

        public void EndRecoveryUnit(int towerUid, bool isComplete)
        {
            syncRecoveryUnit.towerUid = towerUid;
            syncRecoveryUnit.operateHeroUid = unitUid;
            syncRecoveryUnit.state = isComplete ? AsyncOperateState.Complete : AsyncOperateState.Cancel;
            syncRecoveryUnit.isUpdated = false;
            syncRecoveryUnit.isSend = false;
        }


        #endregion


        #region 机关操作--占领操作
        /** 开始占领操作 */
        public void BeginOccupyUnit(int towerUid)
        {
            room.clientNetC.BeginOccupyUnit(towerUid, unitUid);
        }


        /// <summary>
        /// 结束占领操作
        /// </summary>
        /// <param name="towerUid"></param>
        /// <param name="isComplete">ture 完成占领，  false 取消占领</param>
        public void EndOccupyUnit(int towerUid, bool isComplete)
        {
            room.clientNetC.EndOccupyUnit(towerUid, unitUid, isComplete);
        }
        #endregion



        #region 机关操作--修理操作
        public RebuildUnit syncRebuildUnit = new RebuildUnit();
        /** 开始修理操作 */
        public void BeginRebuildUnit(int towerUid)
        {
            syncRebuildUnit.towerUid = towerUid;
            syncRebuildUnit.operateHeroUid = unitUid;
            syncRebuildUnit.state = AsyncOperateState.Begin;
            syncRebuildUnit.isUpdated = false;
            syncRebuildUnit.isSend = false;
        }

        /** 修理中操作 */
        public void DoingRebuildUnit(int towerUid)
        {
            syncRebuildUnit.towerUid = towerUid;
            syncRebuildUnit.operateHeroUid = unitUid;
            syncRebuildUnit.state = AsyncOperateState.Doing;
            syncRebuildUnit.isUpdated = false;
            syncRebuildUnit.isSend = false;
        }


        /// <summary>
        /// 结束修理操作
        /// </summary>
        /// <param name="towerUid"></param>
        /// <param name="isComplete">ture 完成，  false 取消</param>
        public void EndRebuildUnit(bool isComplete)
        {
            syncRebuildUnit.operateHeroUid = unitUid;
            syncRebuildUnit.state = isComplete ? AsyncOperateState.Complete : AsyncOperateState.Cancel;
            syncRebuildUnit.isUpdated = false;
            syncRebuildUnit.isSend = false;
        }
        #endregion



        #region 机关操作--雇佣操作
        /** 开始雇佣操作 */
        public void BeginEmployUnit(int towerUid)
        {
            room.clientNetC.BeginEmployUnit(towerUid, unitUid);
        }


        /// <summary>
        /// 结束雇佣操作
        /// </summary>
        /// <param name="towerUid"></param>
        /// <param name="isComplete">ture 完成，  false 取消</param>
        public void EndEmployUnit(int towerUid)
        {
            room.clientNetC.EndEmployUnit(towerUid);
        }
        #endregion

    }
}
