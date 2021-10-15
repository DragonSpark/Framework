﻿namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public class QueryInput
{
	public bool IncludeTotalCount { get; set; }

	public string? OrderBy { get; set; }

	public string? Filter { get; set; }

	public Partition? Partition { get; set; }
}