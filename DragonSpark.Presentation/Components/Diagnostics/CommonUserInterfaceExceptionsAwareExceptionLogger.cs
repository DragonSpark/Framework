using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CommonUserInterfaceExceptionsAwareExceptionLogger : IExceptionLogger
{
	readonly IExceptionLogger _previous;

	public CommonUserInterfaceExceptionsAwareExceptionLogger(IExceptionLogger previous) => _previous = previous;

	public ValueTask<Exception> Get(ExceptionInput parameter)
		=> parameter.Exception is TaskCanceledException or JSDisconnectedException
			   ? parameter.Exception.ToOperation()
			   : _previous.Get(parameter);
}