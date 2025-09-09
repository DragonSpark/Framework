using DragonSpark.Application.Diagnostics;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CommonUserInterfaceExceptionsAwareExceptions : IExceptions
{
	readonly IExceptions _previous;

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous) => _previous = previous;

	public ValueTask Get(ExceptionInput parameter)
	{
		var (_, exception) = parameter;
		return exception is OperationCanceledException or JSDisconnectedException ||
		       exception.InnerException is OperationCanceledException or JSDisconnectedException
			       ? ValueTask.CompletedTask
			       : _previous.Get(parameter);
	}
}