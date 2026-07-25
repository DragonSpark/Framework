using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Exception = System.Exception;

namespace DragonSpark.Model.Operations.Diagnostics;

sealed class ExceptionAwareOperation<T> : IStopAware<T>
{
	readonly ISelect<Stop<T>, ValueTask> _operation;
	readonly ILogException<T>            _log;

	public ExceptionAwareOperation(ISelect<Stop<T>, ValueTask> operation, ILogException<T> log)
	{
		_operation = operation;
		_log       = log;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		try
		{
			await _operation.Off(parameter);
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_log.Execute(new(e, parameter));
		}
	}
}