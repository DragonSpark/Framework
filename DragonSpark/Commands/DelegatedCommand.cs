using System;

namespace DragonSpark.Commands
{
	public class DelegatedCommand<T> : CommandBase<T>
	{
		readonly Action<T> command;

		public DelegatedCommand( Action<T> command ) 
		{
			this.command = command;
		}

		public override void Execute( T parameter ) => command( parameter );
	}
}