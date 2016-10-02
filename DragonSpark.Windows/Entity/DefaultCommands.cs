using DragonSpark.Commands;
using System.Windows.Input;

namespace DragonSpark.Windows.Entity
{
	public sealed class DefaultCommands : CompositeCommand<DbContextBuildingParameter>
	{
		public static DefaultCommands Default { get; } = new DefaultCommands();
		DefaultCommands() : base( new ICommand[] { EnableLocalStoragePropertyCommand.Default, new RegisterComplexTypesCommand() } ) {}
	}
}