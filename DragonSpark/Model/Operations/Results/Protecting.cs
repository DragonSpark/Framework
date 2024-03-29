﻿using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public class Protecting<T> : IResulting<T>
{
	readonly IResult<ValueTask<T>> _previous;
	readonly AsyncLock             _lock;

	public Protecting(IResult<ValueTask<T>> previous) : this(previous, new AsyncLock()) {}

	public Protecting(IResult<ValueTask<T>> previous, AsyncLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask<T> Get()
	{
		using var @lock  = await _lock.LockAsync().ConfigureAwait(true);
		var       result = await _previous.Await();
		return result;
	}
}

