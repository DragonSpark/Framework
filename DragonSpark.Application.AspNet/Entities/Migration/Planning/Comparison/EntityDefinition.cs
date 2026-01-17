using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct EntityDefinition(PropertyDefinition Properties, NavigationDefinition Navigations)
{
	public EntityDefinition(IEntityType owner, HashSet<PropertyRecord> properties,
	                        HashSet<NavigationRecord> navigation)
		: this(new PropertyDefinition(owner, properties), new(owner, navigation)) {}
}