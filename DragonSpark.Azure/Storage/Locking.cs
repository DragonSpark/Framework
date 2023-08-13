using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

public class Locking<T> : IOperation<T>
{
	readonly IOperation<T>    _previous;
	readonly IDistributedLock _lock;

	public Locking(IOperation<T> previous, IDistributedLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(T parameter)
	{
		await using var @lock = await _lock.Await();
		await _previous.Await(parameter);
	}
}