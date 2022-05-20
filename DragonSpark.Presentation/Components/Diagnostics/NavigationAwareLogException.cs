using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class NavigationAwareLogException : ILogException
{
	readonly ILogException _previous;

	public NavigationAwareLogException(ILogException previous) => _previous = previous;

	public ValueTask<Exception> Get(LogExceptionInput parameter)
		=> parameter.Exception is NavigationException ? parameter.Exception.ToOperation() : _previous.Get(parameter);
}