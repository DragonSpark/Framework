using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model;

public class RemoveFromMemory<TFrom, TTo> : Select<TFrom, TTo>
{
	protected RemoveFromMemory(ISelect<TFrom, TTo> previous, IMemoryCache memory, Func<TFrom, string> key)
		: this(previous, memory.Remove, key) {}

	protected RemoveFromMemory(ISelect<TFrom, TTo> previous, Action<string> remove, Func<TFrom, string> key)
		: base(Start.A.Selection(key).Then().Terminate(remove).ToConfiguration().Select(previous)) {}

	protected RemoveFromMemory(ISelect<TFrom, TTo> previous, Func<string, bool> remove, Func<TFrom, string> key)
		: base(Start.A.Selection(key).Then().Terminate(remove).ToConfiguration().Select(previous)) {}
}

public class RemoveFromMemory<T> : ICommand<T>
{
	readonly IMemoryCache       _memory;
	readonly ISelect<T, string> _key;

	protected RemoveFromMemory(IMemoryCache memory, ISelect<T, string> key)
	{
		_memory = memory;
		_key    = key;
	}

	public void Execute(T parameter)
	{
		_memory.Remove(_key.Get(parameter));
	}
}