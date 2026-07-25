using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeSearchFilterModel : ISelect<SearchFilter, Syncfusion.Blazor.Data.SearchFilter>
{
	public static ComposeSearchFilterModel Default { get; } = new();

	ComposeSearchFilterModel() {}

	public Syncfusion.Blazor.Data.SearchFilter Get(SearchFilter parameter)
	{
		var (fields, key, @operator, ignoreCase, ignoreAccent) = parameter;
		return new ()
		{
			Fields       = fields.ToList(),
			Key          = key,
			Operator     = @operator,
			IgnoreCase   = ignoreCase,
			IgnoreAccent = ignoreAccent
		};
	}
}