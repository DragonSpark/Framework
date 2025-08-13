using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using System.Threading;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public record PageInput(
	bool IncludeTotalCount,
	string? OrderBy,
	string? Filter,
	Partition? Partition,
	CancellationToken Stop);