using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Runtime.Assignments
{
	public sealed class CacheAssign<T1, T2> : IAssign<T1, T2>
	{
		readonly ICache<T1, T2> cache;
		public CacheAssign( ICache<T1, T2> cache )
		{
			this.cache = cache;
		}

		public void Assign( T1 first, T2 second ) => cache.SetOrClear( first, second );
	}
}