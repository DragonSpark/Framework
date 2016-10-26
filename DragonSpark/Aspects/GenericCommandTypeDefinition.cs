using DragonSpark.Aspects.Build;
using DragonSpark.Commands;
using System.Windows.Input;

namespace DragonSpark.Aspects
{
	public sealed class GenericCommandTypeDefinition : ValidatedTypeDefinition
	{
		public static GenericCommandTypeDefinition Default { get; } = new GenericCommandTypeDefinition();
		GenericCommandTypeDefinition() : base( GenericCommandCoreTypeDefinition.Execute ) {}
	}

	public sealed class GenericCommandCoreTypeDefinition : TypeDefinitionWithPrimaryMethodBase
	{
		public static IMethodStore Execute { get; } = new MethodStore( typeof(ICommand<>), nameof(ICommand.Execute) );

		public static GenericCommandCoreTypeDefinition Default { get; } = new GenericCommandCoreTypeDefinition();
		GenericCommandCoreTypeDefinition() : base( Execute ) {}
	}
}