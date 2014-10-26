using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	internal class Signature : IEquatable<Signature>
	{
		#region Fields
		public int hashCode;

		public DynamicProperty[] properties;
		#endregion

		public Signature(IEnumerable<DynamicProperty> properties)
		{
			this.properties = properties.ToArray();
			hashCode = 0;
			foreach (DynamicProperty p in properties)
				hashCode ^= p.Name.GetHashCode() ^ p.Type.GetHashCode();
		}

		#region IEquatable<Signature> Members
		public bool Equals(Signature other)
		{
			if (properties.Length != other.properties.Length)
				return false;
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].Name != other.properties[i].Name || properties[i].Type != other.properties[i].Type)
					return false;
			}
			return true;
		}
		#endregion

		public override int GetHashCode()
		{
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			return obj is Signature ? Equals((Signature)obj) : false;
		}
	}
}