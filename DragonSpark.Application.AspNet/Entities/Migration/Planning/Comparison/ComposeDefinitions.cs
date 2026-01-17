using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class ComposeDefinitions : ISelect<EntityDefinitionInput, ComparisonInput>
{
	public static ComposeDefinitions Default { get; } = new();

	ComposeDefinitions()
		: this(x => new(x.Name, x.ClrType),
		       x => new(x.Name, x.TargetEntityType, x.IsCollection, x.IsOnDependent)) {}

	readonly Func<IProperty, PropertyRecord>     _property;
	readonly Func<INavigation, NavigationRecord> _navigation;

	public ComposeDefinitions(Func<IProperty, PropertyRecord> property,
	                          Func<INavigation, NavigationRecord> navigation)
	{
		_property   = property;
		_navigation = navigation;
	}

	public ComparisonInput Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;

		var from = new EntityDefinition(source, source.GetFlattenedProperties().Select(_property).ToHashSet(),
		                                source.GetNavigations().Select(_navigation).ToHashSet());
		var to = new EntityDefinition(destination, destination.GetFlattenedProperties().Select(_property).ToHashSet(),
		                              destination.GetNavigations().Select(_navigation).ToHashSet());
		return new(from, to);
	}
}