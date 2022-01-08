using DragonSpark.Reflection.Members;
using LightInject;

namespace DragonSpark.Composition.Construction;

sealed class ConstructionInfoProvider : FieldDefinition<ServiceContainer>
{
	public static ConstructionInfoProvider Default { get; } = new();

	ConstructionInfoProvider() : base("constructionInfoProvider") {}
}