using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;

namespace DragonSpark.Sources
{
	public abstract class CacheSpecificationBase<TInstance, TValue> : SpecificationBase<TInstance> where TInstance : class
	{
		protected CacheSpecificationBase( ICache<TInstance, TValue> cache )
		{
			Cache = cache;
		}

		protected ICache<TInstance, TValue> Cache { get; }
	}
}