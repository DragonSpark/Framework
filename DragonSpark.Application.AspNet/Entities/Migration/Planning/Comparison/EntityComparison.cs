using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public sealed class EntityComparison : IEntityComparison
{
	readonly ISelect<EntityDefinitionInput, ComparisonInput> _input;
	readonly ISelect<ComparisonInput, PropertyComparison>    _properties;
	readonly ISelect<ComparisonInput, NavigationComparison>  _navigation;
	public static EntityComparison Default { get; } = new();

	EntityComparison() : this(ComposeDefinitions.Default, CompareProperties.Default, CompareNavigations.Default) {}

	public EntityComparison(ISelect<EntityDefinitionInput, ComparisonInput> input,
	                        ISelect<ComparisonInput, PropertyComparison> properties,
	                        ISelect<ComparisonInput, NavigationComparison> navigation)
	{
		_input      = input;
		_properties = properties;
		_navigation = navigation;
	}

	public ComparisonResult Get(EntityDefinitionInput parameter)
	{
		var (from, to) = parameter;
		var input      = _input.Get(parameter);
		var properties = _properties.Get(input);
		var navigation = _navigation.Get(input);
		return new(from, to, properties, navigation);
	}
}