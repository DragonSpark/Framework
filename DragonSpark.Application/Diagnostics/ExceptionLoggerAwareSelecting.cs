using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics
{
	public class ExceptionLoggerAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut> _previous;
		readonly IExceptionLogger      _logger;

		public ExceptionLoggerAwareSelecting(ISelecting<TIn, TOut> previous, IExceptionLogger logger)
		{
			_previous = previous;
			_logger   = logger;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			try
			{
				return await _previous.Await(parameter);
			}
			catch (Exception e)
			{
				// ReSharper disable once UnthrowableException
				throw await _logger.Await(GetType(), e);
			}
		}
	}
}