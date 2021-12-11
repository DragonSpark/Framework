using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics
{
	public class TryLogOperation : IOperation
	{
		readonly IOperation       _previous;
		readonly IExceptionLogger _logger;

		public TryLogOperation(IOperation previous, IExceptionLogger logger)
		{
			_previous = previous;
			_logger   = logger;
		}

		public async ValueTask Get()
		{
			try
			{
				await _previous.Await();
			}
			// ReSharper disable once CatchAllClause
			catch (Exception e)
			{
				await _logger.Await(new (GetType(), e));
			}
		}
	}
}
