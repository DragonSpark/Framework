using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime.Invocation;

sealed class Stripe<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<Func<TIn, TOut>> where TIn : notnull
{
	readonly static Func<TIn, object> Lock = Locks<TIn>.Default.Get;

	readonly Func<TIn, object> _lock;
	readonly Func<TIn, TOut>   _source;

	public Stripe(Func<TIn, TOut> source) : this(Lock, source) {}

	public Stripe(Func<TIn, object> @lock, Func<TIn, TOut> source)
	{
		_lock   = @lock;
		_source = source;
	}

	public TOut Get(TIn parameter)
	{
		lock (_lock(parameter))
		{
			return _source(parameter);
		}
	}
}