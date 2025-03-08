using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

public class LogOperationExceptionComposer<T> : IResult<IOperation<T>>
{
	public static implicit operator Func<T, ValueTask>(LogOperationExceptionComposer<T> instance)
		=> instance.Get().Get;

	public static implicit operator Operate<T>(LogOperationExceptionComposer<T> instance) => instance.Get().Get;

	public static implicit operator Await<T>(LogOperationExceptionComposer<T> instance) => instance.Get().Off;

	readonly ISelect<T, ValueTask> _operation;
	readonly ILogException<T>      _log;

	public LogOperationExceptionComposer(ISelect<T, ValueTask> operation, ILogException<T> log)
	{
		_operation = operation;
		_log       = log;
	}

	public IOperation<T> Get() => new ExceptionAwareOperation<T>(_operation, _log);
}