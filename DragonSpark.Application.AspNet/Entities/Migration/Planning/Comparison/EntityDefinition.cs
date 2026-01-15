using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public readonly record struct EntityDefinition(PropertyDefinition Properties, NavigationDefinition Navigations)
{
	public EntityDefinition(HashSet<PropertyRecord> properties,
	                        HashSet<NavigationRecord> navigation)
		: this(new PropertyDefinition(properties), new(navigation)) {}
}