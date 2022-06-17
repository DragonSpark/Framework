using DragonSpark.Application.Diagnostics;

namespace DragonSpark.Presentation.Components.Content;

sealed class ExceptionAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T> _previous;
	readonly IExceptions        _exceptions;

	public ExceptionAwareActiveContents(IActiveContents<T> previous, IExceptions exceptions)
	{
		_previous   = previous;
		_exceptions = exceptions;
	}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter)
	{
		var (owner, _) = parameter;
		var result = _previous.Get(parameter).Then().Handle(_exceptions, owner.GetType()).Get();
		return result;
	}
}