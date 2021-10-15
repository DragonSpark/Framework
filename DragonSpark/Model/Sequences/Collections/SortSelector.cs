using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Collections;

sealed class SortSelector<T> : Select<T, int>
{
	public static SortSelector<T> Default { get; } = new SortSelector<T>();

	SortSelector() : base(Start.A.Selection.Of<T>()
	                           .By.Returning(-1)
	                           .Unless.Using(SortMetadata<T>.Default)
	                           .ResultsInAssigned()
	                           .Unless.Input.IsOf(Start.A.Selection<ISortAware>()
	                                                   .By.Self.Select(A.Result)
	                                                   .Get()
	                                                   .Then()
	                                                   .Value()
	                                                   .Get())) {}
}