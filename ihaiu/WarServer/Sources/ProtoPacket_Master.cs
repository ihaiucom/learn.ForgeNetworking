//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: battle_inner.proto
namespace Games.PB
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_B2BM_Ping")]
  public partial class OUTER_B2BM_Ping : global::ProtoBuf.IExtensible
  {
    public OUTER_B2BM_Ping() {}
    
    private ulong _timestamp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong timestamp
    {
      get { return _timestamp; }
      set { _timestamp = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_B2BM_RegNewBattleServer_Req")]
  public partial class OUTER_B2BM_RegNewBattleServer_Req : global::ProtoBuf.IExtensible
  {
    public OUTER_B2BM_RegNewBattleServer_Req() {}
    
    private string _ip;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string ip
    {
      get { return _ip; }
      set { _ip = value; }
    }
    private uint _port;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"port", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint port
    {
      get { return _port; }
      set { _port = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_B2BM_RegNewBattleServer_Resp")]
  public partial class OUTER_B2BM_RegNewBattleServer_Resp : global::ProtoBuf.IExtensible
  {
    public OUTER_B2BM_RegNewBattleServer_Resp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BI_SkillInfo")]
  public partial class BI_SkillInfo : global::ProtoBuf.IExtensible
  {
    public BI_SkillInfo() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BI_TrapInfo")]
  public partial class BI_TrapInfo : global::ProtoBuf.IExtensible
  {
    public BI_TrapInfo() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BI_WeaponInfo")]
  public partial class BI_WeaponInfo : global::ProtoBuf.IExtensible
  {
    public BI_WeaponInfo() {}
    
    private uint _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint id
    {
      get { return _id; }
      set { _id = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BattlePlayerInfo")]
  public partial class BattlePlayerInfo : global::ProtoBuf.IExtensible
  {
    public BattlePlayerInfo() {}
    
    private ulong _char_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"char_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong char_id
    {
      get { return _char_id; }
      set { _char_id = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private uint _level;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint level
    {
      get { return _level; }
      set { _level = value; }
    }
    private uint _hero_id;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"hero_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint hero_id
    {
      get { return _hero_id; }
      set { _hero_id = value; }
    }
    private uint _hero_level;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"hero_level", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint hero_level
    {
      get { return _hero_level; }
      set { _hero_level = value; }
    }
    private readonly global::System.Collections.Generic.List<BI_SkillInfo> _skill = new global::System.Collections.Generic.List<BI_SkillInfo>();
    [global::ProtoBuf.ProtoMember(6, Name=@"skill", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<BI_SkillInfo> skill
    {
      get { return _skill; }
    }
  
    private readonly global::System.Collections.Generic.List<BI_TrapInfo> _trap = new global::System.Collections.Generic.List<BI_TrapInfo>();
    [global::ProtoBuf.ProtoMember(7, Name=@"trap", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<BI_TrapInfo> trap
    {
      get { return _trap; }
    }
  
    private readonly global::System.Collections.Generic.List<BI_WeaponInfo> _weapon = new global::System.Collections.Generic.List<BI_WeaponInfo>();
    [global::ProtoBuf.ProtoMember(8, Name=@"weapon", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<BI_WeaponInfo> weapon
    {
      get { return _weapon; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_BM2B_MPVE_CreateRoom_Req")]
  public partial class OUTER_BM2B_MPVE_CreateRoom_Req : global::ProtoBuf.IExtensible
  {
    public OUTER_BM2B_MPVE_CreateRoom_Req() {}
    
    private readonly global::System.Collections.Generic.List<BattlePlayerInfo> _info = new global::System.Collections.Generic.List<BattlePlayerInfo>();
    [global::ProtoBuf.ProtoMember(1, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<BattlePlayerInfo> info
    {
      get { return _info; }
    }
  
    private uint _copy_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"copy_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint copy_id
    {
      get { return _copy_id; }
      set { _copy_id = value; }
    }
    private uint _room_id;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"room_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint room_id
    {
      get { return _room_id; }
      set { _room_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_BM2B_MPVE_CreateRoom_Resp")]
  public partial class OUTER_BM2B_MPVE_CreateRoom_Resp : global::ProtoBuf.IExtensible
  {
    public OUTER_BM2B_MPVE_CreateRoom_Resp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private uint _room_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"room_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint room_id
    {
      get { return _room_id; }
      set { _room_id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BI_BattleOverData")]
  public partial class BI_BattleOverData : global::ProtoBuf.IExtensible
  {
    public BI_BattleOverData() {}
    

    private byte[] _data;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] data
    {
      get { return _data?? null; }
      set { _data = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool dataSpecified
    {
      get { return _data != null; }
      set { if (value == (_data== null)) _data = value ? data : (byte[])null; }
    }
    private bool ShouldSerializedata() { return dataSpecified; }
    private void Resetdata() { dataSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_B2BM_RoomEnd_Req")]
  public partial class OUTER_B2BM_RoomEnd_Req : global::ProtoBuf.IExtensible
  {
    public OUTER_B2BM_RoomEnd_Req() {}
    
    private uint _room_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"room_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint room_id
    {
      get { return _room_id; }
      set { _room_id = value; }
    }
    private BI_BattleOverData _data;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public BI_BattleOverData data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OUTER_B2BM_RoomEnd_Resp")]
  public partial class OUTER_B2BM_RoomEnd_Resp : global::ProtoBuf.IExtensible
  {
    public OUTER_B2BM_RoomEnd_Resp() {}
    
    private uint _result;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint result
    {
      get { return _result; }
      set { _result = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}