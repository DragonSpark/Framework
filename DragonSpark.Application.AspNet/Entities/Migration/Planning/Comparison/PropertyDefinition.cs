using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct PropertyDefinition(
	HashSet<PropertyRecord> Set,
	IReadOnlyDictionary<string, PropertyRecord> Map)
{
	public PropertyDefinition(HashSet<PropertyRecord> set) : this(set, set.ToDictionary(x => x.Name)) {}
}