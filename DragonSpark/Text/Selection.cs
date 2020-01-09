using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text
{
	public class Selection<TIn, TOut> : Select<string, TIn, TOut>
	{
		public Selection(ISelect<TIn, TOut> @default, params Pair<string, Func<TIn, TOut>>[] pairs)
			: base(Start.A.Selection.Of<string>()
			            .By.Returning(@default.ToDelegate())
			            .Then()
			            .Unless.Using(pairs.ToSelect()).ResultsInAssigned()
			            .Get()
			            .To(NullOrEmpty.Default.Select)) {}
	}
}