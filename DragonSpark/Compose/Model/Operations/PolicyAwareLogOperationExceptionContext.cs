using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Compose.Model.Operations
{
	public class PolicyAwareLogOperationExceptionContext<T> : LogOperationExceptionContext<T>
	{
		readonly ISelect<T, ValueTask> _operation;
		readonly ILogException<T>      _log;

		public PolicyAwareLogOperationExceptionContext(ISelect<T, ValueTask> operation, ILogException<T> log)
			: base(operation, log)
		{
			_operation = operation;
			_log       = log;
		}

		public LogOperationExceptionContext<T> When<TException>() where TException : Exception
			=> When(Is.Of<TException>());

		public LogOperationExceptionContext<T> When(Func<Exception, bool> condition)
		{
			var log = new PolicyAwareLogException<T>(Start.A.Condition<Exception>()
			                                              .By.Calling(condition)
			                                              .Out(), _log);
			var result = new LogOperationExceptionContext<T>(_operation, log);
			return result;
		}
	}
}