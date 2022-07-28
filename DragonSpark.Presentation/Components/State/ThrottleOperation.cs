using DragonSpark.Application.Runtime;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Presentation.Components.State;

public class ThrottleOperation<T> : IOperation<T>
{
	readonly IThrottling<T> _throttling;
	readonly Operate<T>     _operate;

#pragma warning disable CS8714
	public ThrottleOperation(TimeSpan window, Operate<T> operate)
		: this(new Throttling<T>(new Table<T, Timer>(), window), operate) {}
#pragma warning restore CS8714
	public ThrottleOperation(IThrottling<T> throttling, Operate<T> operate)
	{
		_throttling = throttling;
		_operate    = operate;
	}

	public ValueTask Get(T parameter)
	{
		_throttling.Execute(new(parameter, _operate));
		return ValueTask.CompletedTask;
	}
}