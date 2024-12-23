using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Mobile.Run;

sealed class ApplicationErrorHandler : IApplicationErrorHandler
{
	readonly IExceptions _exceptions;
	readonly Type        _owner;

	[ActivatorUtilitiesConstructor]
	public ApplicationErrorHandler(IExceptions exceptions) : this(exceptions, A.Type<ApplicationErrorHandler>()) {}

	public ApplicationErrorHandler(IExceptions exceptions, Type owner)
	{
		_exceptions = exceptions;
		_owner      = owner;
	}

	public void Execute(Exception parameter)
	{
		_exceptions.Allocate(new(_owner, parameter)).GetAwaiter().GetResult();
	}
}