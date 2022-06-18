using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store;

sealed class Memory<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly IMemoryCache      _memory;
	readonly Get<TIn, TOut>    _get;
	readonly Func<TIn, object> _key;

	public Memory(IMemoryCache memory, Func<TIn, object> key, Get<TIn, TOut> get)
	{
		_memory = memory;
		_get    = get;
		_key    = key;
	}

	public TOut Get(TIn parameter)
	{
		var key    = _key(parameter);
		var value  = _memory.TryGetValue(key, out var stored);
		var result = value ? stored.To<TOut>() : _get((key, parameter));
		return result;
	}
}