using DragonSpark.Contracts.Queries;
using System.Collections.Generic;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public sealed record Empty(
	IReadOnlyCollection<SearchFilter> Search,
	IReadOnlyCollection<WhereFilter> Where,
	IReadOnlyCollection<Sort> Sort,
	IReadOnlyCollection<string> Filter)
{
	public static Empty Default { get; } = new();

	Empty() : this([], [], [], []) {}
}