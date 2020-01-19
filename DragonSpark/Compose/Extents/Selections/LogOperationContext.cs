using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Extents.Selections
{
	public sealed class LogOperationContext<T, TParameter>
	{
		readonly ILogMessage<TParameter> _log;
		readonly ISelect<T, ValueTask>   _operation;

		public LogOperationContext(ISelect<T, ValueTask> operation, ILogMessage<TParameter> log)
		{
			_operation = operation;
			_log       = log;
		}

		public IOperation<T> WithArguments(Func<(T Parameter, ValueTask Task), TParameter> @delegate)
			=> _operation.Then()
			             .Configure(@delegate.Start().Terminate(_log).Get())
			             .Out();

		public IOperation<T> WithArguments(Func<T, TParameter> @delegate)
			=> WithArguments(new ParameterSelection<T, TParameter>(@delegate).Get);
	}
}