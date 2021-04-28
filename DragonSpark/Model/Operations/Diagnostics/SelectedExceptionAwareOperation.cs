using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Model.Operations.Diagnostics
{
	sealed class SelectedExceptionAwareOperation<TIn, TOut> : IOperation<TIn>
	{
		readonly ILogException<TOut>     _log;
		readonly ISelect<TIn, ValueTask> _operation;
		readonly Func<TIn, TOut>         _select;

		public SelectedExceptionAwareOperation(ISelect<TIn, ValueTask> operation, Func<TIn, TOut> select,
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
			catch (Exception e)
			{
				_log.Execute(new ExceptionParameter<TOut>(e, _select(parameter)));
			}
		}
	}
}