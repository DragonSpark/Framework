using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct NavigationDefinition(
	IEntityType Owner,
	ImmutableHashSet<NavigationRecord> Set,
	IReadOnlyDictionary<string, NavigationRecord> Map)
{
	public NavigationDefinition(IEntityType owner, ImmutableHashSet<NavigationRecord> set)
		: this(owner, set, set.ToDictionary(x => x.Name)) {}
}