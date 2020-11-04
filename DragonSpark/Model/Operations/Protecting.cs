using AsyncUtilities;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Protecting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Await<TIn, TOut> _previous;
		readonly AsyncLock        _lock;

		public Protecting(ISelecting<TIn, TOut> previous) : this(previous.Await) {}

		public Protecting(Await<TIn, TOut> previous) : this(previous, new AsyncLock()) {}

		public Protecting(Await<TIn, TOut> previous, AsyncLock @lock)
		{
			_previous = previous;
			_lock     = @lock;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			using (await _lock.LockAsync().ConfigureAwait(false))
			{
				return await _previous(parameter);
			}
		}
	}
}