using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class DecoratedSourceCache<T> : DecoratedSourceCache<object, T>, ICache<T>
	{
		public DecoratedSourceCache() : this( new WritableSourceCache<object, T>() ) {}
		public DecoratedSourceCache( Func<object, T> create ) : this( new WritableSourceCache<object, T>( create ) ) {}

		public DecoratedSourceCache( ISourceCache<object, T> inner ) : base( inner ) {}
	}
}