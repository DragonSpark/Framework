using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;
// ReSharper disable PossibleMultipleConsumption
namespace DragonSpark.Compose.Extents.Selections
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

		public ValueTask Get(TIn parameter)
		{
			var result = _operation.Get(parameter);

			if (result.IsFaulted)
			{
				var input = new ExceptionParameter<TOut>(result.AsTask().Exception!, _select((parameter, result)));
				_log.Execute(input);
			}

			return result;
		}
	}
}