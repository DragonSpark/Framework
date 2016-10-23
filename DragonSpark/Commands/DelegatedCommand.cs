using DragonSpark.Coercion;
using System;

namespace DragonSpark.Commands
{
	public class DelegatedCommand<T> : CommandBase<T>
	{
		readonly Action<T> command;

		public DelegatedCommand( Action<T> command )  : this( command, Coercer<T>.Default ) {}

		public DelegatedCommand( Action<T> command, ICoercer<T> coercer )  : base( coercer )
		{
			this.command = command;
		}

		public override void Execute( T parameter ) => command( parameter );
	}
}