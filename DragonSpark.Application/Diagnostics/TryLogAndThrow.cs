using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class TryLogAndThrow<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IExceptionLogger      _logger;

	protected TryLogAndThrow(ISelecting<TIn, TOut> previous, IExceptionLogger logger)
	{
		_previous = previous;
		_logger   = logger;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		try
		{
			return await _previous.Off(parameter);
		}
		catch (Exception e)
		{
			// ReSharper disable once UnthrowableException
			// ReSharper disable once ThrowingSystemException
			throw await _logger.Off(new(GetType(), e));
		}
	}
}