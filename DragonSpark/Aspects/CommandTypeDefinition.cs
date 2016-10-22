using DragonSpark.Commands;
using System.Windows.Input;

namespace DragonSpark.Aspects
{
	public sealed class CommandTypeDefinition : ValidatedTypeDefinition
	{
		public static CommandTypeDefinition Default { get; } = new CommandTypeDefinition();
		CommandTypeDefinition() : base( typeof(ICommand), nameof(ICommand.CanExecute), nameof(ICommand.Execute) ) {}
	}

	public sealed class RunTypeDefinition : ValidatedTypeDefinition
	{
		public static RunTypeDefinition Default { get; } = new RunTypeDefinition();
		RunTypeDefinition() : base( typeof(IRunCommand), nameof(IRunCommand.Execute) ) {}
	}
}