using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Runtime.Operations;

sealed class Completed<T> : ICompleted
{
	readonly ICompleted<T>  _completed;
	readonly T              _instance;
	readonly IResulting<T?> _previous;

	public Completed(IResulting<T?> previous, ICompleted<T> completed, T instance)
	{
		_previous  = previous;
		_completed = completed;
		_instance  = instance;
	}

	public Task Get() => _completed.Get(_previous) ? _completed.Get(_instance.ToOperation<T?>()) : Task.CompletedTask;

	public void Dispose() {}
}