using System;

namespace DragonSpark.Model.Selection;

public class SelectedInstanceSelector<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly Func<TIn, Func<TIn, TOut>> _select;

	public SelectedInstanceSelector(Func<TIn, Func<TIn, TOut>> select) => _select = select;

	public TOut Get(TIn parameter) => _select(parameter)(parameter);
}