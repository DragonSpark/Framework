using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public class DictionaryMaterializer<T, TKey> : DictionaryMaterializer<T, TKey, T> where TKey : notnull
{
	public DictionaryMaterializer(Func<T, TKey> key) : base(key, x => x) {}
}

public class DictionaryMaterializer<T, TKey, TValue> : IMaterializer<T, IReadOnlyDictionary<TKey, TValue>>
	where TKey : notnull
{
	readonly Func<T, TKey>   _key;
	readonly Func<T, TValue> _value;

	public DictionaryMaterializer(Func<T, TKey> key, Func<T, TValue> value)
	{
		_key   = key;
		_value = value;
	}

	public async ValueTask<IReadOnlyDictionary<TKey, TValue>> Get(IQueryable<T> parameter)
		=> await parameter.ToDictionaryAsync(_key, _value).ConfigureAwait(false);
}