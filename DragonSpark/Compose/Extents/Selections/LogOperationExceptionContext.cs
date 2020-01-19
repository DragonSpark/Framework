using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Extents.Selections {
	public sealed class LogOperationExceptionContext<TIn, TOut>
	{
		readonly ISelect<TIn, ValueTask> _operation;
		readonly ILogException<TOut>     _log;

		public LogOperationExceptionContext(ISelect<TIn, ValueTask> operation, ILogException<TOut> log)
		{
			_operation = operation;
			_log       = log;
		}

		public IOperation<TIn> WithArguments(Parameter<TIn, TOut> @delegate)
			=> new LogOperationExceptionBinding<TIn, TOut>(_operation, @delegate, _log);

		public IOperation<TIn> WithArguments(Func<TIn, TOut> @delegate)
			=> WithArguments(new ParameterSelection<TIn, TOut>(@delegate).Get);
	}
}