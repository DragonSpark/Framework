using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToDictionary<T, TKey> : IEvaluate<T, Dictionary<TKey, T>> where TKey : notnull
{
	readonly Func<T, TKey> _key;

	public ToDictionary(Func<T, TKey> key) => _key = key;

	public ValueTask<Dictionary<TKey, T>> Get(IAsyncEnumerable<T> parameter) => parameter.ToDictionaryAsync(_key);
}

sealed class ToDictionary<T, TKey, TValue> : IEvaluate<T, Dictionary<TKey, TValue>> where TKey : notnull
{
	readonly Func<T, TKey>   _key;
	readonly Func<T, TValue> _value;

	public ToDictionary(Func<T, TKey> key, Func<T, TValue> value)
	{
		_key   = key;
		_value = value;
	}

	public ValueTask<Dictionary<TKey, TValue>> Get(IAsyncEnumerable<T> parameter)
		=> parameter.ToDictionaryAsync(_key, _value);
}