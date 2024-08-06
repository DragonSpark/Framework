using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionAware<T> : IOperation<T>
{
	readonly Func<T, ValueTask> _previous;
	readonly IExceptions        _exceptions;
	readonly Type?              _reportedType;

	public ExceptionAware(ISelect<T, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
		: this(previous.Get, exceptions, reportedType) {}

	public ExceptionAware(Func<T, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _previous(parameter).Await();
		}
		catch (Exception e)
		{
			await _exceptions.Await(new(_reportedType ?? GetType(), e));
		}
	}
}