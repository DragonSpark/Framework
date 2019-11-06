using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Text
{
	public class Selection<TIn, TOut> : Select<string, TIn, TOut>
	{
		public Selection(ISelect<TIn, TOut> @default, params Pair<string, Func<TIn, TOut>>[] pairs)
			: this(Start.A.Selection.Of<string>()
			            .By.Returning(@default.ToDelegate())
			            .Unless(Pairs.Select(pairs))
			            .To(NullOrEmpty.Default.Select)) {}

		public Selection(ISelect<string, Func<TIn, TOut>> select) : base(select) {}
	}
}