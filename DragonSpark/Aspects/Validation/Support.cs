using DragonSpark.Aspects.Build;
using System.Linq;

namespace DragonSpark.Aspects.Validation
{
	public sealed class Support : SupportDefinitionBase
	{
		public static Support Default { get; } = new Support();
		Support() : this( ParameterizedSourceTypeDefinition.Default, GenericCommandTypeDefinition.Default, CommandTypeDefinition.Default ) {}

		public Support( params IValidatedTypeDefinition[] definitions ) : base( SpecificationFactory.Default.Get( definitions ), definitions.SelectMany( AspectInstanceLocatorFactory.Default.Get ).ToArray() ) {}
	}
}