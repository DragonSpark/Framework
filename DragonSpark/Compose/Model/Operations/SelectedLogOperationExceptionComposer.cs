using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Compose.Model.Operations;

public class SelectedLogOperationExceptionComposer<TIn, TOut>
{
	readonly ILogException<TOut> _log;
	readonly IStopAware<TIn>     _operation;

	public SelectedLogOperationExceptionComposer(IStopAware<TIn> operation, ILogException<TOut> log)
	{
		_operation = operation;
		_log       = log;
	}

	public PolicyAwareLogOperationExceptionComposer<TIn> Calling(Func<TIn, TOut> select)
		=> new(_operation, new SelectedLogException<TIn, TOut>(select, _log));
}