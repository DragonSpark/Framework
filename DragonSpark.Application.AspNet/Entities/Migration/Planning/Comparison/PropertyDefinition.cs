using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct PropertyDefinition(
	IEntityType Owner,
	HashSet<PropertyRecord> Set,
	IReadOnlyDictionary<string, PropertyRecord> Map)
{
	public PropertyDefinition(IEntityType owner, HashSet<PropertyRecord> set)
		: this(owner, set, set.ToDictionary(x => x.Name)) {}
}