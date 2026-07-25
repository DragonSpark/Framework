using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Diagnostics;

public sealed class ExceptionAwareOperation<T> : IOperation<T>
{
	readonly Func<T, ValueTask> _previous;
	readonly IExceptions        _exceptions;
	readonly Type?              _reportedType;

	public ExceptionAwareOperation(ISelect<T, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
		: this(previous.Get, exceptions, reportedType) {}

	public ExceptionAwareOperation(Func<T, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _previous(parameter).On();
		}
		catch (Exception e)
		{
			await _exceptions.Off(new(_reportedType ?? GetType(), e));
		}
	}
}