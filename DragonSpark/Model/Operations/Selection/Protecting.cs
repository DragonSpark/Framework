using System.Threading.Tasks;
using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Selection;

public class Protecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly Await<TIn, TOut>        _previous;
	readonly ISelect<TIn, AsyncLock> _lock;

	public Protecting(ISelecting<TIn, TOut> previous) : this(previous.Off) {}

	public Protecting(Await<TIn, TOut> previous) : this(previous, new AsyncLock()) {}

	public Protecting(Await<TIn, TOut> previous, AsyncLock @lock) : this(previous, @lock.Start().Accept<TIn>().Get()) {}

	public Protecting(Await<TIn, TOut> previous, ISelect<TIn, AsyncLock> @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var @lock = _lock.Get(parameter);
		using (await @lock.LockAsync().On())
		{
			return await _previous(parameter);
		}
	}
}