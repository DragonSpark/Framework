using System;

namespace DragonSpark.Model.Selection;

public class Configured<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly Action<TIn>     _configure;
	readonly Func<TIn, TOut> _select;

	public Configured(ISelect<TIn, TOut> select, Action<TIn> configure) : this(select.Get, configure) {}

	public Configured(Func<TIn, TOut> select, Action<TIn> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public TOut Get(TIn parameter)
	{
		_configure(parameter);
		return _select(parameter);
	}
}