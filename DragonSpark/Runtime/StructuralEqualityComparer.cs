using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Runtime
{
	public class StructuralEqualityComparer<T> : IEqualityComparer<T>
	{
		readonly IEqualityComparer comparer;
		public static StructuralEqualityComparer<T> Default { get; } = new StructuralEqualityComparer<T>();
		StructuralEqualityComparer() : this( StructuralComparisons.StructuralEqualityComparer ) {}

		public StructuralEqualityComparer( IEqualityComparer comparer )
		{
			this.comparer = comparer;
		}

		public bool Equals( T x, T y ) => comparer.Equals( x, y );

		public int GetHashCode( T obj ) => comparer.GetHashCode( obj );
	}
}