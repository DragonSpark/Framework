using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Delay<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly TimeSpan      _wait;

	public Delay(IOperation<T> previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	public Delay(IOperation<T> previous, TimeSpan wait)
	{
		_previous = previous;
		_wait     = wait;
	}

	public async ValueTask Get(T parameter)
	{
		await Task.Delay(_wait).ConfigureAwait(false);
		await _previous.Await(parameter);
	}
}