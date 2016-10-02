using System;

namespace DragonSpark.Activation
{
	public abstract class TypeRequest : IEquatable<TypeRequest>
	{
		readonly int code;

		protected TypeRequest( Type type )
		{
			RequestedType = type;

			code = RequestedType.GetHashCode();
		}

		public Type RequestedType { get; }

		public bool Equals( TypeRequest other ) => ReferenceEquals( this, other ) || RequestedType == other?.RequestedType;

		public override bool Equals( object obj ) => obj is TypeRequest && Equals( (TypeRequest)obj );

		public override int GetHashCode() => code;

		public static bool operator ==( TypeRequest left, TypeRequest right ) => Equals( left, right );

		public static bool operator !=( TypeRequest left, TypeRequest right ) => !Equals( left, right );
	}
}