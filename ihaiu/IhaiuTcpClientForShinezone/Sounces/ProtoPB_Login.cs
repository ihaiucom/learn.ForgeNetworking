using System;
using System.Collections.Generic;
/** 
* ==============================================================================
*  @Author      	曾峰(zengfeng75@qq.com) 
*  @Web      		http://blog.ihaiu.com
*  @CreateTime      3/16/2018 4:03:17 PM
*  @Description:    
* ==============================================================================
*/
namespace Games.PB
{

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_Ping_1")]
    public partial class S_Ping_1 : global::ProtoBuf.IExtensible
    {
        public S_Ping_1() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }


    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"C_Ping_1")]
    public partial class C_Ping_1 : global::ProtoBuf.IExtensible
    {
        public C_Ping_1() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_FirstAuthorization_10002")]
    public partial class S_FirstAuthorization_10002 : global::ProtoBuf.IExtensible
    {
        public S_FirstAuthorization_10002() { }

        private uint _error_code;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"error_code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public uint error_code
        {
            get { return _error_code; }
            set { _error_code = value; }
        }

        private string _authorization_str;
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"authorization_str", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string authorization_str
        {
            get { return _authorization_str ?? ""; }
            set { _authorization_str = value; }
        }
        [global::System.Xml.Serialization.XmlIgnore]
        [global::System.ComponentModel.Browsable(false)]
        public bool authorization_strSpecified
        {
            get { return _authorization_str != null; }
            set { if (value == (_authorization_str == null)) _authorization_str = value ? authorization_str : (string)null; }
        }
        private bool ShouldSerializeauthorization_str() { return authorization_strSpecified; }
        private void Resetauthorization_str() { authorization_strSpecified = false; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }


    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_ConnectToNewGateway_10003")]
    public partial class S_ConnectToNewGateway_10003 : global::ProtoBuf.IExtensible
    {
        public S_ConnectToNewGateway_10003() { }

        private string _ip;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"ip", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private uint _port;
        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = @"port", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public uint port
        {
            get { return _port; }
            set { _port = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }


    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_AccountDuplicateLogin_10004")]
    public partial class S_AccountDuplicateLogin_10004 : global::ProtoBuf.IExtensible
    {
        public S_AccountDuplicateLogin_10004() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }


    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_SyncServerTimestamp_15001")]
    public partial class S_SyncServerTimestamp_15001 : global::ProtoBuf.IExtensible
    {
        public S_SyncServerTimestamp_15001() { }

        private ulong _timestamp;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public ulong timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }


    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"S_SyncRandomFactor_15002")]
    public partial class S_SyncRandomFactor_15002 : global::ProtoBuf.IExtensible
    {
        public S_SyncRandomFactor_15002() { }

        private string _random_factor;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"random_factor", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string random_factor
        {
            get { return _random_factor; }
            set { _random_factor = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }














    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"C_FirstAuthorization_10000")]
    public partial class C_FirstAuthorization_10000 : global::ProtoBuf.IExtensible
    {
        public C_FirstAuthorization_10000() { }

        private ulong _account_id;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"account_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public ulong account_id
        {
            get { return _account_id; }
            set { _account_id = value; }
        }
        private string _account_name;
        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = @"account_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string account_name
        {
            get { return _account_name; }
            set { _account_name = value; }
        }
        private string _token;
        [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name = @"token", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string token
        {
            get { return _token; }
            set { _token = value; }
        }
        private string _version;
        [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name = @"version", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
        private string _channel;
        [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name = @"channel", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string channel
        {
            get { return _channel; }
            set { _channel = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
    

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"C_ReconnectOnLossandAuthorization_10001")]
    public partial class C_ReconnectOnLossandAuthorization_10001 : global::ProtoBuf.IExtensible
    {
        public C_ReconnectOnLossandAuthorization_10001() { }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }



}
