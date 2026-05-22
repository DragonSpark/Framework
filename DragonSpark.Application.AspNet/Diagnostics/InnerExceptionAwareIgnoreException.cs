using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Application.AspNet.Diagnostics;

sealed class InnerExceptionAwareIgnoreException : ICondition<Exception>
{
	public static InnerExceptionAwareIgnoreException Default { get; } = new();

	InnerExceptionAwareIgnoreException() : this(IgnoreException.Default.Get, InnerExceptions.Default) {}

	readonly Func<Exception, bool>                                         _previous;
	readonly ISelect<Exception, InnerExceptions.InnerExceptionsEnumerable> _exceptions;

	public InnerExceptionAwareIgnoreException(Func<Exception, bool> previous,
	                                          ISelect<Exception, InnerExceptions.InnerExceptionsEnumerable> exceptions)
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