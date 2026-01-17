using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class ComposeDefinitions : ISelect<EntityDefinitionInput, ComparisonInput>
{
	public static ComposeDefinitions Default { get; } = new();

	ComposeDefinitions() : this(ComposeDefinition.Default) {}

	readonly ISelect<IEntityType, EntityDefinition> _definition;

	public ComposeDefinitions(ISelect<IEntityType, EntityDefinition> definition) => _definition = definition;

	public ComparisonInput Get(EntityDefinitionInput parameter)
	{
		var (source, destination) = parameter;
		return new(_definition.Get(source), _definition.Get(destination));
	}
}