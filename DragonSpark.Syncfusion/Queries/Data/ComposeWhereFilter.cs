using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeWhereFilter : ISelect<Syncfusion.Blazor.Data.WhereFilter, WhereFilter>
{
	public static ComposeWhereFilter Default { get; } = new();

	ComposeWhereFilter() {}

	public WhereFilter Get(Syncfusion.Blazor.Data.WhereFilter parameter)
		=> new(parameter.Field.Account() ?? string.Empty,
		       parameter.IgnoreCase,
		       parameter.IgnoreAccent,
		       parameter.IsComplex,
		       parameter.Operator.Account() ?? "equal",
		       parameter.Condition.Account() ?? "and",
		       parameter.value.Verify(),
		       predicates: parameter.predicates.Account()?.Select(Get).ToList() ?? []);
}