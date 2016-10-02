using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources
{
	public class CacheContainsSpecification<TInstance, TValue> : CacheSpecificationBase<TInstance, TValue> where TInstance : class
	{
		public CacheContainsSpecification( ICache<TInstance, TValue> cache ) : base( cache ) {}

		public override bool IsSatisfiedBy( TInstance parameter ) => Cache.Contains( parameter );
	}
}