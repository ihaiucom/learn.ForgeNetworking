using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace Rooms.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector3\"][\"float\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"target\"][\"hp\"]]")]
	[GeneratedInterpol("{\"inter\":[0,0,0.15]")]
	public partial class HelloNetworkObject : NetworkObject
	{
		public const int IDENTITY = 1;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private int _id;
		public event FieldEvent<int> idChanged;
		public Interpolated<int> idInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int id
		{
			get { return _id; }
			set
			{
				// Don't do anything if the value is the same
				if (_id == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_id = value;
				hasDirtyFields = true;
			}
		}

		public void SetidDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_id(ulong timestep)
		{
			if (idChanged != null) idChanged(_id, timestep);
			if (fieldAltered != null) fieldAltered("id", _id, timestep);
		}
		private int _age;
		public event FieldEvent<int> ageChanged;
		public Interpolated<int> ageInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int age
		{
			get { return _age; }
			set
			{
				// Don't do anything if the value is the same
				if (_age == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_age = value;
				hasDirtyFields = true;
			}
		}

		public void SetageDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_age(ulong timestep)
		{
			if (ageChanged != null) ageChanged(_age, timestep);
			if (fieldAltered != null) fieldAltered("age", _age, timestep);
		}
		private float _height;
		public event FieldEvent<float> heightChanged;
		public InterpolateFloat heightInterpolation = new InterpolateFloat() { LerpT = 0.15f, Enabled = true };
		public float height
		{
			get { return _height; }
			set
			{
				// Don't do anything if the value is the same
				if (_height == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_height = value;
				hasDirtyFields = true;
			}
		}

		public void SetheightDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_height(ulong timestep)
		{
			if (heightChanged != null) heightChanged(_height, timestep);
			if (fieldAltered != null) fieldAltered("height", _height, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			idInterpolation.current = idInterpolation.target;
			ageInterpolation.current = ageInterpolation.target;
			heightInterpolation.current = heightInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _id);
			UnityObjectMapper.Instance.MapBytes(data, _age);
			UnityObjectMapper.Instance.MapBytes(data, _height);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_id = UnityObjectMapper.Instance.Map<int>(payload);
			idInterpolation.current = _id;
			idInterpolation.target = _id;
			RunChange_id(timestep);
			_age = UnityObjectMapper.Instance.Map<int>(payload);
			ageInterpolation.current = _age;
			ageInterpolation.target = _age;
			RunChange_age(timestep);
			_height = UnityObjectMapper.Instance.Map<float>(payload);
			heightInterpolation.current = _height;
			heightInterpolation.target = _height;
			RunChange_height(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _id);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _age);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _height);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (idInterpolation.Enabled)
				{
					idInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					idInterpolation.Timestep = timestep;
				}
				else
				{
					_id = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_id(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (ageInterpolation.Enabled)
				{
					ageInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					ageInterpolation.Timestep = timestep;
				}
				else
				{
					_age = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_age(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (heightInterpolation.Enabled)
				{
					heightInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					heightInterpolation.Timestep = timestep;
				}
				else
				{
					_height = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_height(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (idInterpolation.Enabled && !idInterpolation.current.UnityNear(idInterpolation.target, 0.0015f))
			{
				_id = (int)idInterpolation.Interpolate();
				//RunChange_id(idInterpolation.Timestep);
			}
			if (ageInterpolation.Enabled && !ageInterpolation.current.UnityNear(ageInterpolation.target, 0.0015f))
			{
				_age = (int)ageInterpolation.Interpolate();
				//RunChange_age(ageInterpolation.Timestep);
			}
			if (heightInterpolation.Enabled && !heightInterpolation.current.UnityNear(heightInterpolation.target, 0.0015f))
			{
				_height = (float)heightInterpolation.Interpolate();
				//RunChange_height(heightInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];


            RegisterBehaviors();
            RegisterRpcs();

            RegistrationComplete();
        }

		public HelloNetworkObject() : base() { Initialize(); }
		public HelloNetworkObject(RoomScene networker,  int createCode = 0, byte[] metadata = null) : base(networker,  createCode, metadata) { Initialize(); }
		public HelloNetworkObject(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	
	
        //=================================================================
        // RPC Begin
        //-----------------------------------------------------------------
	
	
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_SET_H_P = 1 + 5;

        /// <summary>
        /// ×¢²áÐÐÎª
        /// </summary>
        public virtual void RegisterBehaviors()
        {
        }

        /// <summary>
        /// ×¢²áRpc
        /// </summary>
        public virtual void RegisterRpcs()
		{
			RegisterRpc("Move", Move, typeof(Vector3));
			RegisterRpc("SetHP", SetHP, typeof(float));
		}

		

		/// <summary>
		/// Arguments:
		/// Vector3 target
		/// </summary>
		public virtual void Move(RpcArgs args){}
		/// <summary>
		/// Arguments:
		/// float hp
		/// </summary>
		public virtual void SetHP(RpcArgs args){}
		

        //-----------------------------------------------------------------
        // RPC End
        //=================================================================
	}
}
