using System.Threading.Tasks;
using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Runtime.Invocation;

public class Striping<TIn, TOut> : IStopAware<TIn, TOut> where TIn : notnull
{
	readonly ISelecting<Stop<TIn>, TOut> _previous;
	readonly StripedAsyncLock<TIn>            _lock;

	protected Striping(ISelecting<Stop<TIn>, TOut> previous) : this(previous, new(16)) {}

	protected Striping(ISelecting<Stop<TIn>, TOut> previous, StripedAsyncLock<TIn> @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		var (subject, stop) = parameter;
		using var @lock  = await _lock.LockAsync(subject, stop).Off();
		var       result = await _previous.Off(parameter);
		return result;
	}
}