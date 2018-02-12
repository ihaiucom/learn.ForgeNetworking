using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      2/12/2018 2:44:49 PM
*  @Description:    
* ==============================================================================
*/

// Generated from: battle_server.proto
namespace Games.PB
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_Ping_Req")]
    public partial class OUTER_BM2B_Ping_Req : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_Ping_Req() { }

        private ulong _receivedTimestep;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"receivedTimestep", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public ulong receivedTimestep
        {
            get { return _receivedTimestep; }
            set { _receivedTimestep = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_Pong_Resp")]
    public partial class OUTER_BM2B_Pong_Resp : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_Pong_Resp() { }

        private ulong _receivedTimestep;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"receivedTimestep", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public ulong receivedTimestep
        {
            get { return _receivedTimestep; }
            set { _receivedTimestep = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_Error_Ntf")]
    public partial class OUTER_BM2B_Error_Ntf : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_Error_Ntf() { }

        private uint _errorId;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"errorId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public uint errorId
        {
            get { return _errorId; }
            set { _errorId = value; }
        }

        private string _erroContent;
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"erroContent", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string erroContent
        {
            get { return _erroContent ?? ""; }
            set { _erroContent = value; }
        }
        [global::System.Xml.Serialization.XmlIgnore]
        [global::System.ComponentModel.Browsable(false)]
        public bool erroContentSpecified
        {
            get { return _erroContent != null; }
            set { if (value == (_erroContent == null)) _erroContent = value ? erroContent : (string)null; }
        }
        private bool ShouldSerializeerroContent() { return erroContentSpecified; }
        private void ReseterroContent() { erroContentSpecified = false; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_ConnectionClose_Req")]
    public partial class OUTER_BM2B_ConnectionClose_Req : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_ConnectionClose_Req() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_ConnectionClose_Ntf")]
    public partial class OUTER_BM2B_ConnectionClose_Ntf : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_ConnectionClose_Ntf() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_Accepted_Ntf")]
    public partial class OUTER_BM2B_Accepted_Ntf : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_Accepted_Ntf() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_GM_Req")]
    public partial class OUTER_BM2B_GM_Req : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_GM_Req() { }

        private string _cmd;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"cmd", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_GM_Resp")]
    public partial class OUTER_BM2B_GM_Resp : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_GM_Resp() { }


        private string _content;
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"content", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string content
        {
            get { return _content ?? ""; }
            set { _content = value; }
        }
        [global::System.Xml.Serialization.XmlIgnore]
        [global::System.ComponentModel.Browsable(false)]
        public bool contentSpecified
        {
            get { return _content != null; }
            set { if (value == (_content == null)) _content = value ? content : (string)null; }
        }
        private bool ShouldSerializecontent() { return contentSpecified; }
        private void Resetcontent() { contentSpecified = false; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_CreateRoom_Req")]
    public partial class OUTER_BM2B_CreateRoom_Req : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_CreateRoom_Req() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_CreateRoom_Resp")]
    public partial class OUTER_BM2B_CreateRoom_Resp : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_CreateRoom_Resp() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_RemoveRoom_Req")]
    public partial class OUTER_BM2B_RemoveRoom_Req : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_RemoveRoom_Req() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_RemoveRoom_Resp")]
    public partial class OUTER_BM2B_RemoveRoom_Resp : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_RemoveRoom_Resp() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"OUTER_BM2B_RoomOver_Ntf")]
    public partial class OUTER_BM2B_RoomOver_Ntf : global::ProtoBuf.IExtensible
    {
        public OUTER_BM2B_RoomOver_Ntf() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

}