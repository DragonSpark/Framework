using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class QueryInputFormatter : IFormatter<PageInput>
{
	public static QueryInputFormatter Default { get; } = new();

	QueryInputFormatter() {}

	public string Get(PageInput parameter)
		=> $"{parameter.IncludeTotalCount}+{parameter.Filter}+{parameter.OrderBy}+{parameter.Partition}";
}