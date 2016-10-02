using System.Windows.Input;

namespace DragonSpark.Commands
{
	public class AdapterCommand<T> : CommandBase<T>
	{
		readonly ICommand command;

		public AdapterCommand( ICommand command )
		{
			this.command = command;
		}

		public override void Execute( T parameter ) => command.Execute( parameter );
	}
}