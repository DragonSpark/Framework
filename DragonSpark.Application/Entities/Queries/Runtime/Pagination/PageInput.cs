using DragonSpark.Application.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public record PageInput(bool IncludeTotalCount, string? OrderBy, string? Filter, Partition? Partition);