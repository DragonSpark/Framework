using System.Windows.Input;

namespace DragonSpark.Commands
{
	public class FirstCommand<T> : CompositeCommand<T>
	{
		public FirstCommand( params ICommand[] commands ) : base( commands ) {}

		public override void Execute( T parameter )
		{
			foreach ( var command in Commands )
			{
				if ( command.CanExecute( parameter ) )
				{
					command.Execute( parameter );
					return;
				}
			}
		}
	}
}