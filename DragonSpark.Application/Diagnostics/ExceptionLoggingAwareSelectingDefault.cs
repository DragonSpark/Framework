using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public sealed class ExceptionLoggingAwareSelectingDefault<T, TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IExceptionLogger      _logger;
	readonly TOut                  _default;

	public ExceptionLoggingAwareSelectingDefault(ISelecting<TIn, TOut> previous, IExceptionLogger logger, TOut @default)
	{
		_previous = previous;
		_logger   = logger;
		_default  = @default;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		try
		{
			return await _previous.Await(parameter);
		}
		// ReSharper disable once CatchAllClause
		catch (Exception error)
		{
			await _logger.Await(new(A.Type<T>(), error));
			return _default;
		}
	}
}