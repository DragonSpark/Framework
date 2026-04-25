using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeSearchFilter : ISelect<Syncfusion.Blazor.Data.SearchFilter, SearchFilter>
{
	public static ComposeSearchFilter Default { get; } = new();

	ComposeSearchFilter() {}

	public SearchFilter Get(Syncfusion.Blazor.Data.SearchFilter parameter)
		=> new(parameter.Fields.Verify(), parameter.Key.Account() ?? string.Empty,
		       parameter.Operator.Account() ?? "contains", parameter.IgnoreCase, parameter.IgnoreAccent);
}