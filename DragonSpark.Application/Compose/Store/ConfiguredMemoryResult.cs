using DragonSpark.Model;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store;

class ConfiguredMemoryResult<TIn, TOut> : IConfiguredMemoryResult<TIn, TOut>
{
	readonly IMemoryCache        _memory;
	readonly Func<TIn, TOut>     _source;
	readonly Action<ICacheEntry> _configure;

	public ConfiguredMemoryResult(IMemoryCache memory, Func<TIn, TOut> source, Action<ICacheEntry> configure)
	{
		_memory    = memory;
		_source    = source;
		_configure = configure;
	}

	public TOut Get(Pair<object, TIn> parameter)
	{
		var (key, @in) = parameter;
		using var entry  = _memory.CreateEntry(key);
		var       result = _source(@in);
		entry.Value = result;
		_configure(entry);
		return result;
	}
}

sealed class ConfiguredMemoryResult<T> : ConfiguredMemoryResult<T, T>, IConfiguredMemoryResult<T>
{
	readonly IMemoryCache        _memory;
	readonly Action<ICacheEntry> _configure;

	public ConfiguredMemoryResult(IMemoryCache memory, Action<ICacheEntry> configure) : base(memory, x => x, configure)
	{
		_memory    = memory;
		_configure = configure;
	}

	public void Execute(Pair<object, T?> parameter)
	{
		var (key, @in) = parameter;
		using var entry  = _memory.CreateEntry(key);
		entry.Value = @in;
		_configure(entry);
	}
}