using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record NavigationChanges(
	ImmutableArray<NavigationRecord> Added,
	ImmutableArray<NavigationRecord> Removed,
	ImmutableArray<NavigationRecord> Modified,
	uint Changes)
{
	public NavigationChanges(ImmutableArray<NavigationRecord> Added,
	                            ImmutableArray<NavigationRecord> Removed,
	                            ImmutableArray<NavigationRecord> Modified)
		: this(Added, Removed, Modified, (uint)(Added.Length + Removed.Length + Modified.Length)) {}
}