using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Selection
{
	public class SelectionSelector<_, TIn, TOut> : Selector<_, ISelect<TIn, TOut>>
	{
		public SelectionSelector(ISelect<_, ISelect<TIn, TOut>> subject) : base(subject) {}

		public Selector<_, Func<TIn, TOut>> Delegate() => Select(DelegateSelector<TIn, TOut>.Default);
	}
}