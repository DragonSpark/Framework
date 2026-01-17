using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed class EntityComparison : IEntityComparison
{
	readonly ISelect<EntityDefinitionInput, ComparisonInput>            _input;
	readonly ISelect<ComparisonInput, EntityModifications> _compare;

	public EntityComparison(IEntityTypes types)
		: this(ComposeDefinitions.Default, new EntityStructuralComparer(types)) {}

	public EntityComparison(ISelect<EntityDefinitionInput, ComparisonInput> input,
	                        ISelect<ComparisonInput, EntityModifications> compare)
	{
		_input   = input;
		_compare = compare;
	}

	public EntityComparisonResult Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;
		var input      = _input.Get(parameter);
		var comparison = _compare.Get(input);
		return new(source, destination, comparison);
	}
}

sealed class EntityStructuralComparer : ISelect<ComparisonInput, EntityModifications>
{
	readonly ISelect<CompareKeysInput, KeyChanges>               _keys;
	readonly ISelect<ComparePropertiesInput, PropertyChanges>    _properties;
	readonly ISelect<CompareNavigationsInput, NavigationChanges> _navigation;

	public EntityStructuralComparer(IEntityTypes types)
		: this(CompareKeys.Default, CompareProperties.Default, new CompareNavigations(types)) {}

	public EntityStructuralComparer(ISelect<CompareKeysInput, KeyChanges> keys,
	                                ISelect<ComparePropertiesInput, PropertyChanges> properties,
	                                ISelect<CompareNavigationsInput, NavigationChanges> navigation)
	{
		_keys       = keys;
		_properties = properties;
		_navigation = navigation;
	}

	public EntityModifications Get(ComparisonInput parameter)
	{
		var (from, to) = parameter;
		var keys       = _keys.Get(new(from.Keys, to.Keys));
		var properties = _properties.Get(new(from.Properties, to.Properties));
		var navigation = _navigation.Get(new(from.Navigations, to.Navigations));
		return new(keys, properties, navigation);
	}
}