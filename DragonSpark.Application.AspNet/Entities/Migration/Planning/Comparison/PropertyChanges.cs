using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record PropertyChanges(
	ImmutableArray<PropertyRecord> Added,
	ImmutableArray<PropertyRecord> Removed,
	ImmutableArray<PropertyRecord> Modified,
	uint Changes)
{
	public PropertyChanges(ImmutableArray<PropertyRecord> Added,
	                          ImmutableArray<PropertyRecord> Removed,
	                          ImmutableArray<PropertyRecord> Modified)
		: this(Added, Removed, Modified, (uint)(Added.Length + Removed.Length + Modified.Length)) {}
}