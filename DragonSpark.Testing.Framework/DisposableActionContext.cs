using System;

namespace DragonSpark.Testing.Framework
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