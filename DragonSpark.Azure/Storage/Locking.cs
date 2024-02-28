using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Medallion.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class Locking<T> : IOperation<T>
{
	readonly IOperation<T>                                            _previous;
	readonly ISelect<T, ValueTask<IDistributedSynchronizationHandle>> _lock;

	protected Locking(IOperation<T> previous, IDistributedLock @lock)
		: this(previous, @lock.Then().Accept<T>().Out()) {}

	protected Locking(IOperation<T> previous, IDistributedLock<T> @lock) : this(previous, A.Selection(@lock)) {}

	protected Locking(IOperation<T> previous, ISelect<T, ValueTask<IDistributedSynchronizationHandle>> @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(T parameter)
	{
		await using var @lock = await _lock.Await(parameter);
		await _previous.Await(parameter);
	}
}