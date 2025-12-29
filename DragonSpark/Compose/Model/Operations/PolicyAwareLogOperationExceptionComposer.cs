using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Operations.Stop;
using System;
using Exception = System.Exception;

namespace DragonSpark.Compose.Model.Operations;

public class PolicyAwareLogOperationExceptionComposer<T> : LogOperationExceptionComposer<T>
{
	readonly IStopAware<T>    _operation;
	readonly ILogException<T> _log;

	public PolicyAwareLogOperationExceptionComposer(IStopAware<T> operation, ILogException<T> log)
		: base(operation, log)
	{
		_operation = operation;
		_log       = log;
	}

	public LogOperationExceptionComposer<T> When<TException>() where TException : Exception
		=> When(Is.Of<TException>());

	public LogOperationExceptionComposer<T> When(Func<Exception, bool> condition)
	{
		var log = new PolicyAwareLogException<T>(condition.Start().Out(), _log);
		return new LogOperationExceptionComposer<T>(_operation, log);
	}
}