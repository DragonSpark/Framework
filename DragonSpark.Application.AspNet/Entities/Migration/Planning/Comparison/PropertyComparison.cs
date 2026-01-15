using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed record PropertyComparison(
	IReadOnlyCollection<PropertyRecord> Added,
	IReadOnlyCollection<PropertyRecord> Removed,
	IReadOnlyCollection<PropertyRecord> Changed,
	uint Changes)
{
	public PropertyComparison(IReadOnlyCollection<PropertyRecord> Added,
	                          IReadOnlyCollection<PropertyRecord> Removed,
	                          IReadOnlyCollection<PropertyRecord> Changed)
		: this(Added, Removed, Changed, (uint)(Added.Count + Removed.Count + Changed.Count)) {}
}