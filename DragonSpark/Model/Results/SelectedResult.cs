using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model.Results;

public class SelectedResult<TIn, TOut> : IResult<TOut>
{
	public static implicit operator TOut(SelectedResult<TIn, TOut> @this) => @this.Get();
	
	readonly Func<TIn>       _previous;
	readonly Func<TIn, TOut> _source;

	protected SelectedResult(IResult<TIn> previous, ISelect<TIn, TOut> select)
		: this(previous.Get, @select.Get) {}

	public SelectedResult(Func<TIn> previous, Func<TIn, TOut> source)
	{
		_source    = source;
		_previous = previous;
	}

	public TOut Get() => _source(_previous());
}