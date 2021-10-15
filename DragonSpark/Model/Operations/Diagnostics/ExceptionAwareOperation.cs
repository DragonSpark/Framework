using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Model.Operations.Diagnostics;

sealed class ExceptionAwareOperation<T> : IOperation<T>
{
	readonly ISelect<T, ValueTask> _operation;
	readonly ILogException<T>      _log;

	public ExceptionAwareOperation(ISelect<T, ValueTask> operation, ILogException<T> log)
	{
		_operation = operation;
		_log       = log;
	}

	public async ValueTask Get(T parameter)
	{
		try
		{
			await _operation.Await(parameter);
		}
		// ReSharper disable once CatchAllClause
		catch (Exception e)
		{
			_log.Execute(new ExceptionParameter<T>(e, parameter));
		}
	}
}