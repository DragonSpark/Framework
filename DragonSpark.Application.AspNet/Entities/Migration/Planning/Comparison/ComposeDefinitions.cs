using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class ComposeDefinitions : ISelect<EntityDefinitionInput, ComparisonInput>
{
	public static ComposeDefinitions Default { get; } = new();

	ComposeDefinitions() : this(x => new PropertyRecord(x.Name, x.ClrType.IsEnum ? typeof(Enum) : x.ClrType),
	                            x => new NavigationRecord(x.Name, x.TargetEntityType.ClrType.FullName.Verify(),
	                                                      x.IsCollection, x.IsOnDependent)) {}

	readonly Func<Microsoft.EntityFrameworkCore.Metadata.IProperty, PropertyRecord> _property;
	readonly Func<INavigation, NavigationRecord>                                    _navigation;

	public ComposeDefinitions(Func<Microsoft.EntityFrameworkCore.Metadata.IProperty, PropertyRecord> property,
	                          Func<INavigation, NavigationRecord> navigation)
	{
		_property   = property;
		_navigation = navigation;
	}

	public ComparisonInput Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;

		var from = new EntityDefinition(source.GetProperties().Select(_property).ToHashSet(),
		                                source.GetNavigations().Select(_navigation).ToHashSet());
		var to = new EntityDefinition(destination.GetProperties().Select(_property).ToHashSet(),
		                              destination.GetNavigations().Select(_navigation).ToHashSet());
		return new(from, to);
	}
}