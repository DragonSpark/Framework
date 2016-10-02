using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface IArgumentCache<TArgument, TValue> : IAtomicCache<TArgument, TValue>
	{
		TValue GetOrSet( TArgument key, Func<TValue> factory );
	}
}