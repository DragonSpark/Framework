using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class ComposeDefinition : ISelect<IEntityType, EntityDefinition>
{
	public static ComposeDefinition Default { get; } = new();

	ComposeDefinition() : this(x => new(x.Name, x.ClrType)) {}

	readonly Func<IKey, KeyRecord>               _key;
	readonly Func<IProperty, PropertyRecord>     _property;
	readonly Func<INavigation, NavigationRecord> _navigation;

	public ComposeDefinition(Func<IProperty, PropertyRecord> property)
		: this(x => new([..x.Properties.Select(property)]), property,
		       x => new(x.Name, x.TargetEntityType, x.IsCollection, x.IsOnDependent)) {}

	public ComposeDefinition(Func<IKey, KeyRecord> key, Func<IProperty, PropertyRecord> property,
	                         Func<INavigation, NavigationRecord> navigation)
	{
		_key        = key;
		_property   = property;
		_navigation = navigation;
	}

	public EntityDefinition Get(IEntityType parameter)
		=> new(parameter, parameter.GetKeys().Select(_key).ToImmutableHashSet(),
		       parameter.GetFlattenedProperties().Select(_property).ToImmutableHashSet(),
		       parameter.GetNavigations().Select(_navigation).ToImmutableHashSet());
}