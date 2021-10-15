using System;

namespace DragonSpark.Model.Selection;

public class Selection<TIn, TFrom, TTo> : ISelect<TIn, TTo>
{
	readonly Func<TFrom, TTo> _current;
	readonly Func<TIn, TFrom> _previous;

	public Selection(ISelect<TIn, TFrom> previous, ISelect<TFrom, TTo> current)
		: this(previous.Get, current.Get) {}

	public Selection(Func<TIn, TFrom> previous, Func<TFrom, TTo> current)
	{
		_previous = previous;
		_current  = current;
	}

	public TTo Get(TIn parameter) => _current(_previous(parameter));
}