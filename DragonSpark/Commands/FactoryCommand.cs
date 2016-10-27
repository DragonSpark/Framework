using System;

namespace DragonSpark.Commands
{
	public sealed class FactoryCommand<T> : RunCommandBase
	{
		readonly Func<T> factory;
		public FactoryCommand( Func<T> factory )
		{
			this.factory = factory;
		}

		public override void Execute() => factory();
	}
}
