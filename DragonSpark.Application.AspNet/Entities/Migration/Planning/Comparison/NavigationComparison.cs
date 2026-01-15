using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record NavigationComparison(
	IReadOnlyCollection<NavigationRecord> Added,
	IReadOnlyCollection<NavigationRecord> Removed,
	IReadOnlyCollection<NavigationRecord> Changed,
	uint Changes)
{
	public NavigationComparison(IReadOnlyCollection<NavigationRecord> Added,
	                            IReadOnlyCollection<NavigationRecord> Removed,
	                            IReadOnlyCollection<NavigationRecord> Changed)
		: this(Added, Removed, Changed, (uint)(Added.Count + Removed.Count + Changed.Count)) {}
}