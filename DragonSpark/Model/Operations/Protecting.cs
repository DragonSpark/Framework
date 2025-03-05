using System.Threading.Tasks;
using AsyncUtilities;
using DragonSpark.Compose;

namespace DragonSpark.Model.Operations;

public class Protecting<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly AsyncLock     _lock;

	public Protecting(IOperation<T> previous) : this(previous, new AsyncLock()) {}

	public Protecting(IOperation<T> previous, AsyncLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(T parameter)
	{
		using var @lock = await _lock.LockAsync().Go();
		await _previous.Await(parameter);
	}
}