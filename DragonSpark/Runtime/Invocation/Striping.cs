using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Runtime.Invocation;

public class Striping<TIn, TOut> : ISelecting<TIn, TOut> where TIn : notnull
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly StripedAsyncLock<TIn> _lock;

	public Striping(ISelecting<TIn, TOut> previous) : this(previous, new (16)) {}

	public Striping(ISelecting<TIn, TOut> previous, StripedAsyncLock<TIn> @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		using var @lock  = await _lock.LockAsync(parameter).ConfigureAwait(false);
		var       result = await _previous.Await(parameter);
		return result;
	}
}