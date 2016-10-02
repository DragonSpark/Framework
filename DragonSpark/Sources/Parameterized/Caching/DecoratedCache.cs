using DragonSpark.Activation;
using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class DecoratedCache<TInstance, TValue> : CacheBase<TInstance, TValue>
	{
		readonly ICache<TInstance, TValue> cache;
		public DecoratedCache() : this( ParameterConstructor<TInstance, TValue>.Default ) {}

		public DecoratedCache( Func<TInstance, TValue> factory ) : this( CacheFactory.Create( factory ) ) {}

		public DecoratedCache( ICache<TInstance, TValue> cache )
		{
			this.cache = cache;
		}

		public override TValue Get( TInstance parameter ) => cache.Get( parameter );

		public override bool Contains( TInstance instance ) => cache.Contains( instance );

		public override bool Remove( TInstance instance ) => cache.Remove( instance );

		public override void Set( TInstance instance, TValue value ) => cache.Set( instance, value );
	}

	public class DecoratedCache<T> : DecoratedCache<object, T>
	{
		public DecoratedCache( Func<object, T> factory ) : base( factory ) {}
		public DecoratedCache( ICache<object, T> cache ) : base( cache ) {}
	}
}