using System;

namespace DragonSpark.Runtime
{
	public class DisposableAction : DisposableBase
	{
		readonly Action action;

		public DisposableAction( Action action )
		{
			this.action = action;
		}

		protected override void OnDispose( bool disposing ) => action();
	}
}