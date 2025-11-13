using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store;

sealed class Access<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly IMemoryCache      _memory;
	readonly Getting<TIn, TOut>    _getting;
	readonly Func<TIn, object> _key;

	public Access(IMemoryCache memory, Func<TIn, object> key, Getting<TIn, TOut> getting)
	{
		_memory = memory;
		_getting    = getting;
		_key    = key;
	}

	public TOut Get(TIn parameter)
	{
		var key    = _key(parameter);
		var value  = _memory.TryGetValue(key, out var stored);
		var result = value ? stored!.To<TOut>() : _getting((key, parameter));
		return result;
	}
}