using DragonSpark.Commands;
using System.Windows.Input;

namespace DragonSpark.Aspects
{
	public sealed class GenericCommandTypeDefinition : ValidatedTypeDefinition
	{
		public static GenericCommandTypeDefinition Default { get; } = new GenericCommandTypeDefinition();
		GenericCommandTypeDefinition() : base( typeof(ICommand<>), nameof(ICommand.Execute) ) {}
	}
}