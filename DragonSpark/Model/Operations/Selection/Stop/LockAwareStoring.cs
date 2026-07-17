using AsyncUtilities;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class LockAwareStoring<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly IStopAware<TIn, TOut> _previous;
	readonly AsyncLock             _lock;

	public LockAwareStoring(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source)
		: this(new Storing(store, source)) {}

	public LockAwareStoring(IStopAware<TIn, TOut> previous) : this(previous, new()) {}

	public LockAwareStoring(IStopAware<TIn, TOut> previous, AsyncLock @lock)
	{
		_previous = previous;
		_lock     = @lock;
	}

	sealed class Storing : Storing<TIn, TOut>
	{
		public Storing(ITable<TIn, TOut> store, Func<Stop<TIn>, ValueTask<TOut>> source) : base(store, source) {}
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		using var @lock = await _lock.LockAsync(parameter).On();
		return await _previous.Off(parameter);
	}
}