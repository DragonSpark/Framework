namespace DragonSpark.Contracts.Queries;

public record PageInput(bool IncludeTotalCount, string? OrderBy, string? Filter, Partition? Partition);