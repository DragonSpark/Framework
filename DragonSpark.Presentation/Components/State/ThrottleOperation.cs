using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public class ThrottleOperation<T> : IOperation<T>
{
	readonly IThrottling<T> _throttling;
	readonly Operate<T>     _operate;

	public ThrottleOperation(TimeSpan window, Operate<T> operate) : this(new Throttling<T>(window), operate) {}

	public ThrottleOperation(IThrottling<T> throttling, Operate<T> operate)
	{
		_throttling = throttling;
		_operate    = operate;
	}

	public ValueTask Get(T parameter)
	{
		var source = new TaskCompletionSource();
		_throttling.Execute(new(parameter, _operate, source));
		return source.Task.ToOperation();
	}
}