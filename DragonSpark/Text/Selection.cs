using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Text
{
	public class Selection<TIn, TOut> : Select<string, TIn, TOut>
	{
		public Selection(ISelect<TIn, TOut> @default, params Pair<string, Func<TIn, TOut>>[] pairs)
			: base(Start.A.Selection.Of<string>()
			            .By.Returning(@default.ToDelegate())
			            .Unless(pairs.ToSelect())
			            .To(NullOrEmpty.Default.Select)) {}
	}
}