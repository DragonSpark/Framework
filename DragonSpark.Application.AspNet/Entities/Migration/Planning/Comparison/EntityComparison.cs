using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed class EntityComparison : IEntityComparison
{
	readonly ISelect<EntityDefinitionInput, ComparisonInput>        _input;
	readonly ISelect<ComparePropertiesInput, PropertyComparison>    _properties;
	readonly ISelect<CompareNavigationsInput, NavigationComparison> _navigation;

	public EntityComparison(IEntityTypes types)
		: this(ComposeDefinitions.Default, CompareProperties.Default, new CompareNavigations(types)) {}

	public EntityComparison(ISelect<EntityDefinitionInput, ComparisonInput> input,
	                        ISelect<ComparePropertiesInput, PropertyComparison> properties,
	                        ISelect<CompareNavigationsInput, NavigationComparison> navigation)
	{
		_input      = input;
		_properties = properties;
		_navigation = navigation;
	}

	public ComparisonResult Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;
		var (from, to)            = _input.Get(parameter);
		var properties = _properties.Get(new(from.Properties, to.Properties));
		var navigation = _navigation.Get(new(from.Navigations, to.Navigations));
		return new(source, destination, properties, navigation);
	}
}