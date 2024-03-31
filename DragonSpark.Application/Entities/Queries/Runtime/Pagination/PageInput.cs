using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using System.Threading;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public record PageInput(
	bool IncludeTotalCount,
	string? OrderBy,
	string? Filter,
	Partition? Partition,
	CancellationToken Token)
{
	// ReSharper disable once TooManyDependencies
	public PageInput(bool IncludeTotalCount,
	                 string? OrderBy,
	                 string? Filter,
	                 Partition? Partition)
		: this(IncludeTotalCount, OrderBy, Filter, Partition, CancellationToken.None) {}
}