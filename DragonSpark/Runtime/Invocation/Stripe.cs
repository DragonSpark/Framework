using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation;

public sealed class Stripe<TIn, TOut> : ISelect<TIn, TOut> where TIn : notnull
{
	readonly ISelect<TIn, Lock> _lock;
	readonly Func<TIn, TOut>    _source;

	public Stripe(Func<TIn, TOut> source) : this(Locks<TIn>.Default, source) {}

	public Stripe(ISelect<TIn, Lock> @lock, Func<TIn, TOut> source)
	{
		_lock   = @lock;
		_source = source;
	}

	public TOut Get(TIn parameter)
	{
		lock (_lock.Get(parameter))
		{
			return _source(parameter);
		}
	}
}