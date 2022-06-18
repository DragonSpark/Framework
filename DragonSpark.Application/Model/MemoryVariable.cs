using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model;

public class MemoryVariable<T> : IMutable<T?>
{
	readonly Func<string>         _key;
	readonly MemoryAssignment<T?> _assignment;

	protected MemoryVariable(IMemoryCache memory, string key, ICommand<ICacheEntry> configure)
		: this(memory, key.Start(), configure) {}

	protected MemoryVariable(IMemoryCache memory, Func<string> key, ICommand<ICacheEntry> configure)
		: this(key, new MemoryAssignment<T?>(memory, new ConfiguredMemoryResult<T?>(memory, configure.Execute))) {}

	protected MemoryVariable(Func<string> key, MemoryAssignment<T?> assignment)
	{
		_key        = key;
		_assignment = assignment;
	}

	public bool Pop(out T? result)
	{
		var key = _key();
		return _assignment.Pop(key, out result);
	}

	public void Remove()
	{
		var key = _key();
		_assignment.Remove(key);
	}

	public T? Get()
	{
		var key = _key();
		return _assignment.Get(key);
	}

	public void Execute(T? parameter)
	{
		var key = _key();
		_assignment.Assign(key, parameter);
	}
}