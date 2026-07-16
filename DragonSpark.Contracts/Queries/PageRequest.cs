using System.Collections.Generic;

namespace DragonSpark.Contracts.Queries;

public sealed record PageRequest(
	IReadOnlyCollection<SearchFilter> Search,
	IReadOnlyCollection<WhereFilter> Where,
	IReadOnlyCollection<Sort> Sorting,
	IReadOnlyCollection<string> Filters,
	bool IncludeTotalCount,
	Partition? Partition)
	: PageInput(IncludeTotalCount, null, null, Partition);