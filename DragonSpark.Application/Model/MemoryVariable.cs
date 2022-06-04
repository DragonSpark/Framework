using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model;

public class MemoryVariable<T> : IMutable<T?>
{
	readonly IMemoryCache               _memory;
	readonly Func<string>               _key;
	readonly IConfiguredMemoryResult<T> _configured;

	protected MemoryVariable(IMemoryCache memory, string key, ICommand<ICacheEntry> configure)
		: this(memory, key.Start(), new ConfiguredMemoryResult<T>(memory, configure.Execute)) {}

	protected MemoryVariable(IMemoryCache memory, Func<string> key, ICommand<ICacheEntry> configure)
		: this(memory, key, new ConfiguredMemoryResult<T>(memory, configure.Execute)) {}

	protected MemoryVariable(IMemoryCache memory, Func<string> key, IConfiguredMemoryResult<T> configured)
	{
		_memory     = memory;
		_key        = key;
		_configured = configured;
	}

	public bool Pop(out T? result)
	{
		var key = _key();
		if (_memory.TryGetValue(key, out result))
		{
			Remove();
			return true;
		}

		return false;
	}

	public void Remove()
	{
		var key = _key();
		_memory.Remove(key);
	}

	public T? Get()
	{
		var key = _key();
		return _memory.TryGetValue(key, out var stored) ? stored.To<T?>() : default;
	}

	public void Execute(T? parameter)
	{
		if (parameter is not null)
		{
			var key = _key();
			_configured.Get((parameter, key));
		}
		else
		{
			Remove();
		}
	}
}