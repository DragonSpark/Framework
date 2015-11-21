using System;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DisposableActionContext : IDisposable
	{
		readonly Action action;

		public DisposableActionContext( Action action )
		{
			this.action = action;
		}

		public void Dispose()
		{
			action();
		}
	}
}