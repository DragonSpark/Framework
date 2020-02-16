using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model
{
	public static class Pairs
	{
		public static Pair<TKey, Func<TIn, TOut>> Select<TKey, TIn, TOut>(TKey key, ISelect<TIn, TOut> select)
			=> Create(key, select.ToDelegate());

		public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
			=> new Pair<TKey, TValue>(key, value);
	}
}