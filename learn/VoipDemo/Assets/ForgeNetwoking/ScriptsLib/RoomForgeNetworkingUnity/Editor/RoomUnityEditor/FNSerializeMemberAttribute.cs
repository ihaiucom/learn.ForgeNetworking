using System;

namespace Rooms.Forge.Networking.UnityEditor.Serializer
{
	/// <summary>
	/// Declare a member as serializable
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class FNSerializeMemberAttribute : Attribute
	{
	}
}
