using DragonSpark.Application.AspNet.Diagnostics;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class CommonUserInterfaceExceptionsAwareExceptions : IExceptions
{
	readonly IExceptions           _previous;
	readonly ICondition<Exception> _process;

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous) : this(previous, ShouldProcess.Default) {}

	public CommonUserInterfaceExceptionsAwareExceptions(IExceptions previous, ICondition<Exception> process)
	{
		_previous = previous;
		_process  = process;
	}

	public ValueTask Get(ExceptionInput parameter)
	{
		var (_, exception) = parameter;
		return _process.Get(exception) ? _previous.Get(parameter) : ValueTask.CompletedTask;		
	}
}