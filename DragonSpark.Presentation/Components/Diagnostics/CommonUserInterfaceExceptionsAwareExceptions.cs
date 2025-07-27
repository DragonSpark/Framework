using DragonSpark.Application.Diagnostics;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CommonUserInterfaceExceptionsAwareExceptions : IExceptions
{
	readonly IExceptions _previous;

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous) => _previous = previous;

	public ValueTask Get(ExceptionInput parameter)
	{
		var (_, exception) = parameter;
		return exception is TaskCanceledException or JSDisconnectedException ||
		       exception.InnerException is TaskCanceledException or JSDisconnectedException
			       ? ValueTask.CompletedTask
			       : _previous.Get(parameter);
	}
}