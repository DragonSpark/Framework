using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime.Execution;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class ImmediateOrThrottle<T> : IOperation<T>
{
	readonly IOperation<T> _immediate;
	readonly IOperation<T> _throttle;
	readonly First         _first;

	public ImmediateOrThrottle(Func<T, ValueTask> @delegate, TimeSpan window)
		: this(new Operation<T>(@delegate), new ThrottleOperation<T>(window, new Operate<T>(@delegate))) {}

	public ImmediateOrThrottle(IOperation<T> immediate, IOperation<T> throttle)
		: this(immediate, throttle, new First()) {}

	public ImmediateOrThrottle(IOperation<T> immediate, IOperation<T> throttle, First first)
	{
		_immediate = immediate;
		_throttle  = throttle;
		_first     = first;
	}

	public async ValueTask Get(T parameter)
	{
		if (_first.Get())
		{
			await _immediate.Await(parameter);
		}

		await _throttle.Await(parameter);
		_first.Execute();
	}
}