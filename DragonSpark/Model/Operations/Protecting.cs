﻿using AsyncUtilities;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Protecting<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly AsyncLock     _lock;

	public Protecting(IOperation<T> previous) : this(previous, new AsyncLock()) {}

	public Protecting(IOperation<T> previous, AsyncLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(T parameter)
	{
		using var @lock = await _lock.LockAsync().ConfigureAwait(true);
		await _previous.Await(parameter);
	}
}