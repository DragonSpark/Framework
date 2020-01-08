using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections
{
	sealed class SortSelector<T> : Select<T, int>
	{
		public static SortSelector<T> Default { get; } = new SortSelector<T>();

		SortSelector() : base(Start.A.Selection.Of<T>()
		                           .By.Returning(-1)
		                           .Then()
		                           .Use.UseWhenAssigned(SortMetadata<T>.Default)
		                           .Use.UnlessCalling(Start.A.Selection<ISortAware>()
		                                                      .By.Self.Select(A.Result)
		                                                      .Then()
		                                                      .Value()
		                                                      .Get())) {}
	}
}