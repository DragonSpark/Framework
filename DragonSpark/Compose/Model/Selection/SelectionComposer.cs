using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Selection;

public class SelectionComposer<_, TIn, TOut> : Composer<_, ISelect<TIn, TOut>>
{
	public SelectionComposer(ISelect<_, ISelect<TIn, TOut>> subject) : base(subject) {}

	public Composer<_, Func<TIn, TOut>> Delegate() => Select(DelegateSelector<TIn, TOut>.Default);
}