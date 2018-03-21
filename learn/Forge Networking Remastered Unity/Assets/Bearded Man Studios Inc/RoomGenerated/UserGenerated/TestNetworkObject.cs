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
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class TestNetworkObject : NetworkObject
	{
		public const int IDENTITY = 2;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		private float _hp;
		public event FieldEvent<float> hpChanged;
		public InterpolateFloat hpInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float hp
		{
			get { return _hp; }
			set
			{
				// Don't do anything if the value is the same
				if (_hp == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_hp = value;
				hasDirtyFields = true;
			}
		}

		public void SethpDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_hp(ulong timestep)
		{
			if (hpChanged != null) hpChanged(_hp, timestep);
			if (fieldAltered != null) fieldAltered("hp", _hp, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			hpInterpolation.current = hpInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _hp);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_hp = UnityObjectMapper.Instance.Map<float>(payload);
			hpInterpolation.current = _hp;
			hpInterpolation.target = _hp;
			RunChange_hp(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _hp);

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
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (hpInterpolation.Enabled)
				{
					hpInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					hpInterpolation.Timestep = timestep;
				}
				else
				{
					_hp = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_hp(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (hpInterpolation.Enabled && !hpInterpolation.current.UnityNear(hpInterpolation.target, 0.0015f))
			{
				_hp = (float)hpInterpolation.Interpolate();
				//RunChange_hp(hpInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];


			InitializeRpc();
		}

		public TestNetworkObject() : base() { Initialize(); }
		public TestNetworkObject(RoomScene networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public TestNetworkObject(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	
	
        //=================================================================
        // RPC Begin
        //-----------------------------------------------------------------
	
	
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_SET_HP = 1 + 5;

		
		public void InitializeRpc()
		{
			RegisterRpc("Move", Move, typeof(Vector3));
			RegisterRpc("SetHp", SetHp, typeof(float));

			RegistrationComplete();
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
		public virtual void SetHp(RpcArgs args){}
		

        //-----------------------------------------------------------------
        // RPC End
        //=================================================================
	}
}
