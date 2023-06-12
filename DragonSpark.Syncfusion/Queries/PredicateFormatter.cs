using DragonSpark.Compose;
using DragonSpark.Text;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class PredicateFormatter : IFormatter<WhereFilter>
{
	public static PredicateFormatter Default { get; } = new();

	PredicateFormatter() {}

	public string Get(WhereFilter parameter)
		=> parameter.Field.Account() is not null ? $"{parameter.Field}={parameter.value}" : string.Empty;
}