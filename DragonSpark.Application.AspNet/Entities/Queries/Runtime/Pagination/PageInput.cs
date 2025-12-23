using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public sealed record PageInput(bool IncludeTotalCount, string? OrderBy, string? Filter, Partition? Partition);