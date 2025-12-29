using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public sealed class ExceptionAware<T> : IStopAware<T>
{
	readonly Func<Stop<T>, ValueTask> _previous;
	readonly IExceptions              _exceptions;
	readonly Type?                    _reportedType;

	public ExceptionAware(ISelect<Stop<T>, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
		: this(previous.Get, exceptions, reportedType) {}

	public ExceptionAware(Func<Stop<T>, ValueTask> previous, IExceptions exceptions, Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask Get(Stop<T> parameter)
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