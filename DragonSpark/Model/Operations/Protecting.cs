using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations;

public class Protecting<T> : IOperation<T>
{
	readonly ISelect<T, ValueTask> _previous;
	readonly AsyncLock             _lock;

	public Protecting(IOperation<T> previous) : this(previous, new AsyncLock()) {}

	public Protecting(ISelect<T, ValueTask> previous, AsyncLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask Get(T parameter)
	{
		using var @lock = await _lock.LockAsync().On();
		await _previous.Off(parameter);
	}
}