using System;

namespace DragonSpark.Runtime
{
	public class InitializedDisposableAction : DisposableAction
	{
		public InitializedDisposableAction( Action action ) : base( action )
		{
			action();
		}
	}
}