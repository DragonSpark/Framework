using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IExceptions           _exceptions;
	readonly Type?                 _reportedType;

	public ExceptionAwareSelecting(ISelecting<TIn, TOut> previous, IExceptions exceptions,
	                               Type? reportedType = null)
	{
		_previous     = previous;
		_exceptions   = exceptions;
		_reportedType = reportedType;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		try
		{
			return await _previous.Await(parameter);
		}
		catch (Exception e)
		{
			await _exceptions.Await(new(_reportedType ?? GetType(), e));
			throw;
		}
	}
}