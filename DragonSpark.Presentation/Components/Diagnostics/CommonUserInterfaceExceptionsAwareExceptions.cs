using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CommonUserInterfaceExceptionsAwareExceptions : IExceptions
{
	readonly IExceptions           _previous;
	readonly ICondition<Exception> _process;

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous)
		: this(previous, IgnoreException.Default.Then().Inverse().Out()) {}

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous, ICondition<Exception> process)
	{
		_previous = previous;
		_process  = process;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var (_, exception) = parameter;
		return _process.Get(exception) ||
		       (exception.InnerException is not null && _process.Get(exception.InnerException))
			       ? _previous.Get(parameter)
			       : ValueTask.CompletedTask;
	}
}