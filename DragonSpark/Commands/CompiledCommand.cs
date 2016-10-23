using System;
using System.Collections.Immutable;

namespace DragonSpark.Commands
{
	public class CompiledCommand<T> : CommandBase<ImmutableArray<T>>
	{
		readonly Action<T> command;
		public CompiledCommand( Action<T> command )
		{
			this.command = command;
		}

		public override void Execute( ImmutableArray<T> parameter )
		{
			foreach ( var item in parameter )
			{
				command( item );
			}
		}
	}
}