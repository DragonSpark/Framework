using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Model;

public class Removing<TFrom, TTo> : RemoveFromMemory<TFrom, ValueTask<TTo>>, ISelecting<TFrom, TTo>
{
	protected Removing(ISelect<TFrom, ValueTask<TTo>> previous, IMemoryCache memory, Func<TFrom, string> key)
		: base(previous, memory, key) {}

	protected Removing(ISelect<TFrom, ValueTask<TTo>> previous, Action<string> remove, Func<TFrom, string> key)
		: base(previous, remove, key) {}

	protected Removing(ISelect<TFrom, ValueTask<TTo>> previous, Func<string, bool> remove, Func<TFrom, string> key)
		: base(previous, remove, key) {}
}

public class Removing<T> : RemoveFromMemory<T, ValueTask>, IOperation<T>
{
	protected Removing(ISelect<T, ValueTask> previous, IMemoryCache memory, Func<T, string> key)
		: base(previous, memory, key) {}

	protected Removing(ISelect<T, ValueTask> previous, Action<string> remove, Func<T, string> key)
		: base(previous, remove, key) {}

	protected Removing(ISelect<T, ValueTask> previous, Func<string, bool> remove, Func<T, string> key)
		: base(previous, remove, key) {}
}