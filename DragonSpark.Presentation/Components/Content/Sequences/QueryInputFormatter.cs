using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class QueryInputFormatter : IFormatter<QueryInput>
{
	public static QueryInputFormatter Default { get; } = new();

	QueryInputFormatter() {}

	public string Get(QueryInput parameter)
		=> $"{parameter.IncludeTotalCount}+{parameter.Filter}+{parameter.OrderBy}+{parameter.Partition}";
}