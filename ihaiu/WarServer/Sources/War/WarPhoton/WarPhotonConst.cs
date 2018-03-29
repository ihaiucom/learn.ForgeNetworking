using Games.PB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 房间属性Key
public class TSRoomPropertiesKey
{
    public static string State = "State";
}

// 房间状态
public enum TSRoomState
{
    None,
    Gameing,
}

// 玩家属性Key
public class TSPlayerPropertiesKey
{
    public static string RoleId = "RoleId";
    public static string LegionId = "LegionId";
}


// 消息类型
public class WarPhotonEventCode
{
    // 协议
    public const byte Proto = 1;

    // 加载进度
    public const byte Loader = 2;
}


// 帧同步操作
public class WarTrueSyncInputKey
{
    // 技能
    public const byte PLAYER_JOY_ISCHANGE = 0;
    public const byte PLAYER_JOY = 1;

    // 移动
    public const byte PLAYER_JOYMOVE_ISCHANGE = 2;
    public const byte PLAYER_JOYMOVE = 3;

    // 产兵波次跳过
    public const byte SPAWN_WAVE_SKIP = 4;
    // 投降
    public const byte SURRENDER = 5;

    // 创建机关
    public const byte TOWER_CREATE = 6;
    public const byte TOWER_CREATE_MSG = 7;

    // 回收单位
    public const byte RECOVERY_UNIT = 8;
    public const byte RECOVERY_UNIT_MSG = 9;


}




// 投降操作状态
// 产兵波次跳过操作状态
public enum TSBallotState : byte
{
    None,
    Yes,
    No,
}



public enum CreatePrefabType
{ 
    Create,  // 正常
    Clone,   // 镜像
    Summoner,// 召唤类型
}

public class CreatePrefabPropertiesKey
{
    public const int TypeKey = 0;



    public const int OFFSET = 1;
}
public class CreateUnitPropertiesKey
{
    public static int UnitType = CreatePrefabPropertiesKey.OFFSET + 0;
    public static int LegionId  = CreatePrefabPropertiesKey.OFFSET + 1;
    public static int Rotation = CreatePrefabPropertiesKey.OFFSET + 2;

    public static int BuildType = CreatePrefabPropertiesKey.OFFSET + 3;
    public static int MainbaseIndex = CreatePrefabPropertiesKey.OFFSET + 4;
    public static int TowerAvatarId     = CreatePrefabPropertiesKey.OFFSET + 5;

    public static int SoliderRouteId     = CreatePrefabPropertiesKey.OFFSET + 3;
    public static int SoliderUnitId      = CreatePrefabPropertiesKey.OFFSET + 4;
    public static int SoliderUnitLevel   = CreatePrefabPropertiesKey.OFFSET + 5;

    public static int TowerIsInit        = CreatePrefabPropertiesKey.OFFSET + 4;
    public static int TowerCellUid       = CreatePrefabPropertiesKey.OFFSET + 5;
    public static int TowerUnitId        = CreatePrefabPropertiesKey.OFFSET + 6;
    public static int TowerUnitLevel     = CreatePrefabPropertiesKey.OFFSET + 7;
}


public class CloneUnitPropertiesKey
{
    // 主体UID
    public static int MainUid = CreatePrefabPropertiesKey.OFFSET + 0;
    public static int AvatarId = CreatePrefabPropertiesKey.OFFSET + 1;
    public static int Rotation = CreatePrefabPropertiesKey.OFFSET + 2;
    public static int UnitId = CreatePrefabPropertiesKey.OFFSET + 3;
    public static int LifeTime = CreatePrefabPropertiesKey.OFFSET + 4;
    public static int WeaponId = CreatePrefabPropertiesKey.OFFSET + 5;
}

// 移动遥感
public class MoveJoy
{
    // 是否发送
    public bool isSend;
    // 运行完成
    public bool isUpdated;

    public WarUIAttackType type;
    public Vector3 pos;
    public bool run;
    public float realPos;
}

// 技能遥感
public class SkillStateJoy
{
    // 是否发送
    public bool isSend;
    // 运行完成
    public bool isUpdated;

    // state
    public WarUIAttackType stateType;
    public ButtonState openClose;

    // 参数，子弹数量，目前传递buff层数
    public int bulletCurrentCount;
}

//投票
public class Ballot
{
    // 是否发送
    public bool isSend;
    // 运行完成
    public bool isUpdated;

    // 投票选择
    public TSBallotState ballot;
}

public enum AsyncOperateState
{
    // 开始
    Begin,
    // 持续中
    Doing,
    // 完成
    Complete,
    // 取消
    Cancel,
}

// 回收单位
public class RecoveryUnit
{
    // 是否发送
    public bool isSend;
    // 运行完成
    public bool isUpdated;

    public AsyncOperateState state;
    public int towerUid;
    public int operateHeroUid;
}

// 维修单位
public class RebuildUnit : RecoveryUnit
{

}