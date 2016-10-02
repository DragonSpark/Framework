using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface IAtomicCache<TArgument, TValue> : ICache<TArgument, TValue>
	{
		TValue GetOrSet( TArgument key, Func<TArgument, TValue> factory );
	}
}