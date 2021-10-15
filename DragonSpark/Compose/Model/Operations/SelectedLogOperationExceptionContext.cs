using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

public class SelectedLogOperationExceptionContext<TIn, TOut>
{
	readonly ILogException<TOut>     _log;
	readonly ISelect<TIn, ValueTask> _operation;

	public SelectedLogOperationExceptionContext(ISelect<TIn, ValueTask> operation, ILogException<TOut> log)
	{
		_operation = operation;
		_log       = log;
	}

	public PolicyAwareLogOperationExceptionContext<TIn> Calling(Func<TIn, TOut> select)
		=> new(_operation, new SelectedLogException<TIn, TOut>(select, _log));
}