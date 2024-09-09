using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Compose.Model.Operations;

public class PolicyAwareLogOperationExceptionComposer<T> : LogOperationExceptionComposer<T>
{
	readonly ISelect<T, ValueTask> _operation;
	readonly ILogException<T>      _log;

	public PolicyAwareLogOperationExceptionComposer(ISelect<T, ValueTask> operation, ILogException<T> log)
		: base(operation, log)
	{
		_operation = operation;
		_log       = log;
	}

	public LogOperationExceptionComposer<T> When<TException>() where TException : Exception
		=> When(Is.Of<TException>());

	public LogOperationExceptionComposer<T> When(Func<Exception, bool> condition)
	{
		var log = new PolicyAwareLogException<T>(Start.A.Condition<Exception>()
		                                              .By.Calling(condition)
		                                              .Out(), _log);
		var result = new LogOperationExceptionComposer<T>(_operation, log);
		return result;
	}
}