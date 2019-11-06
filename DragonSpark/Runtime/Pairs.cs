using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime
{
	public static class Pairs
	{
		public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
			=> new Pair<TKey, TValue>(key, value);

		public static ISelect<T, TIn, TOut> Select<T, TIn, TOut>(params Pair<T, Func<TIn, TOut>>[] pairs)
			=> pairs.ToSelect();
	}
}