using DragonSpark.Application.Runtime;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class ThrottleOperation<T> : IOperation<T>
{
	readonly IThrottling<T> _throttling;
	readonly Operate<T>     _operate;

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