using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record KeyChanges(
	ImmutableArray<KeyRecord> Added,
	ImmutableArray<KeyRecord> Removed,
	ImmutableArray<KeyRecord> Modified,
	uint Changes)
{
	public KeyChanges(ImmutableArray<KeyRecord> Added, ImmutableArray<KeyRecord> Removed,
	                  ImmutableArray<KeyRecord> Modified)
		: this(Added, Removed, Modified, (uint)(Added.Length + Removed.Length + Modified.Length)) {}
}