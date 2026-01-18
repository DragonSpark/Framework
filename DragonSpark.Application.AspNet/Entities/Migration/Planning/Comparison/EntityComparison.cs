using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class EntityComparison : IEntityComparison
{
	readonly ISelect<EntityDefinitionInput, ComparisonInput> _input;
	readonly ISelect<ComparisonInput, EntityModifications>   _compare;

	public EntityComparison(IEntityTypes types) 
		: this(ComposeDefinitions.Default, new EntityStructuralComparer(types)) {}

	public EntityComparison(ISelect<EntityDefinitionInput, ComparisonInput> input,
	                        ISelect<ComparisonInput, EntityModifications> compare)
	{
		_input   = input;
		_compare = compare;
	}

	public ModifiedEntityComparisonResult Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;
		var input      = _input.Get(parameter);
		var comparison = _compare.Get(input);
		return new(source, destination, comparison);
	}
}