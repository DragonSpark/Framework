using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Diagnostics;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

public class LogOperationExceptionComposer<T> : IResult<IStopAware<T>>
{
	public static implicit operator Func<Stop<T>, ValueTask>(LogOperationExceptionComposer<T> instance)
		=> instance.Get().Get;

	/*public static implicit operator Operate<T>(LogOperationExceptionComposer<T> instance) => instance.Get().Get;

	public static implicit operator Await<T>(LogOperationExceptionComposer<T> instance) => instance.Get().Off;*/

	readonly ISelect<Stop<T>, ValueTask> _operation;
	readonly ILogException<T>            _log;

	public LogOperationExceptionComposer(ISelect<Stop<T>, ValueTask> operation, ILogException<T> log)
	{
		_operation = operation;
		_log       = log;
	}

	public IStopAware<T> Get() => new ExceptionAwareOperation<T>(_operation, _log);
}