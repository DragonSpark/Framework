using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem
{
	public sealed class AssignableEqualityComparer : IEqualityComparer<Type>
	{
		readonly ImmutableArray<Type> types;

		public AssignableEqualityComparer( params Type[] types )
		{
			this.types = types.ToImmutableArray();
		}

		public bool Equals( Type x, Type y ) => x.Adapt().IsAssignableFrom( y );

		public int GetHashCode( Type obj )
		{
			foreach ( var type in types )
			{
				if ( Equals( type, obj ) )
				{
					return type.GetHashCode();
				}
			}
			return obj.GetHashCode();
		}
	}
}
