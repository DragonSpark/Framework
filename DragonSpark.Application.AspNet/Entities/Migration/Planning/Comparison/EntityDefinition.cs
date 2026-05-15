using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct EntityDefinition(
	KeyDefinition Keys,
	PropertyDefinition Properties,
	NavigationDefinition Navigations)
{
	// ReSharper disable once TooManyDependencies
	public EntityDefinition(IEntityType owner, ImmutableHashSet<KeyRecord> keys, ImmutableHashSet<PropertyRecord> properties,
	                        ImmutableHashSet<NavigationRecord> navigation)
		: this(new(owner, keys), new(owner, properties), new(owner, navigation)) {}
}