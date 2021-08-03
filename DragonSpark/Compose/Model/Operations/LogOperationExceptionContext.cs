using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations
{
	public class LogOperationExceptionContext<T> : IResult<IOperation<T>>
	{
		public static implicit operator Func<T, ValueTask>(LogOperationExceptionContext<T> instance)
			=> instance.Get().Get;

		public static implicit operator Operate<T>(LogOperationExceptionContext<T> instance) => instance.Get().Get;

		public static implicit operator Await<T>(LogOperationExceptionContext<T> instance) => instance.Get().Await;

		readonly ISelect<T, ValueTask> _operation;
		readonly ILogException<T>      _log;

		public LogOperationExceptionContext(ISelect<T, ValueTask> operation, ILogException<T> log)
		{
			_operation = operation;
			_log       = log;
		}

		public IOperation<T> Get() => new ExceptionAwareOperation<T>(_operation, _log);
	}
}