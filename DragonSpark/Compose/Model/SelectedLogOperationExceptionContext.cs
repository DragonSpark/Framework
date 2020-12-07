using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Diagnostics;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class SelectedLogOperationExceptionContext<TIn, TOut>
	{
		readonly ILogException<TOut>     _log;
		readonly ISelect<TIn, ValueTask> _operation;

		public SelectedLogOperationExceptionContext(ISelect<TIn, ValueTask> operation, ILogException<TOut> log)
		{
			_operation = operation;
			_log       = log;
		}

		public OperationContext<TIn> WithArguments(Func<TIn, TOut> select)
			=> new SelectedExceptionAwareOperation<TIn, TOut>(_operation, select, _log).Then();
	}
}