using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct NavigationDefinition(
	HashSet<NavigationRecord> Set,
	IReadOnlyDictionary<string, NavigationRecord> Map)
{
	public NavigationDefinition(HashSet<NavigationRecord> set) : this(set, set.ToDictionary(x => x.Name)) {}
}