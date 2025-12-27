using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class PredicateFormatter : IFormatter<Syncfusion.Blazor.Data.WhereFilter>
{
	public static PredicateFormatter Default { get; } = new();

	PredicateFormatter() {}

	public string Get(Syncfusion.Blazor.Data.WhereFilter parameter)
		=> parameter.Field.Account() is not null ? $"{parameter.Field}={parameter.value}" : string.Empty;
}