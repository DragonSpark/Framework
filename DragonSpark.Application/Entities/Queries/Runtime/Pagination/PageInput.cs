using DragonSpark.Application.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public class PageInput
{
	public bool IncludeTotalCount { get; set; }

	public string? OrderBy { get; set; }

	public string? Filter { get; set; }

	public Partition? Partition { get; set; }
}