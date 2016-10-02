using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Parameterized
{
	sealed class CacheParameterHandler<TKey, TValue> : IParameterAwareHandler
	{
		readonly ICache<TKey, TValue> cache;

		public CacheParameterHandler( ICache<TKey, TValue> cache )
		{
			this.cache = cache;
		}

		public bool Handles( object parameter ) => parameter is TKey && cache.Contains( (TKey)parameter );
		public bool Handle( object parameter, out object handled )
		{
			var result = Handles( parameter );
			handled = result ? cache.Get( (TKey)parameter ) : default(TValue);
			return result;
		}
	}
}