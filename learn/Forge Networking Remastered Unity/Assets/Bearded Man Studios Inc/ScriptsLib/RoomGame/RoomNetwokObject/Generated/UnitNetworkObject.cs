using BeardedManStudios;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine;

namespace Rooms.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector3\"][\"float\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"target\"][\"hp\"]]")]
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0]")]
	public partial class UnitNetworkObject : NetworkObject
	{
		public const int IDENTITY = 1;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
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
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		private int _legionId;
		public event FieldEvent<int> legionIdChanged;
		public Interpolated<int> legionIdInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int legionId
		{
			get { return _legionId; }
			set
			{
				// Don't do anything if the value is the same
				if (_legionId == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_legionId = value;
				hasDirtyFields = true;
			}
		}

		public void SetlegionIdDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_legionId(ulong timestep)
		{
			if (legionIdChanged != null) legionIdChanged(_legionId, timestep);
			if (fieldAltered != null) fieldAltered("legionId", _legionId, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			legionIdInterpolation.current = legionIdInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }
        public override int ClassId { get { return 0; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _legionId);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_legionId = UnityObjectMapper.Instance.Map<int>(payload);
			legionIdInterpolation.current = _legionId;
			legionIdInterpolation.target = _legionId;
			RunChange_legionId(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _legionId);

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
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (legionIdInterpolation.Enabled)
				{
					legionIdInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					legionIdInterpolation.Timestep = timestep;
				}
				else
				{
					_legionId = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_legionId(timestep);
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
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (legionIdInterpolation.Enabled && !legionIdInterpolation.current.UnityNear(legionIdInterpolation.target, 0.0015f))
			{
				_legionId = (int)legionIdInterpolation.Interpolate();
				//RunChange_legionId(legionIdInterpolation.Timestep);
			}
		}

		protected virtual void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

			
            RegisterBehaviors();
			RegisterRpcs();
            ReadMetadata();

			RegistrationComplete();
		}

		public UnitNetworkObject() : base() { Initialize(); }
		public UnitNetworkObject(RoomScene networker, int createCode = 0, byte[] metadata = null) : base(networker, createCode, metadata) { Initialize(); }
		public UnitNetworkObject(RoomScene networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	
	
        //=================================================================
        // RPC Begin
        //-----------------------------------------------------------------
	
	
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_SET_H_P = 1 + 5;

		
        /// <summary>
        /// Metadata
        /// </summary>
        protected virtual void ReadMetadata()
        {
            if (Metadata != null)
            {
                byte transformFlags = Metadata[0];

                if (transformFlags != 0)
                {
                    BMSByte metadataTransform = new BMSByte();
                    metadataTransform.Clone(Metadata);
                    metadataTransform.MoveStartIndex(1);

                    if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
                    {
                        position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
                        rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
                    }
                    else if ((transformFlags & 0x01) != 0)
                    {
                        position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
                    }
                    else if ((transformFlags & 0x02) != 0)
                    {
                        rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); 
                    }
                }
            }

        }
		
        /// <summary>
        /// 
        /// </summary>
        public virtual void RegisterBehaviors()
        {
        }
		
        /// <summary>
        /// Rpc
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
