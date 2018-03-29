using Games.PB;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      10/9/2017 5:08:39 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.Wars
{
    /// <summary>
    /// 战斗协议辅助类
    /// </summary>
    public static class WarProtoUtil
    {

        public static byte[] SerializerSyncMsg<T>(T action)
        {
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, action);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Dispose();

            return bytes;
        }


        public static T DeserializeSyncMsg<T>(byte[] bytes)
        {
            MemoryStream memStream = new MemoryStream(bytes);
            memStream.Position = 0;
            T info = ProtoBuf.Serializer.Deserialize<T>(memStream);
            memStream.Dispose();
            return info;
        }

        public static BattleSyncData CreateBattleSyncMsg<T>(WarSyncActionType actionType, T action)
        {
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, action);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Dispose();

            BattleSyncData msg = new BattleSyncData();
            msg.type = (uint)actionType;
            msg.data = bytes;
            return msg;
        }

        public static BattleSyncData Serializer<T>(WarSyncActionType actionType, T action)
        {
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(stream, action);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Dispose();

            BattleSyncData msg = new BattleSyncData();
            msg.type = (uint)actionType;
            msg.data = bytes;
            return msg;
        }


        public static T Deserialize<T>(BattleSyncData msg)
        {
            MemoryStream memStream = new MemoryStream();
            memStream.Position = 0;
            memStream.Write(msg.data, 0, msg.data.Length);

            memStream.Position = 0;
            T info = ProtoBuf.Serializer.Deserialize<T>(memStream);
            memStream.Dispose();
            return info;
        }

        public static void SetVal(this WarProtoVector3 val, Vector3 src)
        {
            val.x = Mathf.FloorToInt(src.x * 1000);
            val.y = Mathf.FloorToInt(src.y * 1000);
            val.z = Mathf.FloorToInt(src.z * 1000);
        }


        public static WarProtoVector3 Vector3ToProto(this Vector3 src)
        {
            WarProtoVector3 val = new WarProtoVector3();
            val.x = Mathf.FloorToInt(src.x * 1000);
            val.y = Mathf.FloorToInt(src.y * 1000);
            val.z = Mathf.FloorToInt(src.z * 1000);
            return val;
        }


        public static Vector3 ProtoToVector3(this WarProtoVector3 src)
        {
            Vector3 val = new Vector3();
            val.x = src.x / 1000f;
            val.y = src.y / 1000f;
            val.z = src.z / 1000f;
            return val;
        }

        /** 生成技能列表 */
        public static List<WarSyncCreateSkill> GenerateSyncSkillList(WarRoom room, WarEnterUnitData unit)
        {
            return GenerateSyncSkillList(room, unit.skillList);
        }

        public static List<WarSyncCreateSkill> GenerateSyncSkillList(WarRoom room, List<WarEnterSkillData> skillList, int level = -1)
        {
            List<WarSyncCreateSkill> list = new List<WarSyncCreateSkill>();
            foreach (WarEnterSkillData skillData in skillList)
            {
                WarSyncCreateSkill skill = new WarSyncCreateSkill();
                skill.skillUid = room.SKILL_UID;
                skill.skillId = skillData.skillId;
                skill.skillLevel = level > 0 ? level : skillData.skillLevel;
                list.Add(skill);
            }
            return list;
        }

        public static List<WarSyncCreateSkill> GenerateSyncSkillList(WarRoom room, int unitId, int unitLevel)
        {
            UnitLevelConfig unitLevelConfig = Game.config.unitLevel.GetConfig(unitId, unitLevel);
            return GenerateSyncSkillList(room, unitLevelConfig.skillList, unitLevel);
        }

        /** 单位 */
        public static WarSyncCreateUnit GenerateUnit(int uid, int unitId, int avatarId, int unitLevel, int legionId, Vector3 position, Vector3 rotation, List<WarSyncCreateSkill> skills)
        {
            WarSyncCreateUnit msg = new WarSyncCreateUnit();
            msg.uid = uid;
            msg.unitId = unitId;
            msg.unitLevel = unitLevel;
            msg.legionId = legionId;
            msg.position = position.Vector3ToProto();
            msg.rotation = rotation.Vector3ToProto();
            msg.avatarId = avatarId;
            if(skills != null)
            {
                msg.skills.AddRange(skills);
            }
            return msg;
        }


        /** 创建机关单位 */
        public static WarSyncCreateTowerUnit CreateTowerUnit(int cellUid, int uid, int unitId, int avatarId, int unitLevel, int legionId, Vector3 position, Vector3 rotation, List<WarSyncCreateSkill> skills)
        {
            WarSyncCreateTowerUnit msg = new WarSyncCreateTowerUnit();
            msg.cellUid = cellUid;
            msg.unit = WarProtoUtil.GenerateUnit(uid, unitId, avatarId, unitLevel, legionId, position, rotation, skills);
            return msg;
        }

        /** 是否需要主机中转 */
        public static bool IsNeedPasMater(uint actionType)
        {
            return IsNeedPasMater((WarSyncActionType) actionType);
        }

        public static bool IsNeedPasMater(WarSyncActionType actionType)
        {
            switch(actionType)
            {
                case WarSyncActionType.RoomState:
                //case WarSyncActionType.CreateTowerUnit:

                case WarSyncActionType.TowerAttack:
                case WarSyncActionType.UnitHero:
                    return true;
            }
            return false;
        }

        ///** 是否只发送给主机 */
        //public static bool IsOnlySendMaster(uint actionType)
        //{
        //    return IsOnlySendMaster((WarSyncActionType)actionType);
        //}


        //public static bool IsOnlySendMaster(WarSyncActionType actionType)
        //{
        //    switch(actionType)
        //    {
        //        case WarSyncActionType.SoliderWaveSkip:
        //            return true;
        //    }
        //    return false;
        //}


    }
}
