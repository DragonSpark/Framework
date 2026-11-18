using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Diagnostics;

public class InnerExceptionAwareCondition : ICondition<Exception>
{
	readonly Func<Exception, bool>                         _previous;
	readonly ISelect<Exception, InnerExceptionsEnumerable> _exceptions;

	public InnerExceptionAwareCondition(Func<Exception, bool> previous)
		: this(previous, InnerExceptions.Default) {}

	public InnerExceptionAwareCondition(Func<Exception, bool> previous,
	                                    ISelect<Exception, InnerExceptionsEnumerable> exceptions)
	{
		_previous   = previous;
		_exceptions = exceptions;
	}

	public bool Get(Exception parameter)
	{
		foreach (var inner in _exceptions.Get(parameter))
		{
			if (_previous(inner))
			{
				return true;
			}
		}

		return _previous(parameter);
	}
}