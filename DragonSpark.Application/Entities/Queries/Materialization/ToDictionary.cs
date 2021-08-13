using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class ToDictionary<TIn, TKey, TValue> : Materialize<TIn, TValue, IReadOnlyDictionary<TKey, TValue>>
		where TKey : notnull
	{
		public ToDictionary(IQuery<TIn, TValue> query, Func<TValue, TKey> key)
			: this(query, new DictionaryMaterializer<TValue, TKey>(key)) {}

		protected ToDictionary(IQuery<TIn, TValue> query, IMaterializer<TValue, IReadOnlyDictionary<TKey, TValue>> materializer)
			: base(query, materializer) {}
	}

	public class ToDictionary<TIn, T, TKey, TValue> : Materialize<TIn, T, IReadOnlyDictionary<TKey, TValue>>
		where TKey : notnull
	{
		public ToDictionary(IQuery<TIn, T> query, Func<T, TKey> key, Func<T, TValue> value)
			: this(query, new DictionaryMaterializer<T, TKey, TValue>(key, value)) {}

		protected ToDictionary(IQuery<TIn, T> query, IMaterializer<T, IReadOnlyDictionary<TKey, TValue>> materializer)
			: base(query, materializer) {}
	}

}