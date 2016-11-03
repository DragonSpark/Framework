using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Commands
{
	public class DelegatedCommand<T> : CommandBase<T>
	{
		readonly Action<T> command;

		public DelegatedCommand( Action<T> command )  : this( command, Coercer<T>.Default ) {}

		public DelegatedCommand( Action<T> command, IParameterizedSource<T> coercer )  : base( coercer )
		{
			this.command = command;
		}

		public override void Execute( T parameter ) => command( parameter );
	}
}