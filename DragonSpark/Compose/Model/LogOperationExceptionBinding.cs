using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

// ReSharper disable PossibleMultipleConsumption
namespace DragonSpark.Compose.Model
{
	sealed class LogOperationExceptionBinding<TIn, TOut> : IOperation<TIn>
	{
		readonly ILogException<TOut>     _log;
		readonly ISelect<TIn, ValueTask> _operation;
		readonly Parameter<TIn, TOut>    _select;

		public LogOperationExceptionBinding(ISelect<TIn, ValueTask> operation, Parameter<TIn, TOut> select,
		                                    ILogException<TOut> log)
		{
			_operation = operation;
			_select    = select;
			_log       = log;
		}

		public async ValueTask Get(TIn parameter)
		{
			try
			{
				await _operation.Await(parameter);
			}
			// ReSharper disable once CatchAllClause
			catch (System.Exception e)
			{
				var input = new ExceptionParameter<TOut>(e, _select((parameter, ValueTask.CompletedTask)));
				_log.Execute(input);
			}
		}
	}
}