using System.Runtime.CompilerServices;

namespace DragonSpark.Sources.Parameterized.Caching
{
	class CacheRegistry<T> : ICacheRegistry<T>
	{
		public static CacheRegistry<T> Default { get; } = new CacheRegistry<T>();

		readonly ConditionalWeakTable<object, ICache<T>> cache = new ConditionalWeakTable<object, ICache<T>>();

		public void Register( object key, ICache<T> instance ) => cache.Add( key, instance );

		public void Clear( object key, object instance )
		{
			ICache<T> property;
			if ( cache.TryGetValue( key, out property ) )
			{
				property.Remove( instance );
			}
		}
	}
}