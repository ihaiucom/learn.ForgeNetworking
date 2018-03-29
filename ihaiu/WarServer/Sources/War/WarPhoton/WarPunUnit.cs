using Assets.Scripts.Common;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Wars
{

    public class WarPunUnit : MonoBehaviour, IPunObservable
    {
        public PhotonView photonView;
        public WarRoom room;
        public UnitData unitData;

        public WarUnityRes res
        {
            get
            {
                return room.clientRes;
            }
        }

        public int uid
        {
            get
            {
                return photonView.viewID;
            }
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                bool inScene = unitData != null;
                float hp = 0;
                if (inScene)
                    hp = unitData.prop.Hp;

                stream.SendNext(inScene);
                stream.SendNext(hp);
            }
            else
            {
                bool inScene = (bool)stream.ReceiveNext();
                float hp = (float)stream.ReceiveNext();
                if (inScene)
                {
                    unitData.prop.Hp = hp;
                }
            }
        }

        public void Death(int attackUid)
        {
            photonView.RPC("RPC_Death", PhotonTargets.All, attackUid);
        }

        [PunRPC]
        public void RPC_Death(int attackUid)
        {
            unitData.RPC_Death(attackUid);
        }

        void Awake()
        {
            if (photonView == null)
                photonView = GetComponent<PhotonView>();


            room = War.currentRoom;


            UnitProduceType createPrefabType   = (UnitProduceType) photonView.instantiationData[CreatePrefabPropertiesKey.TypeKey];

            switch (createPrefabType)
            {
                case UnitProduceType.Normal:
                    CreatePrefab(createPrefabType);
                    break;
                case UnitProduceType.Clone:
                case UnitProduceType.Summoner:
                    ClonePrefab(createPrefabType);
                    break;
            }

        }

        public void CreatePrefab(UnitProduceType produceType = UnitProduceType.Normal, int avatarId = -1, int cloneMainUid = -1, int unitId = -1, float lifeTime = -1)
        {


            UnitType unitType   = (UnitType) photonView.instantiationData[CreateUnitPropertiesKey.UnitType];
            int legionId        = (int) photonView.instantiationData[CreateUnitPropertiesKey.LegionId];
            Vector3 rotation = (Vector3)photonView.instantiationData[CreateUnitPropertiesKey.Rotation];
            UnitLevelConfig unitLevelConfig;



            Vector3 position = transform.position;
            WarEnterLegionData enterLegionData = room.enterData.GetLegion(legionId);
            WarEnterUnitData enterUnitData = null;
            int routeId = 0;
            int cellId = 0;
            bool towerIsInit = true;
            switch (unitType)
            {
                case UnitType.Hero:
                    enterUnitData = enterLegionData.hero;
                    break;
                case UnitType.Build:
                    UnitBuildType buildType = (UnitBuildType)photonView.instantiationData[CreateUnitPropertiesKey.BuildType];
                    switch (buildType)
                    {
                        case UnitBuildType.Mainbase:
                            int index = (int)photonView.instantiationData[CreateUnitPropertiesKey.MainbaseIndex];
                            int towerAvatarId = (int)photonView.instantiationData[CreateUnitPropertiesKey.TowerAvatarId];
                            enterUnitData = room.enterData.mainbaseList[index].unit;
                            enterUnitData.avatarId = towerAvatarId;
                            break;
                        case UnitBuildType.Tower:
                            towerIsInit = (bool)photonView.instantiationData[CreateUnitPropertiesKey.TowerIsInit];
                            cellId = (int)photonView.instantiationData[CreateUnitPropertiesKey.TowerCellUid];
                            int towerUnitId = (int)photonView.instantiationData[CreateUnitPropertiesKey.TowerUnitId];
                            int towerUnitLevel = (int)photonView.instantiationData[CreateUnitPropertiesKey.TowerUnitLevel];
                            if (towerIsInit)
                            {
                                enterUnitData = new WarEnterUnitData();
                                enterUnitData.unitId = towerUnitId;
                                enterUnitData.unitLevel = towerUnitLevel;

                                unitLevelConfig = Game.config.unitLevel.GetConfig(enterUnitData.unitId, enterUnitData.unitLevel);
                                enterUnitData.skillList = unitLevelConfig.skillList;
                            }
                            else
                            {
                                enterUnitData = room.enterData.GetLegionUnit(legionId, towerUnitId);
                            }
                            break;
                    }
                    break;
                case UnitType.Solider:
                    routeId = (int)photonView.instantiationData[CreateUnitPropertiesKey.SoliderRouteId];
                    int soliderUnitId = (int)photonView.instantiationData[CreateUnitPropertiesKey.SoliderUnitId];
                    int soliderUnitLevel = (int)photonView.instantiationData[CreateUnitPropertiesKey.SoliderUnitLevel];
                    enterUnitData = new WarEnterUnitData();
                    enterUnitData.unitId = soliderUnitId;
                    enterUnitData.unitLevel = soliderUnitLevel;
                    unitLevelConfig = Game.config.unitLevel.GetConfig(soliderUnitId, soliderUnitLevel);
                    enterUnitData.skillList = unitLevelConfig.skillList;
                    break;
            }

            if (unitId != -1)
            {
                enterUnitData.unitId = unitId;
            }

            unitData = CreateUnit(uid, legionId, enterUnitData, position, rotation, avatarId);
            unitData.unitProduceType = produceType;
            unitData.lifeDelateTime = room.LTime.time;
            if (lifeTime > 0)
            {
                unitData.reduceHpPerTime = unitData.prop.HpMax / lifeTime;
            }

            if (cloneMainUid != -1)
            {
                unitData.cloneUnitMainUid = cloneMainUid;
            }
            switch (unitType)
            {
                case UnitType.Solider:
                    unitData.routeId = routeId;
                    break;
                case UnitType.Build:
                    unitData.buildCellUid = cellId;
                    room.sceneData.SetBuildCellUnit(cellId, unitData);
                    if (!towerIsInit)
                        Game.audio.PlaySoundWarSFX(unitData.avatarConfig.audioBirth, position);
                    break;
            }

            SettingUnitData(unitData);
            UnitAgent unitAgent = AddUnitAgent(unitData);

            // 出生被动技能
            unitData.UsePassiveSkillBirth();
        }


        public void ClonePrefab(UnitProduceType produceType)
        {
            int mainUid   = (int) photonView.instantiationData[CloneUnitPropertiesKey.MainUid];
            int avatarId        = (int) photonView.instantiationData[CloneUnitPropertiesKey.AvatarId];
            Vector3 rotation = (Vector3)photonView.instantiationData[CloneUnitPropertiesKey.Rotation];
            int unitId        = (int) photonView.instantiationData[CloneUnitPropertiesKey.UnitId];
            float lifeTime        = ((int) photonView.instantiationData[CloneUnitPropertiesKey.LifeTime]) / 100.0F;
            bool weaponId  = (int)photonView.instantiationData[CloneUnitPropertiesKey.WeaponId] == 1;

            UnitAgent mainUnitAgent = room.clientSceneView.GetUnit(mainUid);
            if (mainUnitAgent == null || mainUnitAgent.photonView == null)
            {
                return;
            }

            object[] srcData =  mainUnitAgent.photonView.instantiationData;

            object[] data = new object[srcData.Length];
            for (int i = 0; i < srcData.Length; i++)
            {
                data[i] = srcData[i];
            }

            data[CreateUnitPropertiesKey.Rotation] = rotation;
            photonView.instantiationData = data;
            CreatePrefab(produceType, avatarId, mainUnitAgent.unitData.uid, unitId, lifeTime);

            mainUnitAgent.unitData.cloneChilds.Add(uid);
        }



        /** 创建单位 */
        public UnitData CreateUnit(int uid, int legionId, WarEnterUnitData enterUnitData, Vector3 position, Vector3 rotation, int avatarId = -1)
        {
            UnitData unit = ClassObjPool<UnitData>.Get();
            unit.uid = uid;
            unit.unitId = enterUnitData.unitId;
            unit.unitLevel = enterUnitData.unitLevel;
            unit.legionId = legionId;
            unit.position = position;
            unit.rotation = rotation;


            UnitConfig unitConfig = Game.config.unit.GetConfig(unit.unitId);
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(unit.unitId, unit.unitLevel);
            unit.prop.AppProps(unitLevelConfig.protoAttachData, true);

            if (avatarId == -1)
            {
                if (enterUnitData.avatarId > 0)
                {
                    avatarId = enterUnitData.avatarId;
                }
                else
                {
                    avatarId = unitConfig.avatarId;
                }
            }

            unit.unitConfig = unitConfig;
            unit.unitLevelConfig = unitLevelConfig;
            unit.avatarConfig = Game.config.avatar.GetConfig(avatarId);
            unit.unitRadius = unitConfig.radius;
            unit.unitFlyHeight = unitConfig.flyHeight;
            unit.unitType = unitConfig.unitType;
            unit.buildType = unitConfig.buildType;
            unit.soliderType = unitConfig.soliderType;
            unit.professionType = unitConfig.professionType;
            unit.spaceType = unitConfig.spaceType;

            unit.name = unitConfig.name + " Lv" + unitLevelConfig.level;
            if (unit.unitType == UnitType.Hero)
            {
                unit.name = room.sceneData.GetLegion(unit.legionId).roleName;
            }

            bool hasSkill300205 = false;
            bool hasSkill300206 = false;
            for (int i = 0; i < enterUnitData.skillList.Count; i++)
            {

                WarEnterSkillData skillMsg = enterUnitData.skillList[i];
                SkillController skillcontroller = new SkillController(room, unit);
                skillcontroller.skillUid = room.SKILL_UID;
                skillcontroller.skillId = skillMsg.skillId;
                skillcontroller.skillLevel = skillMsg.skillLevel;

                if (unit.unitType == UnitType.Solider)
                {
                    skillcontroller.skillLevel = 1;
                }

                if (skillcontroller.skillId > 0)
                {
                    skillcontroller.skillConfig = Game.config.skill.GetConfig(skillcontroller.skillId);
                    skillcontroller.skillLevelConfig = Game.config.skillLevel.GetConfigs(skillcontroller.skillId, skillcontroller.skillLevel);
                    // 被动附加属性
                    if (skillcontroller.skillLevelConfig.attributePack > 0)
                    {
                        AttributePackConfig attributePackConfig = Game.config.attributePack.GetConfig(skillcontroller.skillLevelConfig.attributePack);
                        if (attributePackConfig != null)
                        {
                            List<Prop> proplist = new List<Prop>()
                            {
                                Prop.Create(PropId.Hp,attributePackConfig.hp),
                                Prop.Create(PropId.HpMax,attributePackConfig.hpMax),
                                Prop.Create(PropId.Damage,attributePackConfig.damage),
                                Prop.Create(PropId.PhysicalDefence,attributePackConfig.physicalDefence),
                                Prop.Create(PropId.MagicDefence,attributePackConfig.magicDefence),
                                Prop.Create(PropId.PhysicalAttack,attributePackConfig.physicalAttack),
                                Prop.Create(PropId.MagicAttack,attributePackConfig.magicAttack),
                                Prop.Create(PropId.HpRecover,attributePackConfig.hpRecover),
                                Prop.Create(PropId.EnergyRecover,attributePackConfig.energyRecover),
                                Prop.Create(PropId.RadarRadius,attributePackConfig.radarRadius),
                                Prop.Create(PropId.AttackRadius,attributePackConfig.attackRadius),
                                Prop.Create(PropId.AttackSpeed,attributePackConfig.attackSpeed),
                                Prop.Create(PropId.MovementSpeed,attributePackConfig.movementSpeed)
                            };
                            PropAttachData propAttachData = new PropAttachData(proplist);
                            unit.prop.AppProps(propAttachData, true);
                        }
                    }
                }
                unit.AddSkill(skillcontroller);

                if (skillMsg.skillId == 300205)
                {
                    hasSkill300205 = true;
                }

                if (skillMsg.skillId == 300206)
                {
                    hasSkill300206 = true;
                }
            }


            if (hasSkill300206)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300202);
            }

            if (hasSkill300205)
            {
                unit.avatarConfig = Game.config.avatar.GetConfig(300201);
            }

            if (unit.unitType == UnitType.Solider)
            {

                float totalWeight = 0;
                for (int i = 0; i < unitLevelConfig.aiSoliders.Count; i++)
                {
                    AISoliderSkill ai = new AISoliderSkill();
                    ai.unit = unit;
                    ai.aiSoliderConfig = Game.config.aISolider.GetConfig(unitLevelConfig.aiSoliders[i]);
                    ai.skillController = unit.GetSkill(ai.aiSoliderConfig.skillId);
                    ai.weightMinVal = totalWeight;
                    totalWeight += ai.aiSoliderConfig.weight;
                    ai.weightMaxVal = totalWeight;
                    unit.aiSoliderSkillList.Add(ai);
                    if (ai.skillController.skillId == unit.SkillA.skillId)
                    {
                        unit.attackAiSoliderSkill = ai;
                    }
                }

                foreach (AISoliderSkill ai in unit.aiSoliderSkillList)
                {
                    ai.weightMin = ai.weightMinVal / totalWeight * 100;
                    ai.weightMax = ai.weightMaxVal / totalWeight * 100;
                }
            }

            return unit;
        }

        /** 创建单位数据 */
        public UnitData SettingUnitData(UnitData unit)
        {
            unit.room = room;
            unit.unitData = unit;
            room.sceneData.AddUnit(unit);
            switch (unit.unitType)
            {
                case UnitType.Hero:
                    // 势力 主单位
                    if (unit.legionData.mainUnit == null)
                        unit.legionData.mainUnit = unit;

                    // TODO 临时 给初始能量
                    unit.prop.AddProp(PropId.Energy, PropType.Base, room.stageConfig.energy);
                    unit.prop.AddProp(PropId.EnergyMax, PropType.Base, room.stageConfig.energyMax);
                    break;
                case UnitType.Build:
                    switch (unit.buildType)
                    {
                        case UnitBuildType.Mainbase:
                            // 势力 主基地
                            //unit.legionData.mainbaseUnit = unit;
                            if (unit.legionData.group != null)
                            {
                                foreach (LegionData legion in unit.legionData.group.list)
                                {
                                    legion.mainbaseUnit = unit;
                                }
                            }
                            break;
                    }
                    break;
            }

            unit.prop.InitNonrevertFinals();
            unit.prop.Calculate();
            return unit;
        }


        /** 添加单位 */
        public UnitAgent AddUnitAgent(UnitData unitData)
        {
            if (unitData.avatarConfig == null)
            {
                Loger.LogErrorFormat("单位没有avatar配置 unitData={0}", unitData);
                return null;
            }

            GameObject modelGO = res.GetGameObjectInstall(unitData.avatarConfig.model);


            gameObject.name = gameObject.name.Replace("(Clone)", "(" + unitData.uid + ")");
            UnitAgent unitAgent = gameObject.GetComponent<UnitAgent>();
            if (unitAgent == null)
                unitAgent = gameObject.AddComponent<UnitAgent>();
            unitAgent.AddToAnchorRotation(modelGO);

            unitAgent.animatorManager = modelGO.GetComponent<AnimatorManager>();
            unitAgent.unitBloodBag = modelGO.GetComponent<UnitBloodBag>();
            if (unitAgent.animatorManager == null)
                unitAgent.animatorManager = modelGO.GetComponentInChildren<AnimatorManager>();

            if (unitAgent.animatorManager != null && unitAgent.animatorManager.weaponPos != null)
            {
                if (unitData.unitConfig.weaponDefaultId != 0)
                {
                    AvatarConfig weaponConfig =  Game.config.avatar.GetConfig(unitData.unitConfig.weaponDefaultId);
                    if (!string.IsNullOrEmpty(weaponConfig.model))
                    {
                        unitAgent.animatorManager.weaponObj = res.GetGameObjectInstall(weaponConfig.model);
                        unitAgent.animatorManager.weaponObj.transform.SetParent(unitAgent.animatorManager.weaponPos);
                        unitAgent.animatorManager.weaponObj.transform.localPosition = Vector3.zero;
                        unitAgent.animatorManager.weaponObj.transform.localEulerAngles = Vector3.zero;
                    }
                }
            }


            unitAgent.position = unitData.position;
            unitAgent.rotation = unitData.rotation;
            unitAgent.Init(unitData);
            room.clientSceneView.AddUnit(unitAgent);

            if (unitData.unitType == UnitType.Hero && unitData.isCloneUnit == false)
            {
                if (unitData.clientIsOwn)
                {
                    room.clientOperationUnit.SetUnitAgent(unitAgent);
                    //StudioListener listener = gameObject.GetComponent<StudioListener>();
                    //if (listener == null)
                    //{
                    //    listener = gameObject.AddComponent<StudioListener>();
                    //    listener.ListenerNumber = 1;
                    //    listener.enabled = false;
                    //    listener.enabled = true;

                    //}
                }
                // 天梯设置对方英雄
                else if (room.stageType == StageType.PVPLadder)
                {
                    room.clientOperationUnit.SetUnitAgentPVPLadder(unitAgent);
                }
            }
            return unitAgent;
        }

    }
}
