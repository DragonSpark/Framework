using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct PropertyDefinition(
	IEntityType Owner,
	ImmutableHashSet<PropertyRecord> Set,
	IReadOnlyDictionary<string, PropertyRecord> Map)
{
	public PropertyDefinition(IEntityType owner, ImmutableHashSet<PropertyRecord> set)
		: this(owner, set, set.ToDictionary(x => x.Name)) {}
}

public readonly record struct KeyDefinition(
	IEntityType Owner,
	ImmutableHashSet<KeyRecord> Set,
	IReadOnlyDictionary<string, KeyRecord> Map)
{
	public KeyDefinition(IEntityType owner, ImmutableHashSet<KeyRecord> set)
		: this(owner, set, set.ToDictionary(x => x.Signature)) {}
}
