using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Commands
{
	public class SuppliedCommand<T> : RunCommandBase
	{
		readonly Action<T> command;
		readonly Func<T> parameter;

		public SuppliedCommand( ICommand<T> command, T parameter ) : this( command.ToDelegate(), parameter ) {}

		public SuppliedCommand( ICommand<T> command, Func<T> parameter ) : this( command.ToDelegate(), parameter ) {}

		public SuppliedCommand( Action<T> command, T parameter ) : this( command, Factory.For( parameter ) ) {}

		public SuppliedCommand( Action<T> command, Func<T> parameter ) : base( command.Target.AsDisposable() )
		{
			this.command = command;
			this.parameter = parameter;
		}

		public override void Execute() => command( parameter() );
	}
}