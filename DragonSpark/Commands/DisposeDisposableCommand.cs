using System;

namespace DragonSpark.Commands
{
	public sealed class DisposeDisposableCommand : DisposingCommand<object>
	{
		readonly IDisposable disposable;
		public DisposeDisposableCommand( IDisposable disposable )
		{
			this.disposable = disposable;
		}

		public override void Execute( object parameter ) {}

		protected override void OnDispose() => disposable.Dispose();
	}
}