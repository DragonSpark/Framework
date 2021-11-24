using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionAwareResult<T> : IResulting<T?>
{
	readonly AwaitOf<T?> _previous;
	readonly IExceptions _exceptions;
	readonly Type?       _reportedType;

	public ExceptionAwareResult(IResulting<T?> previous, IExceptions exceptions, Type? reportedType = null)
		: this(previous.Await, exceptions, reportedType) {}

	public ExceptionAwareResult(AwaitOf<T?> previous, IExceptions exceptions, Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask<T?> Get()
	{
		try
		{
			return await _previous();
		}
		catch (Exception e)
		{
			await _exceptions.Await(new(_reportedType ?? GetType(), e));
			throw;
		}
	}
}