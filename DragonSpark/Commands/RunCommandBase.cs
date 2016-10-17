using System;

namespace DragonSpark.Commands
{
	public abstract class RunCommandBase : DisposingCommandBase<object>, IRunCommand
	{
		protected RunCommandBase() {}

		protected RunCommandBase( IDisposable disposable ) : base( disposable ) {}

		public sealed override void Execute( object parameter ) => Execute();

		public abstract void Execute();
	}
}