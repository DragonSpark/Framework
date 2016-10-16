using System;

namespace DragonSpark.Commands
{
	public sealed class DisposingCommand : DisposingCommandBase<object>
	{
		public DisposingCommand( IDisposable disposable ) : base( disposable ) {}

		public override void Execute( object parameter ) {}
	}
}