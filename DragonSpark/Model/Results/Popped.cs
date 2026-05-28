using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Results;

sealed class Popped<T> : IResult<T>
{
	readonly IMutable<T?> _previous;

	public Popped(T instance) : this((IMutable<T?>)new Variable<T>(instance)) {}

	public Popped(IMutable<T?> previous) => _previous = previous;

	public T Get() => _previous.TryPop(out var result) && result is not null
		                  ? result
		                  : throw new InvalidOperationException($"The instance of {A.Type<T>()} has already been popped!");
}