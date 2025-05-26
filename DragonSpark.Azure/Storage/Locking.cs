using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Medallion.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class Locking<T> : IStopAware<T>
{
	readonly IStopAware<T>                                                  _previous;
	readonly ISelect<Stop<T>, ValueTask<IDistributedSynchronizationHandle>> _lock;

	protected Locking(IStopAware<T> previous, IDistributedLock @lock)
		: this(previous, @lock.Then().Accept<Stop<T>>(x => x).Out()) {}

	protected Locking(IStopAware<T> previous, IDistributedLock<T> @lock) : this(previous, A.Selection(@lock)) {}

	protected Locking(IStopAware<T> previous, ISelect<Stop<T>, ValueTask<IDistributedSynchronizationHandle>> @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await using var @lock = await _lock.Off(parameter);
		await _previous.Off(parameter);
	}
}