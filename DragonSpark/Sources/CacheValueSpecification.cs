using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Sources
{
	public class CacheValueSpecification<TInstance, TValue> : CacheContainsSpecification<TInstance, TValue> where TInstance : class
	{
		readonly Func<TValue> value;

		public CacheValueSpecification( ICache<TInstance, TValue> cache, Func<TValue> value ) : base( cache )
		{
			this.value = value;
		}

		public override bool IsSatisfiedBy( TInstance parameter ) => base.IsSatisfiedBy( parameter ) && Equals( Cache.Get( parameter ), value() );
	}
}