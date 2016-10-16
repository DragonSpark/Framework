using DragonSpark.Runtime;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Activation
{
	public sealed class ConstructTypeRequest : IEquatable<ConstructTypeRequest>
	{
		readonly static StructuralEqualityComparer<object[]> Comparer = StructuralEqualityComparer<object[]>.Default;

		readonly int code;

		public ConstructTypeRequest( Type type ) : this( type, Items<object>.Default ) {}

		public ConstructTypeRequest( Type type, params object[] arguments )
		{
			RequestedType = type;
			Arguments = arguments.ToImmutableArray();

			unchecked
			{
				code = RequestedType.GetHashCode() * 397 ^ Comparer.GetHashCode( arguments );
			}
		}

		public Type RequestedType { get; }
		
		public ImmutableArray<object> Arguments { get; }

		public bool Equals( ConstructTypeRequest other ) => ReferenceEquals( this, other ) || code == other?.code;

		public override bool Equals( object obj ) => obj is ConstructTypeRequest && Equals( (ConstructTypeRequest)obj );

		public override int GetHashCode() => code;

		public static bool operator ==( ConstructTypeRequest left, ConstructTypeRequest right ) => Equals( left, right );

		public static bool operator !=( ConstructTypeRequest left, ConstructTypeRequest right ) => !Equals( left, right );
	}
}