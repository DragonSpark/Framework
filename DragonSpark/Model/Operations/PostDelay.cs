using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class PostDelay<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly TimeSpan      _wait;

	public PostDelay(IOperation<T> previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	public PostDelay(IOperation<T> previous, TimeSpan wait)
	{
		_previous = previous;
		_wait     = wait;
	}

	public async ValueTask Get(T parameter)
	{
		await _previous.Await(parameter);
		await Task.Delay(_wait).ConfigureAwait(false);
	}
}